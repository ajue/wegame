using UnityEngine;
using System.Collections;
using LitJson;

public class Chair:MonoBehaviour{

	int chairID = 0;
	int state = 0;
	float gold = 0f;
	bool isLookCard = false;
	int sexID = 0;

	GameObject cardsObj = null;
	GameObject headObj = null;
	GameObject speakObj = null;
	GameObject bipaiObj = null;

	Vector3[] cardsVec = new Vector3[5];
	Vector3[] speakVec = new Vector3[5];

	void Awake(){
		cardsVec [0] = new Vector3 (-150,0,0);
		cardsVec [1] = new Vector3 (-150,0,0);
		cardsVec [2] = new Vector3 (150,0,0);
		cardsVec [3] = new Vector3 (150,0,0);
		cardsVec [4] = new Vector3 (160,0,0);

		speakVec [0] = new Vector3 (-125,80,0);
		speakVec [1] = new Vector3 (-125,80,0);
		speakVec [2] = new Vector3 (125,80,0);
		speakVec [3] = new Vector3 (125,80,0);
		speakVec [4] = new Vector3 (125,80,0);

		cardsObj = Helper.primitiveLoad ("Prefabs/Cards", transform, Vector3.zero);
		headObj = Helper.primitiveLoad ("Prefabs/Head", transform);
		speakObj = Helper.primitiveLoad ("Prefabs/Speak", transform, Vector3.zero);

		Cards cards = cardsObj.AddComponent<Cards> ();
		Head head = headObj.AddComponent<Head> ();

		bipaiObj = head.BipaiObject ();

		speakObj.SetActive (false);
		cardsObj.SetActive (false);
		bipaiObj.SetActive (false);

		UIEventListener.Get (bipaiObj).onClick = onClickBipai;
	}

	public void reset(){
		this.state = 0;
		this.isLookCard = false;
		speakObj.SetActive (false);
		cardsObj.SetActive (false);
		cardsObj.GetComponent<Cards> ().play (false);
		cardsObj.GetComponent<Cards> ().lookCards (false);
		bipaiObj.SetActive (false);
		headObj.GetComponent<Head> ().setGarkActive (false);
		headObj.GetComponent<Head> ().setMakerActive (false);
		headObj.GetComponent<Head> ().setChipCost (0);
		headObj.GetComponent<Head> ().finishTimer ();
	}
	public void onClickBipai(GameObject go){
		bipaiObj.SetActive (false);
		JsonData data = new JsonData ();
		data ["chair1"] = Users.instance.cid;
		data ["chair2"] = this.chairID;
		string str = data.ToJson ();
		KBEngine.Event.fireIn ("reqMessage", new object[]{ ZJHConst.ACTION_ROOM_BIPAI_START,str});
		Invoke ("sendBipai", 1.0f);
	}
	public void sendBipai(){
		JsonData data = new JsonData ();
		data ["chair1"] = Users.instance.cid;
		data ["chair2"] = this.chairID;
		string str = data.ToJson ();
		KBEngine.Event.fireIn ("reqMessage", new object[]{ZJHConst.ACTION_ROOM_BIPAI_END,str});
	}

	public void initPosition(int index){
		this.speakObj.transform.localPosition = speakVec [index];
		if (index < 2) {
			this.speakObj.transform.FindChild ("Sprite").gameObject.transform.Rotate(new Vector3(0,180,0));
		}
		this.cardsObj.transform.localPosition = cardsVec [index];
	}

	public void setProgressLimited(float time){
		headObj.GetComponent<Head> ().setMaxLimited (time);
	}
	public void finishProgress(){
		headObj.GetComponent<Head> ().finishTimer ();
	}
	public void resetProgress(){
		headObj.GetComponent<Head> ().resetTimer ();
	}
	public void registerTimeup(Progress.TimeupCallback methon){
		headObj.GetComponent<Head> ().registerCallback (methon);
	}
	
	public void playCards(bool bShow){
		cardsObj.SetActive (bShow);
		cardsObj.GetComponent<Cards> ().play (bShow);
	}
	public void lookCards(bool bLook){
		this.isLookCard = bLook;
		cardsObj.GetComponent<Cards> ().lookCards (bLook);
	}
	public void setCardsJson(string json,bool look){
		cardsObj.GetComponent<Cards> ().setCardsJson (json,look);
	}
	public void setSpeakState(string str,bool enable){
		this.speakObj.SetActive (enable);
		this.speakObj.GetComponent<UILabel> ().text = str;
	}
	public void setHead(int headID){
		if (headID < 0 || headID > 9) {
			headID = 1;
		}
		this.headObj.GetComponent<Chair> ().setHead (headID);
	}
	public void setBipaiActive(bool bActive){
		this.bipaiObj.SetActive (bActive);
	}
	public bool bipaiActiveSelf(){
		return this.bipaiObj.activeSelf;
	}
	public GameObject getHeadObject(){
		return headObj;
	}
	public void setGarkActive(bool bActive){
		headObj.GetComponent<Head> ().setGarkActive (bActive);
	}
	public void setMakerActive(bool bActive){
		headObj.GetComponent<Head> ().setMakerActive (bActive);
	}
	public void changeState(int state){
		if (state == ZJHConst.PLAYER_STATE_GARK) {
			this.setGarkActive (true);
		} else if (state == ZJHConst.PLAYER_STATE_KANPAI) {
			this.isLookCard = true;
			this.setSpeakState ("看牌", true);
			AudioController.Instance.SoundZJHPlay ("kanpai",sexID);
		} else if (state == ZJHConst.PLAYER_STATE_QIPAI) {
			this.setGarkActive (true);
			this.setSpeakState ("弃牌", true);
			AudioController.Instance.SoundZJHPlay ("fangqi",sexID);
			this.finishProgress ();
		} else if (state == ZJHConst.PLAYER_STATE_READY) {
			this.reset ();
			this.setGarkActive (false);
		}else if (state == ZJHConst.PLAYER_STATE_LOSER) {
			this.setGarkActive (true);
			this.setSpeakState ("比牌输", true);
			this.finishProgress ();
			if (Users.instance.cid == chairID) {
				AudioController.Instance.SoundZJHPlay ("lose",2);
			}
		}else if (state == ZJHConst.PLAYER_STATE_WIN) {
			this.setSpeakState ("比牌赢", true);
			this.finishProgress ();
			if (Users.instance.cid == chairID) {
				AudioController.Instance.SoundZJHPlay ("win",2);
			}
		}

		if (state > ZJHConst.PLAYER_STATE_LINE) {
			this.playCards (true);
			if (this.chairID == RoomUIComponent.makerID) {
				this.setMakerActive (true);
			}
		}
		this.state = state;
	}
	public int ChairID{
		get{
			return chairID;
		}
		set{
			chairID = value;
		}
	}
	public int State{
		get{
			return state;
		}
		set{
			state = value;
		}
	}
	public float Gold{
		set{
			gold = value;
			headObj.GetComponent<Head>().Gold = value.ToString();
		}
		get{
			return gold;
		}
	}
	public float Cost{
		set{
			headObj.GetComponent<Head> ().setChipCost (value);
		}
	}
	public bool IsLookCard {
		get {
			return isLookCard;
		}
		set {
			isLookCard = value;
		}
	}
	public int Sex {
		get {
			return sexID;
		}
		set {
			sexID = value;
		}
	}
}
