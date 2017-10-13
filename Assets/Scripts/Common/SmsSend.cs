using UnityEngine;
using System.Collections;
using System;
using cn.SMSSDK.Unity; 
using KBEngine;

public class SmsSend : SingletonMono<SmsSend>,SMSSDKHandler {
	
	public static int Type_Login	= 1;
	public static int TYPE_register = 2;
	public int Type = Type_Login;

	private SMSSDK smssdk;
	private string phone = "";
	private string zone = "86";
	private string code = "";
	private string result = null;

	public override void initSingletonMono(){
		smssdk = gameObject.GetComponent<SMSSDK>();
		smssdk.init("1d22a63050bd0","0eea3eeb065734fd14b51f9000f07486",false);
		smssdk.setHandler(this);
		Debug.Log ("SmsSend init");
	}
		
	public void getCode(string phone){
		this.phone = phone;
		smssdk.getCode (CodeType.TextCode, phone, "86");
	}
	public void commitCode(string code){
		smssdk.commitCode (phone, "86",code);
	}
	public void showRegisterPage(){
		smssdk.showRegisterPage (CodeType.TextCode);
	}
	public void onComplete(int action, object resp)
	{
		if (resp != null)
		{
			result = resp.ToString();
			GameObject.Find ("AnchorLogin/tips").GetComponent<UILabel> ().text = result;
			KBEngine.Event.fireOut ("onCodeResult", new object[]{action,result});
		}
	}
	public void onError(int action, object resp)
	{
		Debug.Log("Error :" + resp);
		result = resp.ToString();
		KBEngine.Event.fireOut ("onCodeResult", new object[]{action,result});
	}
}
