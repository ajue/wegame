using UnityEngine;
using System.Collections;

public class MoveNotice : MonoBehaviour {
	Transform textTran;
	TweenPosition tween;
	int start = 360;
	int end = -360;
	int curEnd = 0;
	// Use this for initialization
	void Awake () {
		textTran = transform.FindChild ("text");
		tween = textTran.GetComponent<TweenPosition> ();
		tween.from = new Vector3 (start, 0, 0);
	}

	public void setText(string text){
		textTran.GetComponent<UILabel> ().text = text;
		int width = textTran.GetComponent<UILabel> ().width;
		curEnd = end - width;
		tween.to = new Vector3 (curEnd, 0, 0);
		tween.Play ();
	}

}
