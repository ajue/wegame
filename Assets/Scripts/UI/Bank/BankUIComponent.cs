using UnityEngine;
using System.Collections;
using KBEngine;

public class BankUIComponent : GameUIComponent {

	Transform back;
	UILabel  goldNum_lbl;
	UILabel bankNum_lbl;
	GameObject sureBtn;
	Transform cunQian;
	Transform quQian;
	UILabel tipsLabel;

	Transform selectBox;
	int type = 0;

	void Awake()
	{
		tipsLabel = transform.FindChild ("rightPanel/Input/Label").GetComponent<UILabel> ();
		cunQian = transform.FindChild ("rightPanel/cun");
		quQian = transform.FindChild ("rightPanel/qu");
		selectBox = transform.FindChild ("rightPanel/selectBox").transform;

		sureBtn = transform.FindChild ("rightPanel/btn").gameObject;
		goldNum_lbl = transform.FindChild ("leftPanel/goldNum").GetComponent<UILabel> ();
		bankNum_lbl = transform.FindChild ("leftPanel/bankNum").GetComponent<UILabel> ();
		goldNum_lbl.text = Users.instance.Gold.ToString ();
		bankNum_lbl.text = Users.instance.BankGold.ToString ();
		back = transform.FindChild ("back_btn");

		UIEventListener.Get (back.gameObject).onClick = this.onClickBack;
		UIEventListener.Get (sureBtn).onClick = this.onCLickSure;
		UIEventListener.Get (cunQian.gameObject).onClick = onClickCunBtn;
		UIEventListener.Get (quQian.gameObject).onClick = onClickQuBtn;

		KBEngine.Event.registerOut ("onAccessBank", this, "onAccessBank");
		this.init ();
	}
	public void init()
	{
		
		type = 1;
		tipsLabel.text = "要存入的金额数";
		tipsLabel.color = Color.green;
		selectBox.localPosition = new Vector3 (278, 70, 0);
	}
	public void onClickCunBtn(GameObject obj)
	{
		type = 1;
		tipsLabel.text = "要存入的金额数";
		tipsLabel.color = Color.green;
		selectBox.localPosition = new Vector3 (278, 70, 0);
	}
	public void onClickQuBtn(GameObject obj)
	{
		type = 2;
		tipsLabel.text = "要取出的金额数";
		tipsLabel.color = Color.yellow;
		selectBox.localPosition = new Vector3 (282, -130, 0);
//		transform.FindChild ("rightPanel/Input").GetComponent<UIInput> ().value = "";
	}
		
	public void onAccessBank(int retCode,float gold,float bankGold)
	{
		WaittingUIHandler.instance.UnShow ();
		//retcode 0存取成功，-1存取失败
		if (retCode == 0) {
			goldNum_lbl.text = gold.ToString ("F2");
			bankNum_lbl.text = bankGold.ToString ("F2");
			Users.instance.Gold = gold;
			Users.instance.BankGold = bankGold;
			transform.FindChild ("rightPanel/Input").GetComponent<UIInput> ().value = "";
		} else {
			return;
		}


	}
	public void onCLickSure(GameObject obj)
	{
		//参数1为1时为存钱，2为取钱
		string tempvalue = transform.FindChild("rightPanel/Input").GetComponent<UIInput>().value;
		if (tempvalue.Equals ("")) {
			return;
		}
		float tempNum = float.Parse (tempvalue);
		KBEngineApp.app.player ().baseCall ("reqAccessBank",type,tempNum);
		WaittingUIHandler.instance.Show ();
	}
	public void onClickBack(GameObject obj)
	{
		BankUIHandler.instance.UnShow ();
		HallCenterUIHandler.instance.Show ();
	}
}
