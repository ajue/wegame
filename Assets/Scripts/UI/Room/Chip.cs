using UnityEngine;
using System.Collections;

public class Chip : MonoBehaviour {
	UILabel label;
	UISprite bg;
	float chip;
	int level;
	TweenPosition tween;
	// Use this for initialization
	void Awake () {
		label = transform.FindChild ("Label").GetComponent<UILabel> ();
		bg = transform.FindChild ("bg").GetComponent<UISprite> ();
		tween = gameObject.GetComponent<TweenPosition> ();
		tween.duration = 0.5f;
		tween.delay = 0.2f;
		tween.enabled = false;
	}
	bool bFinished = false;
	public void fromSend(Vector3 from){
		tween.from = from;
		Vector3 to = new Vector3 (Random.Range(-50f,50f),Random.Range(-20f,50f),0);
		tween.to = to;
		tween.ResetToBeginning ();
		tween.enabled = true;
		bFinished = false;
	}
	public void reciveTo(Vector3 to){
		tween.from = transform.localPosition;
		tween.to = to;
		tween.ResetToBeginning ();
		tween.enabled = true;
		tween.AddOnFinished (onFinished);

		bFinished = true;
	}
	public void onFinished(){
		if (bFinished) {
			gameObject.SetActive (false);
		}
	}

	public void setChip(float chip,int lev){
		this.chip = chip;
		this.level = lev;
		bg.spriteName = "zhu" + lev;
		if (this.chip > 10000) {
			float txt = this.chip / 10000f;
			label.text = txt.ToString ()+"万";
		} else {
			label.text = this.chip.ToString();
		}
	}
}
