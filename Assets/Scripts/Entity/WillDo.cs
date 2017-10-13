using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using KBEngine;

public class WillDo : MonoBehaviour {
	public static byte Type_Jiaofen 	= 1;
	public static byte Type_Jiabei		= 2;
	public static byte Type_Power		= 3;		//你大
	public static byte Type_NoPower		= 4;		//上家大

	byte type;
	string[] jfNames 	= new string[]{"1分","2分","3分","不叫"};
	string[] jbNames 	= new string[]{"加倍","不加倍","",""};
	string[] powNames 	= new string[]{"提示","出牌","",""};
	string[] noPowNames 	= new string[]{"不出","提示","出牌",""};

	GameObject[] btns = new GameObject[4];
	DDZCardsManager cardsManager;
	void Awake () {
		type = Type_Jiaofen;
		for (int i = 0; i < btns.Length; i++) {
			btns [i] = transform.FindChild ((i + 1).ToString()).gameObject;
			UIEventListener.Get (btns [i]).onClick = onClick;
		}
		gameObject.SetActive (false);
	}

	public void reset(){
		this.setType (Type_Jiaofen);
		gameObject.SetActive (false);
	}

	public void setType(byte type){
		gameObject.SetActive (true);
		if(type >= Type_Power && cardsManager == null){
			cardsManager = FindObjectOfType<DDZCardsManager> ();
		}
		this.type = type;
		if (type == Type_Jiaofen) {
			for (int i = 0; i < btns.Length; i++) {
				btns [i].SetActive (true);
				btns [i].GetComponent<UILabel> ().text = jfNames [i];
			}
		} else if (type == Type_Jiabei) {
			for (int i = 0; i < btns.Length; i++) {
				if (i < 2) {
					btns [i].GetComponent<UILabel> ().text = jbNames [i];
					btns [i].SetActive (true);
				} else {
					btns [i].SetActive (false);
				}
			}
		}else if (type == Type_Power) {
			for (int i = 0; i < btns.Length; i++) {
				if (i < 2) {
					btns [i].GetComponent<UILabel> ().text = powNames [i];
					btns [i].SetActive (true);
				} else {
					btns [i].SetActive (false);
				}
			}
		}else if (type == Type_NoPower) {
			for (int i = 0; i < btns.Length; i++) {
				if (i < 3) {
					btns [i].GetComponent<UILabel> ().text = noPowNames [i];
					btns [i].SetActive (true);
				} else {
					btns [i].SetActive (false);
				}
			}
		}
		transform.GetComponent<UIGrid> ().Reposition ();
		transform.GetComponent<UIGrid> ().repositionNow = true;
	}
	public void setEnableCenter(int id){
		for (int i = 0; i < btns.Length; i++) {
			if (i <= (id-1)) {
				btns [i].SetActive (false);
			}
		}
		transform.GetComponent<UIGrid> ().Reposition ();
		transform.GetComponent<UIGrid> ().repositionNow = true;
	}
	public void onClick(GameObject obj){
		DDZRoomUIComponent roomUI = DDZRoomUIHandler.instance.getUIComponent ();
		JsonData data = new JsonData ();
		data ["curCid"] = Users.instance.cid;
		if (type == Type_Jiaofen) {
			gameObject.SetActive (false);
			int score = int.Parse (obj.name);
			if (score == 4) {
				score = 0;
			}
			data ["curScore"] = score;
			string json = data.ToJson ();

			KBEngineApp.app.player ().cellCall ("reqMessageC",DDZConst.ACTION_ROOM_JIAOPAI, json);
		} else if (type == Type_Jiabei) {
			gameObject.SetActive (false);
			int multiple = 0;
			if (obj.name == "1") {
				multiple = 12;
			} else {
				multiple = 11;
			}
			data ["curScore"] = multiple;
			string json = data.ToJson ();
			KBEngineApp.app.player ().cellCall ("reqMessageC",DDZConst.ACTION_ROOM_JIAOPAI,json);
		} else if (type == Type_Power) {
			if (obj.name == "1") {

				cardsManager.ShowTipCards ();
				AudioController.Instance.SoundPlay ("ddz/choose");

			} else {

				DDZRules.instance.clearList ();
				cardsManager.setSelectCards ();
				DDZRules.instance.sortByHTOL ();
				DDZRules.instance.sendCardsMessage ();
				AudioController.Instance.SoundPlay ("ddz/givecard");
			}
		} else if (type == Type_NoPower) {
			if (obj.name == "1") {
				gameObject.SetActive (false);
				DDZRoomUIHandler.instance.getUIComponent ().unShowTips ();
				JsonData cards = new JsonData ();
				cards.SetJsonType (JsonType.Array);
				data ["cards"] = cards;
				string json = data.ToJson ();
				KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, json);
			
			}else if(obj.name == "2"){
				
				cardsManager.ShowTipCards ();
				AudioController.Instance.SoundPlay ("ddz/choose");

			}else if(obj.name == "3"){
				DDZRules.instance.clearList ();
				cardsManager.setSelectCards ();
				DDZRules.instance.sortByHTOL ();
				DDZRules.instance.sendCardsMessage ();
				AudioController.Instance.SoundPlay ("ddz/givecard");
			}
		}
	}
}
