using UnityEngine;
using System.Collections;
using KBEngine;

public class DuiHuanUIComponent : GameUIComponent {

	UILabel hasGoldLab;
	Transform back;
	GameObject sureBtn;

	UIInput amountInput;
	UIInput alipayInput;

	int amount;
	string alipay;

	void Awake()
	{
		back = transform.FindChild ("back_btn");
		sureBtn = transform.FindChild ("rightPanel/btn").gameObject;

		UIEventListener.Get (back.gameObject).onClick = this.onClickBack;
		UIEventListener.Get (sureBtn).onClick = this.onClickSure;

		hasGoldLab = transform.FindChild ("leftPanel/gold").GetComponent<UILabel> ();
		amountInput = transform.FindChild ("rightPanel/InputJinbi").GetComponent<UIInput> ();
		alipayInput = transform.FindChild ("rightPanel/InputAlipay").GetComponent<UIInput> ();

		UILabel tips = transform.FindChild ("leftPanel/text").GetComponent<UILabel> ();
		tips.text = "兑换后账号余额不能低于"+Users.instance.base_money+"元。" +
			"提现"+Users.instance.duixian_base+"元以内，收取"+
			Users.instance.duixian_base_fee+" 元手续费。"+
			"超过"+Users.instance.duixian_base+"元,"+
			"每"+Users.instance.duixian_add+"元内,收取"+Users.instance.duixian_add_fee+
			"元手续费。\n兑换成功后30分钟内到账，如果未能到账的玩家，请及时与客服取得联系.";

		KBEngine.Event.registerOut ("onCashInfo", this, "onCashInfo");
		KBEngine.Event.registerOut ("onCash", this, "onCash");
	}

	public void OnEnable(){
		hasGoldLab.text = Users.instance.Gold.ToString ("F2");
		alipayInput.value = Users.instance.Alipay;
//		phoneInput.value = Users.instance.Phone;
	}
	public void onClickSure(GameObject obj)
	{
		if (amountInput.value.Equals ("")
		    || alipayInput.value.Equals ("")
			|| amountInput.value.Equals ("")) {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (2);
			box.setTitle ("兑现失败");
			box.setContent ("请填写完整正确信息！！！");

		} else {
			WaittingUIHandler.instance.Show ();
			this.amount = int.Parse(amountInput.value);
			KBEngineApp.app.player ().baseCall ("reqCashInfo", this.amount);
		}
	}

	public void onClickSend(){
		WaittingUIHandler.instance.Show ();
		KBEngineApp.app.player ().baseCall ("reqCash", this.amount,this.alipay);
	}
	public void onClickBack(GameObject obj)
	{
		DuiHuanUIHandler.instance.UnShow ();
		HallCenterUIHandler.instance.Show ();
	}
	public void onCashInfo(int retcode,int gold,int fee){
		WaittingUIHandler.instance.UnShow ();

		if (retcode == -1 || retcode == -2) {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (2);
			box.setTitle ("兑现失败");
			if (retcode == -2) {
				box.setContent ("兑现金额过低！！！");
			} else {
				box.setContent ("兑现后用户余额不能低于" + Users.instance.base_money + "元！！！");
			}
		} else {
			BoxUIHandler.instance.Show ();
			string content = "兑现金额:" + gold + "\n" +
				"实际到账:" + (gold-fee) + "\n" +
				"支付宝:" + alipayInput.value + "\n"+
				"手机号:" + Users.instance.Account;

			this.amount = int.Parse(amountInput.value);
			this.alipay = alipayInput.value;
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (1);
			box.setTitle ("请确认兑换信息");
			box.setContent (content);
			box.setCallback (onClickSend);
		}
	}
	public void onCash(int retcode,float gold,string alipay){
		WaittingUIHandler.instance.UnShow ();
		if (retcode == -1) {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (2);
			box.setTitle ("兑现失败");
			box.setContent ("兑现后用户余额不能低于"+Users.instance.base_money+"元！！！");
			return;
		} else if (retcode == 0) {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (2);
			box.setTitle ("兑现成功");
			box.setContent ("兑换金额将转账至\n 支付宝:"+alipay+"\n 请注意查收。\n 如有疑问，请联系客服。");
		}
		Users.instance.Gold = gold;
		Users.instance.Alipay = alipay;
		hasGoldLab.text = Users.instance.Gold.ToString ();
		Debug.Log ("onCash retcode = " + retcode);
	}
}
