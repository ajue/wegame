using UnityEngine;
using System.Collections;

public class Jubaosdk : SingletonNew<Jubaosdk>{

	AndroidJavaClass unityPlayer;
	AndroidJavaObject currentActivity;
	AndroidJavaClass jubaoClass;
	AndroidJavaObject jubaoObj;
	// Use this for initialization
	public void init () {
		unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//		jubaoClass = new AndroidJavaClass ("com.hengyuenet.jubaosdk.Pay");
//		jubaoObj = jubaoClass.CallStatic<AndroidJavaObject> ("instance");
//		jubaoObj.Call ("setContext", currentActivity);
	}

	public void setAmount(string amount){
		if (currentActivity == null) {
			init ();
		}
		Debug.Log("hyc android call setAmount.");
		currentActivity.Call("setAmount",Users.instance.Account, amount);
//		jubaoObj.Call("setAmount",Users.instance.Account, amount);
	}
}
