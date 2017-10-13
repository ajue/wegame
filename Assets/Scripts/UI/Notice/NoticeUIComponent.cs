using UnityEngine;
using System.Collections;
using LitJson;
using KBEngine;

public class NoticeUIComponent : GameUIComponent {

	Transform btn;
	UILabel txt;

	void Awake()
	{
		btn = transform.FindChild ("btn");
		UIEventListener.Get (btn.gameObject).onClick = this.onClickBack;
		txt = transform.FindChild ("text").GetComponent<UILabel> ();
		KBEngine.Event.registerOut ("onNoticeInfos", this, "onNoticeInfos");

		KBEngineApp.app.player ().baseCall ("reqNoticeInfos");
	}

	// Update is called once per frame
	void Update () {

	}
	public void onClickBack(GameObject obj)
	{
		NoticeUIHandler.instance.UnShow ();
		HallCenterUIHandler.instance.Show ();
	}
	public void onNoticeInfos(string json)
	{
		WaittingUIHandler.instance.UnShow();
		JsonData jsonData = JsonMapper.ToObject (json);
		txt.text = jsonData ["static"].ToString();
	}
}
