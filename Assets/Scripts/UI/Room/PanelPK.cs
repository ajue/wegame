using UnityEngine;
using System.Collections;

public class PanelPK : MonoBehaviour {
	GameObject left;
	GameObject right;
	TweenPosition leftTween;
	TweenPosition rightTween;
	// Use this for initialization
	void Awake () {

		left = transform.FindChild ("left").gameObject;
		right = transform.FindChild ("right").gameObject;

		leftTween = left.GetComponent<TweenPosition> ();
		leftTween.from = new Vector3(-1000,0,0);
		leftTween.to = new Vector3 (-275, 0, 0);
		leftTween.duration = 1f;
		leftTween.enabled = false;
		leftTween.AddOnFinished(onLeftFinished);

		rightTween = right.GetComponent<TweenPosition> ();
		rightTween.from = new Vector3(1000,0,0);
		rightTween.to = new Vector3 (300, 0, 0);
		rightTween.duration = 1f;
		rightTween.enabled = false;
		rightTween.AddOnFinished(onRightFinished);
	}

	public void onLeftFinished(){
		if (count > 2) {
			gameObject.SetActive (false);
		} else {
			count ++;
			this.leftTween.PlayReverse();
		}
	}
	public void onRightFinished(){
		if (count > 2) {
			gameObject.SetActive (false);
		} else {
			count ++;
			this.rightTween.PlayReverse();
		}
	}
	int count = 0;
	public void play(){
		this.PlayForward ();
	}
	public void PlayForward(){
		count = 1;
		this.leftTween.PlayForward ();
		this.rightTween.PlayForward ();
	}
}
