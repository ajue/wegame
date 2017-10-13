using UnityEngine;
using System.Collections;

public class Progress : MonoBehaviour {
	public delegate void TimeupCallback();
	public TimeupCallback callback = null;
	UISprite sp;
	float maxLimited;
	float curLimited;
	
	// Use this for initialization
	void Awake () {
		sp = gameObject.GetComponent<UISprite> ();
		sp.fillAmount = 0;
	}

	void Update(){
//		Debug.Log ("Limited:"+Time.deltaTime);
		if (curLimited <= 0||sp.fillAmount <= 0)
			return;
		if (curLimited > 0) {
			curLimited -= Time.deltaTime;
		} else {
			curLimited = 0;
		}
		sp.fillAmount = curLimited / maxLimited;
		if (sp.fillAmount <= 0&&callback!=null) {
			callback();
		}
	}

	public void reset(){
		this.curLimited = maxLimited;
		sp.fillAmount = 1f;
	}
	public float MaxLimited{
		get{
			return maxLimited;
		}
		set{
			maxLimited = value;
		}
	}
	public float CurLimited{
		get{
			return curLimited;
		}
		set{
			curLimited = value;
		}
	}
	public float FillAmount{
		set{
			sp.fillAmount = value;
		}
	}
	public void OnDestroy(){

	}
	
}
