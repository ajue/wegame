using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {
	int clock;
	float curClock;
	UILabel showLab;

	// Use this for initialization
	void Awake () {
		showLab = transform.GetComponent<UILabel> ();
	}
	public void reset(){
		this.curClock = this.clock;
		this.showLab.text = this.clock.ToString ();
	}
	// Update is called once per frame
	void Update () {
		
		if (this.curClock > 0) {
			this.curClock -= Time.deltaTime;
			this.showLab.text = ((int)this.curClock).ToString ();
		}
	}

	public void setClock(int clock,int chairID){
		this.clock = clock;
		this.curClock = clock;
		this.showLab.text = this.clock.ToString ();

		if (chairID == Users.instance.cid) {
			transform.localPosition = new Vector3 (0, 0, 0);
		}
		else if (chairID == Users.instance.cid % 3 + 1) 
		{
			transform.localPosition = new Vector3 (385, 20, 0);
		} 
		else
		{
			transform.localPosition = new Vector3 (-385, 20, 0);
		}
		gameObject.SetActive (true);
	}
}
