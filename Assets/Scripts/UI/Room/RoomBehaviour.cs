using UnityEngine;
using System.Collections.Generic;
using LitJson;

public class RoomBehaviour : MonoBehaviour {
	Dictionary<int,GameObject> btnDict = new Dictionary<int, GameObject> ();
	Dictionary<int,GameObject> chairDict = null;

	List<GameObject> multBtn = new List<GameObject>();
	List<float> jiazhuConfig = new List<float>();

	GameObject myChair;
	GameObject beishu;

	// Use this for initialization
	void Awake () {
		btnDict.Add (1, transform.FindChild ("genzhu").gameObject);
		btnDict.Add (2, transform.FindChild ("jiazhu").gameObject);
		btnDict.Add (3, transform.FindChild ("kanpai").gameObject);
		btnDict.Add (4, transform.FindChild ("bipai").gameObject);
		btnDict.Add (5, transform.FindChild ("qipai").gameObject);

		beishu = transform.FindChild ("beishu").gameObject;
		beishu.SetActive (false);

		for (int i=1; i<=btnDict.Count; i++) {
			GameObject go;
			if(btnDict.TryGetValue(i,out go)){
				UIEventListener.Get (go).onClick = onClick;
			}
		}

		for (int i=0; i<beishu.transform.childCount; i++) {
			GameObject go = beishu.transform.GetChild(i).gameObject;
			multBtn.Add(go);
			UIEventListener.Get(go).onClick = onClickBeishu;
		}
	}
	
	public void onClick(GameObject go){
		if (!go.name.Equals ("jiazhu")) {
			beishu.SetActive (false);
		}

		if (go.name.Equals ("genzhu")) {
			JsonData data = new JsonData ();
			data ["chairID"] = Users.instance.cid;
			string json = data.ToJson ();
			KBEngine.Event.fireIn ("reqMessage", new object[]{ZJHConst.ACTION_ROOM_GENZHU,json});
			this.nomalReset(false);
			myChair.GetComponent<Chair>().finishProgress();
			AudioController.Instance.SoundPlay("m_follow1");

		} else if (go.name.Equals ("jiazhu")) {
			beishu.SetActive(!beishu.activeSelf);
			if(beishu.activeSelf){
				float curzhu = GameObject.Find("RoomTopUI").GetComponent<RoomTopUI>().Curzhu;
				Chair userChair = chairDict [Users.instance.cid].GetComponent<Chair>();
				int mult = 1;
				if (userChair.IsLookCard) {
					mult = 2;
				}
				for (int i = 0; i < jiazhuConfig.Count; i++) {
					if (curzhu < jiazhuConfig [i] && userChair.Gold >= jiazhuConfig [i] * mult) {
						this.setMulitState (i, true);
					} else {
						this.setMulitState (i, false);
					}
				}
			}

		} else if (go.name.Equals ("kanpai")) {
			JsonData data = new JsonData ();
			data ["chairID"] = Users.instance.cid;
			string json = data.ToJson ();
			KBEngine.Event.fireIn ("reqMessage", new object[]{ZJHConst.ACTION_ROOM_KANPAI,json});
			this.setBehaviourState (3,false);
			AudioController.Instance.SoundPlay("m_see");

		} else if (go.name.Equals ("bipai")) {
			Chair userChair = chairDict [Users.instance.cid].GetComponent<Chair>();
			foreach(int key in chairDict.Keys){
				Chair chair = chairDict[key].GetComponent<Chair>();
				if(chair.State > ZJHConst.PLAYER_STATE_LINE && Users.instance.cid!=chair.ChairID ){
					if ((!chair.IsLookCard && userChair.IsLookCard)) {
						if (userChair.Gold >= RoomTopUI.instance.Curzhu * 4) {
							chair.setBipaiActive (!chair.bipaiActiveSelf ());
						}
					} else if (userChair.Gold >= RoomTopUI.instance.Curzhu * 2) {
						chair.setBipaiActive (!chair.bipaiActiveSelf ());
					}
				}
			}
		} else if (go.name.Equals ("qipai")) {
			this.reqQipai();
		}
	}

	public void reqQipai(){

		JsonData data = new JsonData ();
		data ["chairID"] = Users.instance.cid;
		string json = data.ToJson ();
		KBEngine.Event.fireIn ("reqMessage", new object[]{ZJHConst.ACTION_ROOM_QIPAI,json});

		AudioController.Instance.SoundPlay("m_giveup");
	}

	public void onClickBeishu(GameObject go){
		int idx = int.Parse(go.name);

		JsonData data = new JsonData ();
		data ["chairID"] = Users.instance.cid;
		data ["jiazhu"] = jiazhuConfig[idx];
		string json = data.ToJson ();
		KBEngine.Event.fireIn ("reqMessage", new object[]{ZJHConst.ACTION_ROOM_JIAZHU,json});

		myChair.GetComponent<Chair>().finishProgress();
		this.nomalReset (false);
		beishu.SetActive (false);
	}
	public void Timeup(){
		//this.reqQipai ();
		//gameObject.SetActive (false);
	}
	public GameObject MyChair{
		get{
			return myChair;
		}
		set{
			myChair = value;
			myChair.GetComponent<Chair>().registerTimeup(this.Timeup);
		}
	}
	public Dictionary<int,GameObject> ChairDict{
		set{
			chairDict = value;
		}
	}
	
	public void setBehaviourState(int idx,bool state){
		GameObject go = btnDict [idx];
		go.GetComponent<BoxCollider> ().enabled = state;
	
		if(!state)
			go.GetComponent<UIButton> ().SetState (UIButtonColor.State.Disabled, true);
		else{
			go.GetComponent<UIButton> ().SetState (UIButtonColor.State.Normal, true);
		}

	}
	public void reset(){
		for (int i=1; i<=btnDict.Count; i++) {
			if (myChair.GetComponent<Chair> ().IsLookCard && i == 3) {
				this.setBehaviourState (i, false);
			} else {
				this.setBehaviourState(i,true);
			}
		}
	}
	public void nomalReset(bool bState){
		this.setBehaviourState (1,bState);
		this.setBehaviourState (2,bState);
		this.setBehaviourState (4,bState);
	}

	public void setMulitState(int index,bool enabled){
		
		GameObject go = multBtn[index];
		if(!enabled){
			go.GetComponent<BoxCollider> ().enabled = false;
			go.GetComponent<UIButton> ().SetState (UIButtonColor.State.Disabled, true);
		}
		else{
			go.GetComponent<BoxCollider> ().enabled = true;
			go.GetComponent<UIButton> ().SetState (UIButtonColor.State.Normal, true);
		}
	}

	public void setConfig(string json){
		Debug.Log ("setConfig json = " + json);
		JsonData data = JsonMapper.ToObject (json);
		for (int i=0; i<data.Count; i++) {
			jiazhuConfig.Add (float.Parse(data [i].ToString()));
			beishu.transform.FindChild (i + "/Label").GetComponent<UILabel> ().text = data [i].ToString ();
		}
	}

}
