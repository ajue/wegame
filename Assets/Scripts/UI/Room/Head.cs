using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Head : MonoBehaviour {
	UILabel gold;
	UILabel nickname;
	UILabel addrLab;
	UILabel cost;
	UISprite headSprite;
	Progress progress;
	GameObject garkObj;
	GameObject bipaiObj;
	GameObject makerObj;
	void Awake () {
		gold = transform.FindChild ("jinqian_num").GetComponent<UILabel> ();
		nickname = transform.FindChild ("nikename").GetComponent<UILabel> ();
		addrLab = transform.FindChild ("addr").GetComponent<UILabel> ();
		cost = transform.FindChild ("chipCost").GetComponent<UILabel> ();
		progress = transform.FindChild ("progress").GetComponent<Progress> ();
		headSprite = transform.FindChild ("head").GetComponent<UISprite> ();

		garkObj = transform.FindChild ("gark").gameObject;
		bipaiObj = transform.FindChild ("BipaiButton").gameObject;
		makerObj = transform.FindChild ("maker").gameObject;

		garkObj.SetActive (false);
		bipaiObj.SetActive (false);
		makerObj.SetActive (false);
		progress.MaxLimited = 15f;

	}
	public void setHead(int head){
		if (head < 1 || head > 8) {
			head = 1;
		}
		this.headSprite.spriteName = head.ToString ();
	}
	public void setChipCost(float cost){
		this.cost.text = cost.ToString ();
	}
	public void registerCallback(Progress.TimeupCallback methon){
		progress.callback = methon;
	}
	public void setMaxLimited(float limit){
		progress.MaxLimited = limit;
	}
	public void setCurLimited(float curLimit){
		progress.CurLimited = curLimit;
	}
	public void finishTimer(){
		progress.FillAmount = 0;
	}
	public void resetTimer(){
		progress.reset ();
	}
	public void setGarkActive(bool bGark){
		this.garkObj.SetActive (bGark);
	}
	public void setMakerActive(bool bActive){
		this.makerObj.SetActive (bActive);
	}

	public GameObject BipaiObject(){
		return bipaiObj;
	}
	public string NickName{
		get{
			return nickname.text;
		}
		set{
			nickname.text = value;
		}
	}
	public string Gold{
		get{
			return this.gold.text;
		}
		set{
			this.gold.text = value;
		}
	}
	public string Addr{
		get{
			return this.addrLab.text;
		}
		set{
			this.addrLab.text = value;
		}
	}

}
