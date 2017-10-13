using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using KBEngine;
#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

public class PayCheckUIComponent : GameUIComponent {

	Transform backBtn;
	Transform payBtn;
	Transform refreshBtn;
	Transform moneyText;
	Transform accountLab;
	Transform goldLab;
	bool bSendPay = false;
//	[DllImport("__Internal")]
	private static extern void iospay(int price,string name);

	int[] limitArray = new int[]{30,50,100,200,500,1000};
	void Awake()
	{
		KBEngine.Event.registerOut ("onRefresh", this, "onRefresh");

		backBtn = transform.FindChild ("back_btn");
		payBtn = transform.FindChild ("rightPanel/pay_btn");
		moneyText = transform.FindChild ("rightPanel/Input");
		UIEventListener.Get (backBtn.gameObject).onClick = this.onClickBack;
		UIEventListener.Get (payBtn.gameObject).onClick = this.onClickPay;

		accountLab	= transform.FindChild ("leftPanel/account");
		goldLab	= transform.FindChild ("leftPanel/gold");
		refreshBtn = transform.FindChild("leftPanel/refreshBtn");
		accountLab.GetComponent<UILabel> ().text = Users.instance.Account;
		goldLab.GetComponent<UILabel> ().text = Users.instance.Gold.ToString("F2");
		UIEventListener.Get (refreshBtn.gameObject).onClick = this.onClickRefresh;

//		Jubaosdk.instance.init ();
	}

	public void onClickRefresh(GameObject obj){
		WaittingUIHandler.instance.Show ();
		KBEngineApp.app.player ().baseCall ("reqRefresh");
	}

	public void onClickPay(GameObject obj){
		WaittingUIHandler.instance.Show ();
		KBEngineApp.app.player ().baseCall ("reqRefresh");
		bSendPay = true;
	}
	public void onRefresh(string json){
		WaittingUIHandler.instance.UnShow ();
		JsonData jsonData = JsonMapper.ToObject (json);
		Users.instance.Gold = float.Parse(jsonData ["gold"].ToString());
		goldLab.GetComponent<UILabel> ().text = Users.instance.Gold.ToString("F2");

		if (bSendPay) {
			sendPay ();
			bSendPay = false;
		}
	}

	public void sendPay(){
		Debug.Log ("发起支付");

		string strAmount = moneyText.GetComponent<UIInput> ().value;
		if (strAmount.Equals ("")) {
			moneyText.FindChild("text").GetComponent<UILabel>().text = "请输入正确金额...";
			return;
		}
		int amount = int.Parse (strAmount);
		bool checkRight = false;
		for (int i = 0; i < limitArray.Length; i++) {
			if (limitArray [i] == amount) {
				checkRight = true;
			}
		}
		if (!checkRight) {
			moneyText.FindChild("text").GetComponent<UILabel>().text = "请输入正确金额...";
			moneyText.FindChild ("text").GetComponent<UILabel> ().color = Color.red;
			return;
		}

//		#if UNITY_ANDROID
//		Jubaosdk.instance.setAmount (strAmount);
//		#endif
//		#if UNITY_IPHONE
//
//		iospay(amount,Users.instance.Account);
//
//		Debug.Log("这里是苹果设备>_<");
//		#endif
		PayTypeHandler.instance.Show();
		PayTypeHandler.instance.getUIComponent ().setMoney (strAmount);
	}
		
	public void onClickBack(GameObject obj)
	{
		PayCheckUIHandler.instance.UnShow ();
		HallCenterUIHandler.instance.Show ();
	}

}
