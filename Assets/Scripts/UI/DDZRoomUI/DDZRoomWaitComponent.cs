using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDZRoomWaitComponent : MonoBehaviour {
	Transform clock;
	UILabel clockLab;
	float second;
	int minute;
	bool isStop = false;
	// Use this for initialization
	void Start () {
		clock = transform.FindChild("clock");
		clockLab = clock.FindChild ("time").GetComponent<UILabel> ();
	}
	public void OnEnable(){
		this.second = 0;
		this.minute = 0;
	}
	public void OnDisable(){
		
	}
	// Update is called once per frame
	void Update () {
		this.updateWaittingClock ();
	}
	public void updateWaittingClock()
	{
		if (isStop) {
			return;
		}
		if (this.second < 60) {
			this.second += Time.deltaTime;
			if (this.second < 10) {
				if (this.minute < 10) {
					this.clockLab.text = "0" + minute.ToString () + ":" + "0" + ((int)this.second).ToString ();
				} else {
					this.clockLab.text =  minute.ToString () + ":" + "0" + ((int)this.second).ToString ();
				}
			} else {
				if (this.minute < 10) {
					this.clockLab.text = "0" + minute.ToString () + ":" + ((int)this.second).ToString ();
				} else {
					this.clockLab.text = minute.ToString () + ":" + ((int)this.second).ToString ();
				}
			}
		} else {
			this.second = 0;
			minute++;
			if (minute >= 60) {
				isStop = true;
			}
		}
	}
}
