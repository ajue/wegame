using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	int id;
	UISprite sp;
	TweenPosition tp;

	void Awake(){
		sp = gameObject.GetComponent<UISprite> ();
		tp = gameObject.GetComponent<TweenPosition>();
		tp.enabled = false;
		tp.delay = 0.5f;
        tp.duration = 0.3f;
		tp.from = transform.localPosition;
		this.reset ();
	}
	public void reset(){
		tp.ResetToBeginning ();
		this.look (false);
	}
	
	public void look(bool bShow){
		if (bShow) {
			int index = 0;
			if(id>48){
				index = id-48;
			}
			else{
				index = (id + 4);
			}
			sp.spriteName = index.ToString ();
		} else {
			sp.spriteName = "CardBack";
		}
	}
	public void setActive(bool active){
		gameObject.SetActive (active);
	}
	public void setMoveTo(Vector3 to){
		tp.to = to;
	}
	public void PlayForward(){
		tp.enabled = true;
		tp.PlayForward ();
	}
	public void PlayReverse(){
		tp.enabled = true;
		tp.PlayReverse ();
	}
	public int ID{
		get{
			return id;
		}
		set{
			if (value < 1 || value > 54) {
				Debug.LogError("Card (id < 1 || id > 54) 越界 value = "+value);
			}
			id = value;
		}
	}
}
