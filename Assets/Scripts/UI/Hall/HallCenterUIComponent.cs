using UnityEngine;
using System.Collections.Generic;
using KBEngine;
using System;
using LitJson;

public class HallCenterUIComponent : GameUIComponent {

	Transform game;
	Transform bottom;
	Transform setting;
	Transform qianzhuang;
	Transform head_btn;

	UILabel nameLab;
	UILabel goldLab;
	UISprite headSp;

	MoveNotice moveNotice;

	void Awake()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		KBEngine.Event.registerOut("onGameInfo", this, "onGameInfo");
		KBEngine.Event.registerOut("onGamesConfig", this, "onGamesConfig");
		KBEngine.Event.registerOut ("onNoticeInfos", this, "onNoticeInfos");
		KBEngine.Event.registerOut ("onRefresh", this, "onRefresh");
	
		this.game = transform.FindChild ("Game");
		this.bottom = transform.FindChild ("Bottom");
		this.setting = transform.FindChild ("Setting");
		this.head_btn = transform.FindChild ("Left/head_bg/Panel/head");

		headSp = head_btn.GetComponent<UISprite> ();
		nameLab = transform.FindChild ("Left/name").GetComponent<UILabel> ();
		goldLab = transform.FindChild ("Left/gold").GetComponent<UILabel> ();

		moveNotice = transform.FindChild ("MoveNotice/Panel").GetComponent<MoveNotice> ();

		UIEventListener.Get (setting.gameObject).onClick = this.onClickSetting;
		UIEventListener.Get (head_btn.gameObject).onClick = this.onClickHead;

		for (int i=0; i<game.childCount; i++) {
			GameObject obj = game.GetChild(i).gameObject;
			UIEventListener.Get (obj).onClick = this.onClickGame;
		}
		for (int i=0; i<bottom.childCount; i++) {
			GameObject obj = bottom.GetChild(i).gameObject;
			UIEventListener.Get (obj).onClick = this.onClickItem;
			if (obj.name == "duihuan") {
				obj.SetActive (Users.instance.use_ui_duihuan);
			}
		}

		if (!Users.instance.isReadConfig) {
			KBEngineApp.app.player ().baseCall ("reqGamesConfig");
		}

		KBEngineApp.app.bConnected = true;

		WebServer.instance.autoPositionIP();

		AudioController.Instance.BGMPlay("game/backgroundmusic");
	}

	void OnEnable(){
		KBEngineApp.app.player ().baseCall ("reqGameInfo");
		KBEngineApp.app.player ().baseCall ("reqNoticeInfos");

		this.localUpdateUI ();
	}
	void localUpdateUI(){
		Entity entity = KBEngineApp.app.player ();
		Users.instance.Nickname 	= entity.getDefinedProperty("name").ToString();
		Users.instance.Alipay 		= entity.getDefinedProperty("alipay").ToString ();
		Users.instance.headID 		= (int)entity.getDefinedProperty("head");
		Users.instance.Sex 			= (int)entity.getDefinedProperty("sex");
		Users.instance.Gold 		= (float)entity.getDefinedProperty ("gold");
		Users.instance.BankGold 	= (float)entity.getDefinedProperty("bankGold");

		this.nameLab.text 			= Users.instance.Nickname;
		this.goldLab.text 			= Users.instance.Gold.ToString ("F2");
		this.headSp.spriteName 		= Users.instance.headID.ToString ();
	
	}
	public void onRefresh(string json){
		JsonData jsonData = JsonMapper.ToObject (json);
		Users.instance.Gold = float.Parse(jsonData ["gold"].ToString());
		this.goldLab.text  = Users.instance.Gold.ToString("F2");
	}

	public void onNoticeInfos(string json)
	{
		JsonData data = JsonMapper.ToObject (json);
		moveNotice.setText(data ["moving"].ToString ());
	}
	public void onGameInfo(string result){
		Debug.Log ("onGameInfo = " + result);
		JsonData list = JsonMapper.ToObject (result);
		for (int i=0; i<list.Count; i++) {
			JsonData dict = list[i];
			string	name 			=	dict ["name"].ToString();
			int     playerCount 	= 	(int)dict["players_count"];
			byte 	open			= 	(byte)dict ["open"];

			if (open == 0) {
				transform.FindChild ("Game/" + name).gameObject.SetActive (false);
			}
			UILabel label = transform.FindChild("Game/"+name+"/Online").gameObject.GetComponent<UILabel>();
			label.text = playerCount.ToString ();
		}
	}
	public void onGamesConfig(string json){
		
		JsonData jsonData = JsonMapper.ToObject (json);
		Users.instance.base_money 		= float.Parse (jsonData ["base_money"].ToString ());
		Users.instance.duixian_base 	= float.Parse (jsonData ["duixian_base"].ToString ());
		Users.instance.duixian_base_fee = float.Parse (jsonData ["duixian_base_fee"].ToString ());
		Users.instance.duixian_add 		= float.Parse (jsonData ["duixian_add"].ToString ());
		Users.instance.duixian_add_fee 	= float.Parse (jsonData ["duixian_add_fee"].ToString ());

		Users.instance.use_pay_alipay = (bool)jsonData["use_pay_alipay"];
		Users.instance.use_pay_weixin = (bool)jsonData["use_pay_weixin"];
		Users.instance.use_ui_duihuan = (bool)jsonData["use_ui_duixian"];

		GameObject obj = this.bottom.FindChild ("duihuan").gameObject;
		if (obj.name == "duihuan") {
			obj.SetActive (Users.instance.use_ui_duihuan);
		}
	}
		
	public void onClickHead(GameObject obj)
	{
		PlayerCenterUIHandler.instance.Show ();
		HallCenterUIHandler.instance.UnShow ();
	}

	public void onClickSetting(GameObject obj)
	{
		WeSettingUIHandler.instance.Show ();
	}

	public void onClickItem(GameObject obj)
	{
		if (obj.name.Equals ("notice")) {
			NoticeUIHandler.instance.Show ();
			WaittingUIHandler.instance.Show ();
		}
		else if(obj.name.Equals ("rank"))
		{
			HallCenterUIHandler.instance.UnShow ();
			RankUIHandler.instance.Show ();
		}
		else if(obj.name.Equals ("bank"))
		{
			BankUIHandler.instance.Show ();
			HallCenterUIHandler.instance.UnShow ();
		}
		else if(obj.name.Equals ("duihuan"))
		{
			HallCenterUIHandler.instance.UnShow ();
			DuiHuanUIHandler.instance.Show ();
		}
		else if(obj.name.Equals ("shop"))
		{
			HallCenterUIHandler.instance.UnShow ();
			PayCheckUIHandler.instance.Show ();
		}
	}

	public void onClickGame(GameObject obj){
		int gameID = 0;
		if (obj.name.Equals ("ddz")) {
			gameID = 1;
		} else if(obj.name.Equals("zjh")){
			gameID = 2;
		}

		HallsecondUIHandler.instance.Show ();
		HallsecondUIHandler.instance.getUIComponent ().reqEnterGame (gameID);
		WaittingUIHandler.instance.Show ();
	}
	public void OnDestroy(){
		Debug.Log ("KBEngine.Event.deregisterOut:" + gameObject.name);
		KBEngine.Event.deregisterOut (this);
	}
}
