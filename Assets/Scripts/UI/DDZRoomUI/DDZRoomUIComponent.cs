using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using KBEngine;

public class DDZRoomUIComponent : GameUIComponent {

	public static int state;
	public static int roomtime;

	Transform topUI;
	Transform clock;
	Transform myCards;
	Transform needDestory;
	Transform tuoguanBtn;
	GameObject willObj;

	UILabel tipsLab;

	Dictionary<int,Entity> players = new Dictionary<int,Entity>();

	//是否重置场景
	bool bReset;
	public void resetGame()
	{
		bReset = true;
		players.Clear ();

		topUI.GetComponent<DDZTopUI> ().reset ();
		willObj.GetComponent<WillDo> ().reset ();
		myCards.GetComponent<DDZCardsManager> ().reset ();

		tipsLab.gameObject.SetActive (false);
		clock.gameObject.SetActive (false);
		willObj.gameObject.SetActive (false);
		tuoguanBtn.gameObject.SetActive (false);

		needDestory.DestroyChildren ();
	}

	void Awake()
	{
		DDZRoomWaitHandler.instance.Show ();
		topUI = transform.FindChild ("DDZTopUI");
		topUI.gameObject.AddComponent<DDZTopUI> ();

		willObj = transform.FindChild ("Willdo").gameObject;
		willObj.AddComponent<WillDo> ();
		willObj.SetActive (true);

		tipsLab = transform.FindChild ("Tips").GetComponent<UILabel> ();
		tuoguanBtn = transform.FindChild ("DDZTopUI/tuoguanBtn");
		UIEventListener.Get (tuoguanBtn.gameObject).onClick = cancelTuoGuan;

		needDestory = transform.FindChild("needDestory");

		clock = transform.FindChild ("Clock");
		clock.gameObject.AddComponent<Clock> ();
		clock.gameObject.SetActive (false);

		myCards = transform.FindChild ("myCard");
		myCards.gameObject.AddComponent<DDZCardsManager> ();

		DDZRoomWaitHandler.instance.Show ();

		AudioController.Instance.BGMPlay ("ddz/game_back2");

		installEvents();

		KBEngineApp.app.player ().baseCall ("reqEnterRoom", Users.instance.Addr);
	}

	void installEvents(){
		
		KBEngine.Event.registerOut ("onLeaveHall", this, "onLeaveHall");
		KBEngine.Event.registerOut ("onMessage", this, "onMessage");

		KBEngine.Event.registerOut ("onEnterWorld", this, "onEnterWorld");
		KBEngine.Event.registerOut ("onEnterSpace", this, "onEnterSpace");
		KBEngine.Event.registerOut ("onLeaveWorld", this, "onLeaveWorld");
		KBEngine.Event.registerOut ("onContinue", this, "onContinue");

		KBEngine.Event.registerOut ("onSetSpaceData", this, "onSetSpaceData");

		KBEngine.Event.registerOut ("set_cards", this, "set_cards");
		KBEngine.Event.registerOut ("set_cardCount", this, "set_cardCount");
		KBEngine.Event.registerOut ("set_type", this, "set_type");
		KBEngine.Event.registerOut ("set_multiple", this, "set_multiple");
		KBEngine.Event.registerOut ("set_tuoguan", this, "set_tuoguan");
		KBEngine.Event.registerOut ("set_goldC", this, "set_goldC");
		KBEngine.Event.registerOut ("set_curScore", this, "set_curScore");
		KBEngine.Event.registerOut ("set_showCards", this, "set_showCards");

		KBEngine.Event.resume();
	}
		
	DDZHead getHead(Entity entity){

		if (entity.renderObj == null) {
			GameObject obj = (GameObject)Instantiate (Resources.Load (GameSetting.UIPath + "DDZHead"), new Vector3(-99999,0,0), Quaternion.identity);	
			obj.transform.parent = needDestory;
			obj.transform.localScale = new Vector3 (1, 1, 1);
			obj.AddComponent<DDZHead> ();
			entity.renderObj = obj;

			int cid = (int)entity.getDefinedProperty ("cid");
			players [cid] = entity;
		}

		GameObject player = (GameObject)entity.renderObj;
		return player.GetComponent<DDZHead> ();

	}
	public void cancelTuoGuan(GameObject obj)
	{
		JsonData data = new JsonData ();
		data ["tuoguan"] = 0;
		string str = data.ToJson ();
		KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_TUOGUAN, str);
	}

	public void onEnterSpace(Entity entity){
		this.onEnterWorld (entity);
	}

	public void onEnterWorld(Entity entity){
//		Debug.LogError (entity.getDefinedProperty ("cid").ToString());

		if (entity.className == "Player") {
			int cid 	= (int)entity.getDefinedProperty ("cid");
			string head = entity.getDefinedProperty ("headC").ToString();
			string name = entity.getDefinedProperty ("nameC").ToString();
			string addr = entity.getDefinedProperty ("addrC").ToString();

			if (entity.isPlayer ()) {
				Users.instance.cid = cid;
			}
			bReset = false;
			this.getHead(entity).CID = cid;
			this.getHead(entity).Name = name;
			this.getHead(entity).Addr = addr;
			this.getHead(entity).Head = head;
			this.getHead (entity).Player = entity;
		}
	}
		
	public void onLeaveWorld(Entity entity){

		if (entity.className == "Player") {
			if (entity.isPlayer () && !bReset) {
				KBEngineApp.app.player ().baseCall ("reqLeaveHall");
			}

			if (state != 1 && entity.renderObj != null) {

				int cid = (int)entity.getDefinedProperty ("cid");
				players.Remove (cid);
				Destroy ((GameObject)entity.renderObj);
			}

			entity.renderObj = null;
		}
	}

	public void set_goldC(Entity entity){
		this.getHead(entity).Gold = entity.getDefinedProperty ("goldC").ToString();
	}
	public void set_cards(Entity entity){
		
		if (entity.isPlayer ()) {
			DDZRoomWaitHandler.instance.UnShow ();
			topUI.GetComponent<DDZTopUI> ().init ();

			List<object> cards = (List<object>)entity.getDefinedProperty ("cards");

			myCards.gameObject.GetComponent<DDZCardsManager> ().DeleAndUpdateCards (cards);
		}
	}

	public void set_cardCount(Entity entity){
		int cardCount = (int)entity.getDefinedProperty ("cardCount");
		this.getHead(entity).updateCardsCount (cardCount);
	}

	public void set_multiple(Entity entity){
		int multiple = (int)entity.getDefinedProperty ("multiple");
		this.getHead(entity).setMultiple(multiple);
	}

	public void set_type(Entity entity){
		
		int type = (int)entity.getDefinedProperty ("type");
		this.getHead(entity).setPlayerType(type);
	}

	public void set_tuoguan(Entity entity){
		
		byte tuoguan = (byte)entity.getDefinedProperty ("tuoguan");
		int cid	   = (int)entity.getDefinedProperty ("cid");

		this.getHead(entity).setTuoGuan (tuoguan == 0?false:true);
		if (cid == Users.instance.cid) {
			tuoguanBtn.gameObject.SetActive (tuoguan == 0?false:true);
			topUI.GetComponent<DDZTopUI> ().setTuoguan (tuoguan == 0 ? false : true);
		}
	}

	public virtual void set_curScore(Entity entity){
		int score = (int)entity.getDefinedProperty("curScore");
		this.getHead (entity).setSaybox (score);
		willObj.SetActive (false);
	}

	public virtual void set_showCards(Entity entity){
		List<object> showcards = (List<object>)entity.getDefinedProperty ("showCards");
		this.getHead (entity).setShowCards (showcards);
		willObj.SetActive (false);
	}

	public void onSetSpaceData(System.UInt32 spaceID, string key, string value){

		if (KBEngineApp.app.getSpaceData ("roomtime") != "")
		{
			roomtime = int.Parse (KBEngineApp.app.getSpaceData ("roomtime"));
		}
		if (KBEngineApp.app.getSpaceData ("state") != "")
		{
			state = int.Parse (KBEngineApp.app.getSpaceData ("state"));
		}

		if (key == "ACTION_ROOM_JIAOPAI_NEXT")
		{
			JsonData jsonData = JsonMapper.ToObject (KBEngineApp.app.getSpaceData ("ACTION_ROOM_JIAOPAI_NEXT"));
			int curCid  = (int)jsonData ["curCid"];
			int score	= (int)jsonData ["curScore"];
			int type	= (int)jsonData ["type"];

			clock.GetComponent<Clock> ().setClock (roomtime, curCid);
			getHead (players [curCid]).unshow ();
			if (curCid == Users.instance.cid) {
				if (type == 0) {
					willObj.GetComponent<WillDo> ().setEnableCenter (score);
					willObj.GetComponent<WillDo> ().setType (WillDo.Type_Jiaofen);
				} else {
					willObj.GetComponent<WillDo> ().setType (WillDo.Type_Jiabei);
				}
			}
		}
		else if (key == "ACTION_ROOM_NEXT") 
		{
			JsonData jsonData = JsonMapper.ToObject (KBEngineApp.app.getSpaceData ("ACTION_ROOM_NEXT"));
			int curCid		= (int)jsonData ["curCid"];
			int powerCid 	=	(int)jsonData ["powerCid"];
			JsonData powerCards	=	jsonData ["powerCards"];

			DDZRules.instance.powerID = powerCid;
			DDZRules.instance.powerCards = powerCards;

			clock.GetComponent<Clock> ().setClock (roomtime, curCid);
			getHead (players [curCid]).unshow ();

			JsonData cards_json = myCards.GetComponent<DDZCardsManager> ().getHandCardsJson ();
	
			if (Users.instance.cid != curCid)
				return;

			if (Users.instance.cid == powerCid) 
			{
				DDZTips.instance.initTips (cards_json, null);
				willObj.GetComponent<WillDo> ().setType (WillDo.Type_Power);
			}
			else
			{
				DDZTips.instance.initTips (cards_json, powerCards);
				willObj.GetComponent<WillDo> ().setType (WillDo.Type_NoPower);
			}
		}
	}

	public void onMessage(Entity entity,int action,string json)
	{
		JsonData jsonData = JsonMapper.ToObject (json);
		if (action == DDZConst.ACTION_ROOM_DISPATCH) 
		{
			myCards.GetComponent<DDZCardsManager> ().reset ();

			tipsLab.gameObject.SetActive (false);
			clock.gameObject.SetActive (false);
			willObj.gameObject.SetActive (false);

			foreach(Entity player in players.Values){
				this.getHead (player).unshow ();
			}
			
		}
		else if (action == DDZConst.ACTION_ROOM_COMPUTE) 
		{
			clock.gameObject.SetActive (false);

			GameObject statementsUI = (GameObject)Instantiate (Resources.Load (GameSetting.UIPath + "StatementsUI"), transform.position, Quaternion.identity);	
			statementsUI.transform.parent = needDestory;
			statementsUI.transform.localPosition = new Vector3 (0, 0, 0);
			statementsUI.transform.localScale = new Vector3 (1f, 1f, 1);
			StatementsUI compute_ui = statementsUI.AddComponent<StatementsUI> ();

			for (int i = 1; i <= 3; i++) {
				DDZHead head 	  = ((GameObject)this.players [i].renderObj).GetComponent<DDZHead>();
				Entity player	  = this.players[i];

				head.unshow ();

				float difen = float.Parse(jsonData["curfen"].ToString());
				int mult 	= (int)jsonData["multiple"];

				if (!player.isPlayer()) {
					JsonData cards = jsonData[i.ToString()]["cards"];
					head.showRemainCards (cards);
				}
				compute_ui.setData(player, jsonData [i.ToString ()], difen, mult);
			}
		}
	}
	public void onLeaveHall(){
		
		DDZRoomWaitHandler.instance.UnShow ();
		DDZRoomUIHandler.instance.UnShow ();
		GameSceneManager.instance.loadScene(SceneType.DDZHall);

	}

	public void onContinue(){
		this.resetGame ();
	}
	public void showTips(int type)
	{
		if (type == 1) {
			tipsLab.gameObject.SetActive (true);
			tipsLab.text = "当前所选牌型不符合规则";
		}
	}
	public void unShowTips()
	{
		tipsLab.gameObject.SetActive (false);
	}
	public void OnDestroy(){
		KBEngine.Event.deregisterOut (this);
	}
}