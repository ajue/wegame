using UnityEngine;
using System.Collections;
using System;
using KBEngine;
using System.Collections.Generic;
using LitJson;
using cn.SMSSDK.Unity; 

public class LoginUIComponent : GameUIComponent {

	private GameObject archLogin;
	private GameObject archRegister;
	private UIToggle rembox;

	void Start()
	{
		//注册网络事件
		KBEngine.Event.registerOut ("onConnectionState", this, "onConnectionState");
		KBEngine.Event.registerOut ("onLoginFailed", this, "onLoginFailed");
		KBEngine.Event.registerOut ("onLoginSuccessfully", this, "onLoginSuccessfully");
		KBEngine.Event.registerOut ("onCreateAccountResult", this, "onCreateAccountResult");
		KBEngine.Event.registerOut ("onLoginBaseappFailed", this, "onLoginBaseappFailed");
		KBEngine.Event.registerOut ("onCodeResult", this, "onCodeResult");
		KBEngine.Event.registerOut ("onReviseProperties", this, "onReviseProperties");

		archLogin = transform.FindChild ("AnchorLogin").gameObject;
		archRegister = transform.FindChild ("AnchorRegister").gameObject;
		archRegister.SetActive (false);

		rembox = transform.FindChild ("AnchorLogin/CheckBox").GetComponent<UIToggle>();
		rembox.value = PlayerPrefs.GetInt ("Remember") == 1 ? true : false;

		UIEventListener.Get (rembox.gameObject).onClick = onClickBox;

		this.showAccountPanel (PlayerPrefs.GetString ("Account"));
	}

	void OnEnable(){
		KBEngineApp.app.bConnected = false;
	}
	void OnDisable(){
		if (!rembox.value) {
			PlayerPrefs.SetString ("Account", "");
		}
	}
	void Update(){
		if (Input.GetKeyDown(KeyCode.Return)){
			GameOverTipUIHandler.instance.Show ();
		}
	}
	public void onClickBox(GameObject obj){
		PlayerPrefs.SetInt ("Remember", rembox.value?1:0);
	}
	public void reqAccountLogin(string account,string password){
		WaittingUIHandler.instance.Show ();
		Users.instance.Account = account;
		PlayerPrefs.SetString ("Account", account);
		KBEngine.Event.fireIn ("login", account, password, System.Text.Encoding.UTF8.GetBytes (""));
	}
	public void showAccountPanel(string phone){
		this.archLogin.SetActive (true);
		UIInput accountInput = transform.FindChild ("AnchorLogin/account").GetComponent<UIInput> ();
		accountInput.value = phone;

		GameObject loginBtn = transform.FindChild ("AnchorLogin/LoginBtn").gameObject;
		GameObject registerBtn = transform.FindChild("AnchorLogin/RegisterBtn").gameObject;

		UIEventListener.Get (loginBtn).onClick = delegate {
			GameObject account = transform.FindChild ("AnchorLogin/account").gameObject;
			string a = account.GetComponent<UIInput>().value;

			#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			this.reqAccountLogin(a,Helper.getPassword(a));

			#else
			if(PlayerPrefs.HasKey("Account") && PlayerPrefs.GetString("Account").Equals(a)){
				this.reqAccountLogin(a,Helper.getPassword(a));
			}
			else{
				PlayerPrefs.SetString("Account","");
				Invoke ("willSmsSendLogin", 0f);
			}
			#endif
		};
		UIEventListener.Get (registerBtn).onClick = delegate {
			#if UNITY_ANDROID
			SmsSend.instance.Type = SmsSend.TYPE_register;
			SmsSend.instance.showRegisterPage();
			#endif
			#if UNITY_IPHONE
			SmsSend.instance.Type = SmsSend.TYPE_register;
			SmsSend.instance.showRegisterPage();
			#endif
			#if UNITY_EDITOR_OSX
			string testPhone = "1122334455662";
			this.regiseterAccount(testPhone,Helper.getPassword(testPhone));
			#endif
		};
	}
	public void showRegisterPanel()
	{
		this.archRegister.SetActive (true);
		this.archLogin.SetActive (false);
		GameObject btnBack = transform.FindChild ("AnchorRegister/back_btn").gameObject;
		UIEventListener.Get (btnBack).onClick = delegate {
			archLogin.SetActive (true);
			archRegister.SetActive (false);
		};
		GameObject btn = transform.FindChild ("AnchorRegister/BtnRegister").gameObject;
		UIEventListener.Get (btn).onClick = delegate {
			string name = transform.FindChild("AnchorRegister/name").GetComponent<UIInput>().value;
			if(name == ""){
				return;
			}
			bool man = transform.FindChild("AnchorRegister/sex/man").GetComponent<UIToggle>().value;
			int sex = 1;
			if(!man){
				sex = 2;
			}
			WaittingUIHandler.instance.Show();
			//请求进行玩家属性设置
			KBEngineApp.app.player().baseCall("reqReviseProperties",name,sex,1);
		};
	}
	void willSmsSendLogin(){
		SmsSend.instance.Type = SmsSend.Type_Login;
		SmsSend.instance.showRegisterPage();
	}
	//验证返回
	public void onCodeResult(int action,string result){
		ActionType act = (ActionType)action;
		if (act == ActionType.CommitCode) {
			JsonData jsonData = JsonMapper.ToObject (result);
			string phone = jsonData ["phone"].ToString ();
			PlayerPrefs.SetString ("Account", phone);
			if (SmsSend.instance.Type == SmsSend.TYPE_register) {
				//号码验证完成
				WaittingUIHandler.instance.Show();
				this.regiseterAccount(phone,Helper.getPassword(phone));
			} else {
				this.showAccountPanel (phone);
			}
		}
	}
	public void onConnectionState(bool status){
		if (!status) {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (2);
			box.setContent ("连接失败,请检查网络");
		}
	}

	public void onLoginFailed(UInt16 s){
		WaittingUIHandler.instance.UnShow ();

		BoxUIHandler.instance.Show ();
		BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
		box.setType (2);
		box.setContent (KBEngineApp.app.serverErr (s));
	}
	//登陆网关失败
	public void onLoginBaseappFailed(UInt16 failedcode){
		WaittingUIHandler.instance.UnShow ();

		BoxUIHandler.instance.Show ();
		BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
		box.setType (2);
		box.setContent (KBEngineApp.app.serverErr (failedcode));
	}
	//账号注册
	public void regiseterAccount(string stringAccount,string stringPassword){
		WaittingUIHandler.instance.Show ();
		KBEngine.Event.fireIn ("createAccount", 
			stringAccount,
			stringPassword,
			System.Text.Encoding.UTF8.GetBytes (stringAccount+"|"+stringPassword));
	}
	//注册结果
	public void onCreateAccountResult(UInt16 retcode,byte[] datas){
		WaittingUIHandler.instance.UnShow ();
		if (retcode != 0) {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setTitle ("系统提示");
			box.setType (2);
			box.setContent (KBEngineApp.app.serverErr (retcode));
		} else {
			string buf = System.Text.Encoding.UTF8.GetString (datas);
			string[] infos = buf.Split('|');
			reqAccountLogin (infos[0], infos[1]);
		}
	}
	public void onLoginSuccessfully(Player account){
		WaittingUIHandler.instance.UnShow ();
		string name = account.getDefinedProperty ("name").ToString ();
		int state = (int)account.getDefinedProperty ("state");

		if(state == 0){
			KBEngine.Event.resume ();
			if (name == null || name.Equals ("")) {
				this.showRegisterPanel ();
			} else {
				GameSceneManager.instance.loadScene (SceneType.Games);
			}
		}
		else if(state == 1){
			GameSceneManager.instance.loadScene (SceneType.DdzRoom);
		}
	}
	public void onReviseProperties(int retcode,string name,int sexId,int headId)
	{
		WaittingUIHandler.instance.UnShow ();
		if (retcode == 0) {
			GameSceneManager.instance.loadScene (SceneType.Games);
		} else {
			BoxUIHandler.instance.Show ();
			BoxUIComponent box = BoxUIHandler.instance.getUIComponent ();
			box.setType (2);
			box.setContent ("昵称只能由中文，英文，数字组合！");
		}
	}
	public void OnDestroy(){
		KBEngine.Event.deregisterOut (this);
	}
}
