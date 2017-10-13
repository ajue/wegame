using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public static bool startShake = false;
	public static float seconds = 0.0f;
	public static bool started = false;
	public static float quake = 0.2f;
	private Vector3 camPOS;
	public bool is2D;
	
	void Start() {
		camPOS = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(startShake){
			transform.position = Random.insideUnitSphere * quake;
			if(is2D) transform.position = new Vector3(transform.position.x,transform.position.y,camPOS.z);
		}
		
		if(started){     //相机震动效果
		  	StartCoroutine(WaitForSecond(seconds));	
			started = false;
		}
	}
	
	public static void shakeFor(float a,float b){
		seconds = a;
		started = true;
		quake = b;
	}
	
 	IEnumerator WaitForSecond(float a) {
		camPOS = transform.position;
		startShake = true;
 		yield return new WaitForSeconds(a);
		startShake = false;
		transform.position = camPOS;
	}
}
