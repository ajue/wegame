using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using KBEngine;

public class NetworkClient : KBEMain {
	//公网ip
	public string outIP = "101.201.67.131";
	//本地ip
	public string inIP = "172.16.4.154";
	//
	public bool usedOutnet = false;
	bool startRelogin = false;
	public override void initKBEngine(){

		ip = usedOutnet ? outIP : inIP;
		base.initKBEngine ();
	}
	public override void installEvents(){
		KBEngine.Event.registerOut("onDisconnected",this,"onDisconnected");
		KBEngine.Event.registerOut ("onReloginBaseappSuccessfully", this, "onReloginBaseappSuccessfully");
		KBEngine.Event.registerOut ("onReloginBaseappFailed", this, "onReloginBaseappFailed");
	}
	public void onDisconnected(){
		WaittingUIHandler.instance.Show();
		WaittingUIHandler.instance.getUIComponent ().setText ("你已掉线，尝试重连中...");
		startRelogin = true;
		Invoke ("onReloginBaseappTimer", 1);
	}
	void onReloginBaseappTimer(){
		WaittingUIHandler.instance.getUIComponent ().setText ("正在链接...");
		KBEngineApp.app.reloginBaseapp();
		if (startRelogin) {
			WaittingUIHandler.instance.Show ();
			Invoke ("onReloginBaseappTimer", 3.0f);
		}
	}
	public void onReloginBaseappSuccessfully(){
		WaittingUIHandler.instance.getUIComponent ().setText ("登陆成功...");
		WaittingUIHandler.instance.UnShow ();
		startRelogin = false;
		CancelInvoke ();
	}
	public void onReloginBaseappFailed(UInt16 failedcode){
		WaittingUIHandler.instance.getUIComponent ().setType (WaittingUIComponent.Type.NetworkError);
		WaittingUIHandler.instance.getUIComponent().setText(KBEngineApp.app.serverErr (failedcode));
		startRelogin = false;
		CancelInvoke ();
	}

	public override void KBEUpdate()
	{
		if (!isMultiThreads) {
			gameapp.process ();
		}

		KBEngine.Event.processOutEvents();
		KBEngine.Event.processNocacheOutEvent ();
	}
}
