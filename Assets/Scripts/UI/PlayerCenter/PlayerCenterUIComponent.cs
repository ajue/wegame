using UnityEngine;
using System.Collections;
using KBEngine;

public class PlayerCenterUIComponent : GameUIComponent {

	GameObject btn_back;
	UISprite headSp;

	UILabel accountLab;
	UILabel goldLab;
	UILabel aliLab;

	UIInput inpt_name;
	GameObject btn_name;

	UIToggle boy;
	UIToggle girl;

	int headID;

	void Awake()
	{
		btn_back = transform.FindChild ("back_btn").gameObject;
		btn_name = transform.FindChild ("datas/btn_name").gameObject;

		UIEventListener.Get (btn_back).onClick = this.onClick;
		UIEventListener.Get (btn_name).onClick = this.onClick;

		GameObject btn_head = transform.FindChild ("head_bg/Panel/head").gameObject;
		UIEventListener.Get (btn_head).onClick = onClick;

		headSp 		= btn_head.GetComponent<UISprite> ();
		goldLab 	= transform.FindChild ("gold").GetComponent<UILabel> ();
		accountLab 	= transform.FindChild ("datas/account").GetComponent<UILabel> ();
		inpt_name 	= transform.FindChild ("datas/name").GetComponent<UIInput> ();
		aliLab		= transform.FindChild ("datas/alipay").GetComponent<UILabel> ();
		boy 		= transform.FindChild ("box_boy").GetComponent<UIToggle> ();
		girl 		= transform.FindChild ("box_girl").GetComponent<UIToggle> ();

		accountLab.text = Users.instance.Account;
		inpt_name.value 	= Users.instance.Nickname;
		goldLab.text 	= Users.instance.Gold.ToString("F2");
		aliLab.text		= Users.instance.Alipay;
		headSp.spriteName = Users.instance.headID.ToString ();

		boy.value = Users.instance.Sex == 1 ? true : false;
		girl.value = Users.instance.Sex == 2 ? true : false;

		EventDelegate.Add(inpt_name.onChange,onChange);

		KBEngine.Event.registerOut ("onReviseProperties", this, "onReviseProperties");
	}

	void onChange(){
		//限制名字长度为7个
		if (inpt_name.value.Length > 7) {
			string name = inpt_name.value.Substring(0,7);
			inpt_name.value = name;
		}
	}

	public void onReviseProperties(int retcode,string name,int sexID,int headID)
	{
		WaittingUIHandler.instance.UnShow ();
		Users.instance.Nickname = name;
		Users.instance.Sex = sexID;
		Users.instance.headID = headID;
		headSp.spriteName  = Users.instance.headID.ToString ();

	}

	public void onClick(GameObject obj)
	{
		if (obj == btn_back) {
			PlayerCenterUIHandler.instance.UnShow ();
			HallCenterUIHandler.instance.Show ();
		} else if (obj == btn_name) {
			WaittingUIHandler.instance.Show ();
			KBEngineApp.app.player().baseCall("reqReviseProperties",inpt_name.value,Users.instance.Sex,Users.instance.headID);
		}
		else {
			PlayerCenterUIHandler.instance.UnShow ();
			HeadSelectHandler.instance.Show ();
		}
	}

	public void OnDestroy(){
		KBEngine.Event.deregisterOut (this);
	}
}
