using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using KBEngine;

public class StatementsUI : MonoBehaviour {
	
	UILabel jieguo_lbl;
	Transform id1;
	Transform id2;
	Transform id3;
	Transform leave_btn;
	Transform goOn_btn;

	Dictionary<int,Transform> dataList = new Dictionary<int,Transform>();
	void Awake()
	{
		KBEngine.Event.registerOut ("onContinue", this, "onContinue");

		jieguo_lbl = transform.FindChild ("jieguo").GetComponent<UILabel> ();
		jieguo_lbl.color = Color.red;

		id1 = transform.FindChild ("PlayerInfo/id1");
		dataList.Add (1, id1);
		id2 = transform.FindChild ("PlayerInfo/id2");
		dataList.Add (2,id2);
		id3 = transform.FindChild ("PlayerInfo/id3");
		dataList.Add (3,id3);

		leave_btn = transform.FindChild ("leave");
		goOn_btn = transform.FindChild ("goOn");

		UIEventListener.Get (leave_btn.gameObject).onClick = OnClickLeave;
		UIEventListener.Get (goOn_btn.gameObject).onClick = OnClickGoOn;
	}
		
	public void setData(Entity player,JsonData jsonData,float difen,int roomMult){

		int cid 			= (int)player.getDefinedProperty ("cid");
		int mult			= (int)player.getDefinedProperty ("multiple");
		int type 			= (int)player.getDefinedProperty ("type");
		string name 		= player.getDefinedProperty ("name").ToString();
		float gold 			= float.Parse (jsonData ["gold"].ToString ());
		float settleGold	= float.Parse(jsonData ["settleGold"].ToString());

		Color color 	= player.isPlayer()?Color.yellow:Color.white;
		string typeName = (type == 1) ? "地主" : "农民";

		if(player.isPlayer()){
			jieguo_lbl.text = (settleGold > 0) ? "胜利":"失败";
			jieguo_lbl.color = (settleGold > 0) ? Color.red : Color.yellow;
		}

		this.setList (cid,name,difen,settleGold,gold, typeName, roomMult*mult, color);
	}

	void setList(int cid,string name,float difen,float settleGold,float gold,string type,int mult,Color color){

		dataList[cid].FindChild ("name").GetComponent<UILabel> ().color = color;
		dataList[cid].FindChild ("difen").GetComponent<UILabel> ().color = color;
		dataList[cid].FindChild ("gold").GetComponent<UILabel> ().color = color;
		dataList[cid].FindChild ("identity").GetComponent<UILabel> ().color = color;
		dataList[cid].FindChild ("multiple").GetComponent<UILabel> ().color = color;

		dataList[cid].FindChild ("name").GetComponent<UILabel> ().text = name;
		dataList[cid].FindChild ("identity").GetComponent<UILabel> ().text = type;
		dataList [cid].FindChild ("difen").GetComponent<UILabel> ().text = difen.ToString ();
		dataList[cid].FindChild ("gold").GetComponent<UILabel> ().text = settleGold.ToString ();
		dataList[cid].FindChild ("multiple").GetComponent<UILabel> ().text = mult.ToString ();
	}
	public void OnClickLeave(GameObject obj)
	{
		WaittingUIHandler.instance.Show ();
		KBEngineApp.app.player ().baseCall ("reqLeaveRoom");
	}
	public void OnClickGoOn(GameObject obj)
	{
		WaittingUIHandler.instance.Show ();
		KBEngineApp.app.player ().baseCall ("reqContinue");
	}
	public void onContinue()
	{
		WaittingUIHandler.instance.UnShow ();
		DDZRoomWaitHandler.instance.Show ();
		DDZRoomUIHandler.instance.getUIComponent ().resetGame ();
	}
	void OnDestory()
	{
		KBEngine.Event.deregisterOut (this);
	}
}
