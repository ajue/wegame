namespace KBEngine{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System;
	using LitJson;
	//Player负责整个玩家账号信息
	public class Player : Entity {
		//登陆成功即由插件调用__init__
		public override void __init__(){
			if (!isPlayer ())
				return;
			KBEngine.Event.registerIn ("reqEnterHall", this, "reqEnterHall");
			KBEngine.Event.registerIn ("reqLeaveGame", this, "reqLeaveGame");
			KBEngine.Event.registerIn ("reqLeaveHall", this, "reqLeaveHall");
			KBEngine.Event.registerIn ("reqEnterRoom", this, "reqEnterRoom");
			KBEngine.Event.registerIn ("reqLeaveRoom", this, "reqLeaveRoom");
			KBEngine.Event.registerIn ("reqMessage", this, "reqMessage");

			//使用非缓存事件通知
			KBEngine.Event.nocacheFireOut ("onLoginSuccessfully", new object[]{this});
		}
	
		public virtual void set_goldC(object old){
			Event.fireOut("set_goldC", this);
		}
		public virtual void set_cards(object old){
			Event.fireOut("set_cards", this);
		}
		public virtual void set_cardCount(object old){
			Event.fireOut("set_cardCount", this);
		}
		public virtual void set_multiple(object old){
			Event.fireOut("set_multiple", this);
		}
		public virtual void set_type(object old){
			Event.fireOut("set_type", this);
		}
		public virtual void set_tuoguan(object old){
			Event.fireOut("set_tuoguan", this);
		}
		public virtual void set_curScore(object old){
			Event.fireOut("set_curScore", this);
		}
		public virtual void set_showCards(object old){
			Event.fireOut("set_showCards", this);
		}
		public void reqRefresh(){
			baseCall ("reqRefresh", new object[]{});
		}
		public void onAccessBank(int retcode,float gold,float bankGold){
			KBEngine.Event.fireOut ("onAccessBank", new object[]{ retcode, gold, bankGold });
		}
		public void reqContinue(){
			baseCall ("reqContinue", new object[]{ });
		}
		public void onContinue()
		{
			KBEngine.Event.fireOut ("onContinue");
		}

		//ddz进入房间返回chairID
		public void onEnterDDZRoom(int chairdID)
		{
			KBEngine.Event.fireOut ("onEnterDDZRoom", chairdID);
		}
			
		public void onRanksInfo(string json)
		{
			JsonData data = JsonMapper.ToObject (json);
			KBEngine.Event.fireOut ("onRanksInfo", json);
		}
	
		public void onMyRankInfo(string json)
		{
			JsonData data = JsonMapper.ToObject (json);
			KBEngine.Event.fireOut ("onMyRankInfo", json);
		}

		public void onNoticeInfos(String json)
		{
			KBEngine.Event.fireOut ("onNoticeInfos", json);
		}
	
		public void reqLeaveGame(){
			baseCall ("reqLeaveGame", new object[]{});
		}
		public void reqEnterHall(int hallID){
			baseCall("reqEnterHall",new object[]{hallID});
		}
		public void reqLeaveHall(){
			baseCall ("reqLeaveHall", new object[]{});
		}
		public void reqEnterRoom(string addr){
			baseCall ("reqEnterRoom", new object[]{addr});
		}
		public void reqLeaveRoom(){
			baseCall ("reqLeaveRoom", new object[]{});
		}
		public void onGameInfo(string list){
			KBEngine.Event.fireOut ("onGameInfo", new object[]{list});
		}
		public void onGamesConfig(string json){
			KBEngine.Event.fireOut ("onGamesConfig", new object[]{json});
		}
		public void onEnterGame(int gameID,string result){
			KBEngine.Event.fireOut ("onEnterGame", new object[]{gameID,result});
		}
		public void onLeaveGame(int gameID){
			KBEngine.Event.fireOut ("onLeaveGame", new object[]{});
		}
		public void onEnterHall(int hallID){
			KBEngine.Event.fireOut ("onEnterHall", new object[]{hallID});
		}
		public void onLeaveHall(int hallID){
			KBEngine.Event.fireOut ("onLeaveHall", new object[]{});
		}
		public void onRoomState(string json){
			KBEngine.Event.fireOut ("onRoomState", new object[]{json});
		}
		public void onEnterRoom(string json){
			KBEngine.Event.fireOut ("onEnterRoom", new object[]{json});
	    }
		public void onLeaveRoom(int retcode,int chairID){
			KBEngine.Event.fireOut ("onLeaveRoom", new object[]{retcode,chairID});
		}

		public void reqMessage(int action,string json){
			baseCall ("reqMessage", new object[]{action,json});
		}

		public void onMessage(int recode,int action,string json){
			KBEngine.Event.fireOut ("onMessage", new object[]{this,action,json});
		}
		public void onRefresh(string data){
			KBEngine.Event.fireOut ("onRefresh", new object[]{data});
		}
		public void onCash(int retcode,float gold,string alipay){
			KBEngine.Event.fireOut ("onCash", new object[]{ retcode,gold,alipay});
		}
		public void onCashInfo(int retcode,int gold,int fee){
			KBEngine.Event.fireOut ("onCashInfo", new object[]{ retcode,gold,fee });
		}
		public void onCharge(float amount,float gold){
			KBEngine.Event.fireOut ("onCharge", new object[]{gold});
		}
		public void onReviseProperties(int retcode,string name,int sexID,int headID)
		{
			KBEngine.Event.fireOut ("onReviseProperties", new object[]{retcode,name,sexID,headID});
		}

		public override void onDestroy(){
			KBEngine.Event.deregisterIn (this);
		}

	}
}
