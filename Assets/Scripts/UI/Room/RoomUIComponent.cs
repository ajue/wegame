using UnityEngine;
using System.Collections.Generic;
using LitJson;
using KBEngine;

public class RoomUIComponent : GameUIComponent {

	Dictionary<int,GameObject> chairObjDict = new Dictionary<int,GameObject>();
	Vector3[] chairVec = new Vector3[5];

	GameObject behaviorObj;
	GameObject topUIObj;
	GameObject clockObj;
	GameObject turnObj;
	GameObject backObj;
	GameObject pkObj;

	public static int makerID;
	float curRoomTime = 0;
	int curTurn = 0;

	void Awake()
	{
		chairVec [0] = new Vector3 (500, -100, 0);
		chairVec [1] = new Vector3 (400, 150, 0);
		chairVec [2] = new Vector3 (-500, 150, 0);
		chairVec [3] = new Vector3 (-400, -100, 0);
		chairVec [4] = new Vector3 (-50, -170, 0);

		KBEngine.Event.registerOut ("onLeaveHall", this, "onLeaveHall");
		KBEngine.Event.registerOut ("onEnterRoom", this, "onEnterRoom");
		KBEngine.Event.registerOut ("onLeaveRoom", this, "onLeaveRoom");
		KBEngine.Event.registerOut ("onRoomState", this, "onRoomState");
		KBEngine.Event.registerOut ("onMessage", this, "onMessage");

		behaviorObj = transform.FindChild ("RoomBehavior").gameObject;
		clockObj = transform.FindChild ("Clock").gameObject;
		topUIObj = transform.FindChild ("RoomTopUI").gameObject;
		backObj = transform.FindChild ("back_btn").gameObject;
		turnObj = transform.FindChild ("CurTurn").gameObject;
		pkObj	= transform.FindChild ("PanelPK").gameObject;

		clockObj.SetActive (false);
		behaviorObj.SetActive (false);

		UIEventListener.Get (backObj).onClick = delegate {
			KBEngine.Event.fireIn ("reqLeaveRoom", new object[]{ });
			WaittingUIHandler.instance.Show();
		};

		KBEngine.Event.fireIn ("reqEnterRoom", new object[]{Users.instance.Addr});
		AudioController.Instance.BGMPlay ("zjh2/bgroom");
	}
	public GameObject createChair(int chairID,string name,string addr,int headID,float curCost,float gold,int state,float limitTime,int sex){
		int index = this.computeIndex (chairID);

		GameObject go = Helper.primitiveLoad ("Prefabs/Chair", transform,chairVec[index]);
		Chair chair = go.AddComponent<Chair> ();
		chair.setProgressLimited (limitTime);
		chair.initPosition (index);
		chair.Cost = curCost;

		Head info = chair.getHeadObject ().GetComponent<Head> ();
		info.NickName = name;
		info.Addr = addr;
		info.Gold = gold.ToString();
		info.setHead (headID);

		chair.ChairID = chairID;
		chair.changeState(state);
		chair.Sex = sex;
		this.chairObjDict.Add (chairID, go);
		return go;
	}
	//获取room信息
	public void onRoomState(string json){

		JsonData data = JsonMapper.ToObject (json);
		RoomUIComponent.makerID = (int)data["makerID"];
		int state = (int)data["state"];
		int chairID = (int)data["chairID"];
		int limitTime = (int)data["limitTime"];
		float dizhu = float.Parse(data["dizhu"].ToString());
		float totalzhu  = float.Parse(data["totalzhu"].ToString());
		string jiazhuConfig = data ["jiazhuConfig"].ToJson();
		string addr = data ["addr"].ToString ();
		topUIObj.GetComponent<RoomTopUI> ().setData (totalzhu, dizhu);

		Users.instance.cid = chairID;
		Users.instance.Limitime = limitTime;
		GameObject chair = this.createChair(chairID,
			Users.instance.Nickname,
			addr,
			Users.instance.headID,
			0,
			Users.instance.Gold,
			state,
			limitTime,Users.instance.Sex);
		
		behaviorObj.GetComponent<RoomBehaviour> ().MyChair = chair;
		behaviorObj.GetComponent<RoomBehaviour> ().ChairDict = chairObjDict;
		behaviorObj.GetComponent<RoomBehaviour> ().setConfig (jiazhuConfig);

		//如果是灰色状态，则更新筹码
		if (state == ZJHConst.PLAYER_STATE_GARK) {
			JsonData chipMgr = data ["chipMgr"];
			for (int i=0; i<chipMgr.Count; i++) {
				float curzhu = float.Parse (chipMgr [i].ToString ());
				ChipManager.instance.addChip (curzhu, dizhu, new Vector3 (0, 50, 0));
			}
		}
	}
	public void onEnterRoom(string json){
		JsonData data = JsonMapper.ToObject (json);
		int 		chairID 	= (int)data ["chairID"];
		string 		name 		= (string)data["name"];
		string 		addr 		= (string)data["addr"];
		int 		state 		= (int)data["state"];
		float 		curCost 	= float.Parse(data ["curCost"].ToString());
		float 		gold 		= float.Parse(data["gold"].ToString());
		int 		head 		= (int)data["head"];
		int 		sexID 		= (int)data ["sex"];
		this.createChair (chairID, name,addr,head,curCost,gold, state,Users.instance.Limitime,sexID);

	}
	public void onLeaveRoom(int retcode,int chairID){
		WaittingUIHandler.instance.UnShow();

		GameObject chairObj = null;
		if(Users.instance.cid == chairID){
			chairObjDict.Remove(chairID);
			KBEngine.Event.fireIn("reqLeaveHall",new object[]{});
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
			//AudioController.Instance.BGMStop ();
		}
		else if (chairObjDict.TryGetValue (chairID,out chairObj)) {
			chairObjDict.Remove(chairID);
			Destroy(chairObj);
		}
		if (chairObjDict.Count < 2) {
			this.reset();
		}
	}

	public void reset(){
		RoomUIComponent.makerID = 0;

		foreach (GameObject go in chairObjDict.Values) {
			go.GetComponent<Chair>().reset();
		}
		this.stopClock();
		this.setTurn (0);
		this.topUIObj.GetComponent<RoomTopUI> ().reset ();
	}
	public void onLeaveHall(){
		RoomUIHandler.instance.UnShow ();
		GameSceneManager.instance.loadScene(SceneType.ZjhHall);
	}

	public void onMessage(KBEngine.Entity entity,int action,string json){
		Debug.Log ("Player.onMessage action = " + ZJHConst.ACTION_STRING[action] + " json:" + json);
		JsonData data = JsonMapper.ToObject (json);

		if (action == ZJHConst.ACTION_ROOM_DISPATCH) {
			this.clockObj.SetActive (false);
			float curzhu = float.Parse (data ["curzhu"].ToString ());
			foreach (GameObject go in chairObjDict.Values) {
				go.GetComponent<Chair> ().playCards (true);
				go.GetComponent<Chair> ().State = ZJHConst.PLAYER_STATE_START;
				ChipManager.instance.addChip (curzhu, curzhu, go.transform.position);
			}
		} else if (action == ZJHConst.ACTION_ROOM_TIME) {
			int room_time = (int)data ["room_time"];
			this.setClock (room_time);

		} else if (action == ZJHConst.ACTION_ROOM_START) {
			//to do
			int curChairID = (int)data ["curChairID"];
			int curTurn = (int)data ["curTurn"];

			this.setTurn (curTurn);
			chairObjDict [curChairID].GetComponent<Chair> ().resetProgress ();
			chairObjDict [curChairID].GetComponent<Chair> ().setMakerActive (true);

			this.behaviorObj.GetComponent<RoomBehaviour> ().reset ();
			this.behaviorObj.GetComponent<RoomBehaviour> ().setBehaviourState (3, false);
			this.behaviorObj.SetActive (true);
			if (Users.instance.cid != curChairID) {
				this.behaviorObj.GetComponent<RoomBehaviour> ().nomalReset (false);
			}
			AudioController.Instance.BGMPlay ("zjh2/bgplay");


		} else if (action == ZJHConst.ACTION_INFO_UPDATE) {
			int chairID = (int)data ["chairID"];
			float gold = float.Parse (data ["gold"].ToString ());
			float totalzhu = float.Parse (data ["totalzhu"].ToString ());
			float curzhu = float.Parse (data ["curzhu"].ToString ());
			float curCost = float.Parse (data ["curCost"].ToString ());

			Users.instance.Gold = gold;
			chairObjDict [chairID].GetComponent<Chair> ().Gold = gold;
			chairObjDict [chairID].GetComponent<Chair> ().Cost = curCost;

			topUIObj.GetComponent<RoomTopUI> ().setTotalzhu (totalzhu);
			topUIObj.GetComponent<RoomTopUI> ().Curzhu = curzhu;

		} else if (action == ZJHConst.ACTION_ROOM_NEXT) {
			int curChairID = (int)data ["curChairID"];
			int lastChairID = (int)data ["lastChairID"];
			int curTurn = (int)data ["curTurn"];
			this.setTurn (curTurn);
			if (Users.instance.cid == curChairID) {
				this.behaviorObj.SetActive (true);
				this.behaviorObj.GetComponent<RoomBehaviour> ().reset ();
				if (curTurn <= 1) {
					this.behaviorObj.GetComponent<RoomBehaviour> ().setBehaviourState (3, false);
				}
			}
			chairObjDict [lastChairID].GetComponent<Chair> ().finishProgress ();
			chairObjDict [curChairID].GetComponent<Chair> ().resetProgress ();
		} else if ( action == ZJHConst.ACTION_ROOM_KANPAI) {
			int chairID = (int)data ["chairID"];
			int state	= (int)data ["state"];
			if (Users.instance.cid == chairID) {
				string cards = data ["cards"].ToJson ();
				chairObjDict [chairID].GetComponent<Chair> ().changeState (state);
				chairObjDict [chairID].GetComponent<Chair> ().setCardsJson (cards, true);
			} else {
				chairObjDict [chairID].GetComponent<Chair> ().changeState (state);
			}
			int sexID = chairObjDict [chairID].GetComponent<Chair> ().Sex;
			AudioController.Instance.SoundZJHPlay ("kanpai", sexID);

		} else if ( action == ZJHConst.ACTION_ROOM_JIAZHU) {
			int chairID = (int)data ["chairID"];
			float curzhu = float.Parse (data ["curzhu"].ToString ());

			Vector3 from = chairObjDict [chairID].transform.localPosition;
			float dizhu = topUIObj.GetComponent<RoomTopUI> ().Dizhu;
			ChipManager.instance.addChip (curzhu, dizhu, from);
			int sexID = chairObjDict [chairID].GetComponent<Chair> ().Sex;
			AudioController.Instance.SoundZJHPlay ("jiazhu", sexID);
		} else if ( action == ZJHConst.ACTION_ROOM_GENZHU) {
			int chairID = (int)data ["chairID"];
			float curzhu = float.Parse (data ["curzhu"].ToString ());
			Vector3 from = chairObjDict [chairID].transform.localPosition;
			float dizhu = topUIObj.GetComponent<RoomTopUI> ().Dizhu;
			ChipManager.instance.addChip (curzhu, dizhu, from);
			int sexID = chairObjDict [chairID].GetComponent<Chair> ().Sex;
			AudioController.Instance.SoundZJHPlay ("genzhu", sexID);

		} else if ( action == ZJHConst.ACTION_ROOM_BIPAI_START) {
			//chair1是比牌发起人，chair2是被比牌人
			int chair1 = (int)data ["chair1"];
			int chair2 = (int)data ["chair2"];
			if (chair1 == Users.instance.cid) {
				AudioController.Instance.SoundZJHPlay ("bipai", Users.instance.Sex);
			}else if(chair2 == Users.instance.cid){
				int sexID = chairObjDict [chair1].GetComponent<Chair> ().Sex;
				AudioController.Instance.SoundZJHPlay ("bipai", sexID);
			}
			foreach (GameObject go in chairObjDict.Values) {
				go.GetComponent<Chair> ().setBipaiActive (false);
			}
			//统一播放比牌声音
		} else if ( action == ZJHConst.ACTION_ROOM_BIPAI_END) {
			int wID = (int)data ["wid"];
			int wState = (int)data ["wState"];
			string wCards = data ["wCards"].ToJson ();

			int lID = (int)data ["lid"];
			int lState = (int)data ["lState"];
			string lCards = data ["lCards"].ToJson ();
			chairObjDict [wID].GetComponent<Chair> ().changeState (wState);
			chairObjDict [lID].GetComponent<Chair> ().changeState (lState);

			if (Users.instance.cid == lID) {
				//如果玩家失败，则都显示牌，否则只显示对手的手牌
				this.behaviorObj.SetActive (false);
				//chairObjDict [wID].GetComponent<Chair> ().setCardsJson (wCards, true);
				//chairObjDict [lID].GetComponent<Chair> ().setCardsJson (lCards, true);
				AudioController.Instance.SoundZJHPlay ("bilose",2);

			} else if (Users.instance.cid == wID) {
				this.behaviorObj.GetComponent<RoomBehaviour> ().nomalReset (false);
				//chairObjDict [lID].GetComponent<Chair> ().setCardsJson (lCards, true);
				AudioController.Instance.SoundZJHPlay ("biwin",2);

			}
			foreach (GameObject go in chairObjDict.Values) {
				go.GetComponent<Chair> ().setBipaiActive (false);
			}
		} else if ( action == ZJHConst.ACTION_ROOM_QIPAI) {
			int chairID = (int)data ["chairID"];
			int state = (int)data ["state"];
			chairObjDict [chairID].GetComponent<Chair> ().changeState (state);
			if (Users.instance.cid == chairID) {
				this.behaviorObj.SetActive (false);
			}
		} else if ( action == ZJHConst.ACTION_ROOM_PUBLICH) {
			int chairID = (int)data ["chairID"];
			chairObjDict [chairID].GetComponent<Chair> ().setCardsJson (data ["cards"].ToJson (), true);
			chairObjDict [chairID].GetComponent<Chair> ().lookCards (true);
			this.behaviorObj.SetActive (false);

		} else if ( action == ZJHConst.ACTION_ROOM_SETTLE) {
			int victoryID = (int)data ["victoryID"];
			float addzhu = float.Parse (data ["addzhu"].ToString ());
			chairObjDict [victoryID].GetComponent<Chair> ().Gold = float.Parse (data ["gold"].ToString ());
			ChipManager.instance.reciveChip (chairObjDict [victoryID].transform.localPosition);
			this.behaviorObj.SetActive (false);
			AudioController.Instance.SoundZJHPlay ("luckygift",2);
		} else if ( action == ZJHConst.ACTION_ROOM_CLEARGAME) {
			this.reset ();
		} else if ( action == ZJHConst.ACTION_ROOM_KAIPAI_BEGIN) {
			pkObj.SetActive (true);
			pkObj.GetComponent<PanelPK>().play();
		}else if ( action == ZJHConst.ACTION_ROOM_KAIPAI_END) {
			for (int i = 0; i < data.Count; i++) {
				JsonData curData = data [i];
				int 	chairID 	= 	(int)curData ["chairID"];
				int 	state		=  	(int)curData ["state"];
				JsonData cards 		= 	curData ["cards"];

				chairObjDict [chairID].GetComponent<Chair> ().setCardsJson (cards.ToJson (), true);
				chairObjDict [chairID].GetComponent<Chair> ().lookCards (true);
				chairObjDict [chairID].GetComponent<Chair> ().changeState (state);
			}
			behaviorObj.SetActive (false);
		}
	}
	public void setTurn(int curTurn){
		this.curTurn = curTurn;
		this.turnObj.GetComponent<UILabel> ().text = "第 " + curTurn + " 轮";
	}
	public void stopClock(){
		this.clockObj.SetActive (false);
	}
	public void setClock(float roomTime){
		this.curRoomTime = roomTime;
		this.clockObj.SetActive (true);
		this.clockObj.GetComponent<UILabel> ().text = ((int)this.curRoomTime).ToString();
	}
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			KBEngineApp.app.player ().baseCall ("reqLeaveRoom");
		}
		this.updateClock ();
	}
	public void updateClock(){
		if (this.clockObj.activeSelf && this.curRoomTime>0) {
			this.curRoomTime -= Time.deltaTime;
			this.clockObj.GetComponent<UILabel> ().text = ((int)this.curRoomTime).ToString();
		}
	}
	//计算chairID 对应的座位index
	private int computeIndex(int chairID){
		if (Users.instance.cid == 0) {
			Debug.LogError ("UserInfo.chairID 未进行赋值.");
		} else {
			int offset1 = 5 - Users.instance.cid;
			int index = (chairID - 1 + offset1)%5;
			return index;
		}
		return 0;
	}
	public void OnDestroy(){
		Debug.Log ("KBEngine.Event.deregisterOut:" + gameObject.name);
		KBEngine.Event.deregisterOut (this);
	}
}
