using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class HeadSelectUIComponent : MonoBehaviour {

	UISprite sp_head;
	UIGrid gird;

	void Awake () {
		KBEngine.Event.registerOut ("onReviseProperties", this, "onReviseProperties");

		sp_head = transform.FindChild ("head_bg/Panel/head").GetComponent<UISprite> ();
		sp_head.spriteName = Users.instance.headID.ToString ();

		gird = transform.FindChild ("UIGrid").GetComponent<UIGrid> ();
		foreach(Transform child in gird.transform){
			child.FindChild ("head_bg/Panel/head").GetComponent<UISprite> ().spriteName = child.name;
			UIEventListener.Get (child.gameObject).onClick = onToggleClick;
			if (child.name == Users.instance.headID.ToString()) {
				child.GetComponent<UIToggle> ().startsActive = true;
			}
		}

		GameObject btn_use = transform.FindChild ("use_btn").gameObject;
		GameObject btn_back = transform.FindChild ("back_btn").gameObject;

		UIEventListener.Get (btn_use).onClick = onClick;
		UIEventListener.Get (btn_back).onClick = onClick;

	}

	void onToggleClick(GameObject obj){
		sp_head.spriteName = obj.name;
	}

	void onClick(GameObject obj){
		if (obj.name == "use_btn") {
			int head = int.Parse (sp_head.spriteName);
			WaittingUIHandler.instance.Show ();
			KBEngineApp.app.player().baseCall("reqReviseProperties",Users.instance.Nickname,Users.instance.Sex,head);

		} else if (obj.name == "back_btn") {
			HeadSelectHandler.instance.UnShow ();
			PlayerCenterUIHandler.instance.Show ();
		}
	}

	public void onReviseProperties(int retcode,string name,int sexID,int headID)
	{
		WaittingUIHandler.instance.UnShow ();
		Users.instance.Nickname = name;
		Users.instance.Sex = sexID;
		Users.instance.headID = headID;

		HeadSelectHandler.instance.UnShow ();
		PlayerCenterUIHandler.instance.Show ();
	}

	public void OnDestroy(){
		KBEngine.Event.deregisterOut (this);
	}

}
