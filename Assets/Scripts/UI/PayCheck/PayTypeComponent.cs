using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayTypeComponent : MonoBehaviour {

	GameObject btn_weixin;
	GameObject btn_alipay;
	GameObject btn_back;

	string money;

	void Awake () {
		btn_alipay = transform.FindChild ("UIGrid/alipay").gameObject;
		btn_weixin = transform.FindChild ("UIGrid/weixin").gameObject;
		btn_back	= transform.FindChild ("back").gameObject;

		btn_alipay.SetActive (Users.instance.use_pay_alipay);
		btn_weixin.SetActive (Users.instance.use_pay_weixin);

		transform.FindChild ("UIGrid").GetComponent<UIGrid> ().Reposition();

		UIEventListener.Get (btn_alipay).onClick = onClick;
		UIEventListener.Get (btn_weixin).onClick = onClick;
		UIEventListener.Get (btn_back).onClick = onClick;
	}
	public void setMoney(string money){
		this.money = money;
	}

	public void onClick(GameObject obj){
		if (obj == btn_alipay) {
			WapPay.instance.reqPay (2, Users.instance.Account, money);
		} else if (obj == btn_weixin) {
			WapPay.instance.reqPay (1, Users.instance.Account, money);
		}else if (obj == btn_back) {
			PayTypeHandler.instance.UnShow ();
		}
	}
}
