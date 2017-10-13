using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using LitJson;

public class HallsecondUIComponent : MonoBehaviour {
	UIGrid grid;
	int gid;
	UISprite game_title;
	void Awake () {
		KBEngine.Event.registerOut ("onEnterGame", this, "onEnterGame");
		KBEngine.Event.registerOut ("onEnterHall", this, "onEnterHall");
		KBEngine.Event.registerOut ("onLeaveGame", this, "onLeaveGame");

		grid 		= transform.FindChild ("Grid").GetComponent<UIGrid>();
		game_title  = transform.FindChild ("title").GetComponent<UISprite> ();
		GameObject back_btn = transform.FindChild ("back_btn").gameObject;
		UIEventListener.Get (back_btn).onClick = delegate {
			KBEngineApp.app.player().baseCall("reqLeaveGame");
		};
	}
	public void reqEnterGame(int gid){
		this.gid = gid;
		game_title.spriteName = "game_title_" + this.gid;
		KBEngineApp.app.player ().baseCall ("reqEnterGame", gid);
	}

	public void onEnterGame(int gid,string result){

		WaittingUIHandler.instance.UnShow ();
		HallCenterUIHandler.instance.UnShow ();

		JsonData list = JsonMapper.ToObject (result);
		for (int i = 0 ; i<list.Count; i++)
		{
			JsonData dict = list [i];
			GameObject item = (GameObject)Instantiate (Resources.Load(GameSetting.UIItemPath + "HallItem"),transform.position,Quaternion.identity);			
			item.transform.parent = grid.transform;
			item.transform.localScale = new Vector3 (1, 1, 1);
			item.name = "hall" + i.ToString();

			GameHallData itemData = new GameHallData();
			itemData.type = (byte)(i + 1);
			if (gid == 1) {
				itemData.bg_name = "ddz_"+itemData.type+"_1";
			} else {
				itemData.bg_name = "zjh_"+itemData.type+"_1";
			}
			itemData.type 			= (int)dict ["id"];
			itemData.playerCount 	= (int)dict ["players_count"];
			itemData.difen 			= float.Parse(dict ["base"].ToString());
			itemData.xianzhi 		= float.Parse(dict ["limit"].ToString());

			item.AddComponent<HallItemUIComponent>().setData(itemData);
			UIEventListener.Get(item).onClick = onClickHall;
		}
		grid.Reposition ();
	}

	public void onEnterHall(int hallID){
		WaittingUIHandler.instance.UnShow ();
		if (hallID == -1) {
			//hallID = -1 金钱不足
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (2);
			box.setTitle ("系统提示");
			box.setContent ("抱歉，你的剩余金钱不足以进入该场次，请前往充值呢？");
			return;
		} else if (hallID == 0) {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (2);
			box.setTitle ("系统提示");
			box.setContent ("该账号已开始了对局，否是回到游戏中呢？");
			return;
		}
		if (gid == 1) {
			GameSceneManager.instance.loadScene (SceneType.DdzRoom);
		} else if(gid == 2) {
			GameSceneManager.instance.loadScene (SceneType.ZjhRoom);
		}

	}

	public void onClickHall(GameObject obj){

		GameHallData data = obj.GetComponent<HallItemUIComponent> ().HallData;
		KBEngine.Event.fireIn ("reqEnterHall", new object[]{ data.type });
		WaittingUIHandler.instance.Show ();
	}
		
	public void onLeaveGame(){
		Users.instance.GameID = GAME_ID.GAME_NONE;
		grid.transform.DestroyChildren ();
		HallsecondUIHandler.instance.UnShow ();
		HallCenterUIHandler.instance.Show ();
	}

	public void OnDestroy(){
		Debug.Log ("KBEngine.Event.deregisterOut:" + gameObject.name);
		KBEngine.Event.deregisterOut (this);
	}
}
