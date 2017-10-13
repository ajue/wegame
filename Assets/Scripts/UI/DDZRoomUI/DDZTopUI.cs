using UnityEngine;
using System;
using System.Collections;
using LitJson;
using KBEngine;

public class DDZTopUI : MonoBehaviour {

	Transform threeCards;
	Transform leaveBtn;
	UILabel difenLab;
	UILabel multLab;

	public float difen;
	public int beishu;

	GameObject boxTuoguan;

	void Awake()
	{
		threeCards = transform.FindChild ("DDZCards");
		threeCards.gameObject.SetActive (false);

		leaveBtn = transform.FindChild ("leaveBtn");
		UIEventListener.Get (leaveBtn.gameObject).onClick = onClickLeaveRoom;

		difenLab = transform.FindChild ("difen").GetComponent<UILabel> ();
		multLab = transform.FindChild ("beishu").GetComponent<UILabel> ();

		boxTuoguan = transform.FindChild ("BoxTuoguan").gameObject;
		boxTuoguan.SetActive (false);
		UIEventListener.Get (boxTuoguan).onClick = onBoxClick;

		KBEngine.Event.registerOut ("onSetSpaceData", this, "onSetSpaceData");

	}
	public void init(){
		boxTuoguan.SetActive (true);
	}
	public void setTuoguan(bool bState){
		boxTuoguan.GetComponent<UIToggle> ().value = bState;
	}
	public void onSetSpaceData(UInt32 spaceID, string key, string value){

		if (KBEngineApp.app.getSpaceData ("curfen") != "") {
			difenLab.text = "底分:" + float.Parse(KBEngineApp.app.getSpaceData ("curfen"));
		}
		multLab.text  = "倍数:" + KBEngineApp.app.getSpaceData ("multiple");

		if (KBEngineApp.app.getSpaceData ("threeCards") != "") {
			JsonData data = JsonMapper.ToObject(KBEngineApp.app.getSpaceData ("threeCards"));
			this.setThreeCards (data);
		}
	}

	public void reset(){
		threeCards.gameObject.SetActive (false);
		boxTuoguan.SetActive (false);
		boxTuoguan.GetComponent<UIToggle> ().value = false;
	}
	void onBoxClick(GameObject obj){
		JsonData data 		= new JsonData ();
		data ["tuoguan"] 	= obj.GetComponent<UIToggle> ().value?1:0;
		string json_str 	= data.ToJson ();
		KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_TUOGUAN, json_str);
	}
	public void setThreeCards(JsonData json)
	{
		if (json.Count < 0)
			return;
		
		threeCards.gameObject.SetActive (true);
		for (int i = 0; i < threeCards.childCount; i++)
		{
			threeCards.GetChild (i).gameObject.GetComponent<DDZCard> ().ID = (int)json[i];
			threeCards.GetChild (i).gameObject.GetComponent<DDZCard> ().setCardData ();
		}
	}

	public void onClickLeaveRoom(GameObject obj)
	{
		if (DDZRoomUIComponent.state == 1) {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (1);
			box.setTitle ("系统提示");
			box.setContent ("现在退出游戏将会由笨笨哒机器人替你打完这局哦！");
			box.setCallback (exitGame);
		} else {
			KBEngineApp.app.player ().baseCall ("reqLeaveRoom");
		}
	}
	void exitGame(){
		Application.Quit ();
	}
	public void setRoomData(float curDifen,int multiple){
		if (curDifen > 0) {
			this.difen = curDifen;
		}
		if (multiple > 0) {
			this.beishu = multiple;
		}
		if (difenLab.gameObject.activeSelf) {
			difenLab.text = "底分:" + this.difen.ToString ();
		} else {
			difenLab.gameObject.SetActive (true);
		}
		if (multLab.gameObject.activeSelf) {
			multLab.text = "倍数:" + this.beishu.ToString ();
		} else {
			multLab.gameObject.SetActive (true);
		}
	}
	public void OnDestroy(){
		KBEngine.Event.deregisterOut (this);
	}

}
