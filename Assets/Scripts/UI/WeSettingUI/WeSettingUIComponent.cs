using UnityEngine;
using System.Collections;
using KBEngine;
public class WeSettingUIComponent : GameUIComponent {

	Transform back;
	Transform musicClose;
	Transform musicOpen;
	Transform SoundClose;
	Transform SoundOpen;

	public int musicValue = 1;//1代表open 0 代表关闭
	public int soundValue = 1;

	GameObject reAccount;
	void Awake()
	{
		if (PlayerPrefs.GetInt ("Music") != 0) {
			musicValue = PlayerPrefs.GetInt ("Music");
		}
		if (PlayerPrefs.GetInt ("Sound1") != 0) {
			soundValue = PlayerPrefs.GetInt ("Sound1");
		}
		
		musicClose = transform.FindChild ("Music/Mclose");
		musicOpen = transform.FindChild ("Music/Mopen");
		SoundClose = transform.FindChild ("Sound/Sclose");
		SoundOpen = transform.FindChild ("Sound/Sopen");
		back = transform.FindChild ("back_btn");

		reAccount = transform.FindChild ("reAccount").gameObject;

		UIEventListener.Get (back.gameObject).onClick = onClickBack;
		UIEventListener.Get (reAccount).onClick = replaceAccount;
		UIEventListener.Get (musicClose.gameObject).onClick = onClickBtn;
		UIEventListener.Get (musicOpen.gameObject).onClick = onClickBtn;
		UIEventListener.Get (SoundClose.gameObject).onClick = onClickBtn;
		UIEventListener.Get (SoundOpen.gameObject).onClick = onClickBtn;

		if (musicValue == 1) {
			musicOpen.gameObject.SetActive (false);
			musicClose.gameObject.SetActive (true);
		} else if(musicValue == -1){
			musicOpen.gameObject.SetActive (true);
			musicClose.gameObject.SetActive (false);
		}

		if (soundValue == 1) {
			SoundOpen.gameObject.SetActive (false);
			SoundClose.gameObject.SetActive (true);
		} else if(soundValue == -1){
			SoundOpen.gameObject.SetActive (true);
			SoundClose.gameObject.SetActive (false);
		}
	}

	public void replaceAccount(GameObject go){
		WaittingUIHandler.instance.Show ();
		GameObject obj = GameObject.Find ("NetworkClient");
		DestroyImmediate (obj);

		GameObject client = (GameObject)Instantiate (Resources.Load(GameSetting.UIPath + "NetworkClient"),new Vector3(0,0,0),Quaternion.identity);		
		client.name = "NetworkClient";
		Invoke ("gotoLoginScene", 0.5f);
	}
	void gotoLoginScene(){
		GameSceneManager.instance.loadScene(SceneType.LOGIN);
	}

	public void onClickBack(GameObject obj)
	{
		WeSettingUIHandler.instance.UnShow ();
	}
	public void onClickBtn(GameObject obj)
	{
		if (obj.name.Equals ("Mclose")) {
			musicValue = 0;
			PlayerPrefs.SetInt ("Music", -1);
			musicClose.gameObject.SetActive (false);
			musicOpen.gameObject.SetActive (true);
		} else if (obj.name.Equals ("Mopen")) {
			musicValue = 1;
			PlayerPrefs.SetInt ("Music", 1);
			musicClose.gameObject.SetActive (true);
			musicOpen.gameObject.SetActive (false);
		}else if (obj.name.Equals ("Sclose")) {
			soundValue = 0;
			PlayerPrefs.SetInt ("Sound1", -1);
			SoundClose.gameObject.SetActive (false);
			SoundOpen.gameObject.SetActive (true);
		}else if (obj.name.Equals ("Sopen")) {
			soundValue = 1;
			PlayerPrefs.SetInt ("Sound1", 1);
			SoundClose.gameObject.SetActive (true);
			SoundOpen.gameObject.SetActive (false);
		}
		AudioController.Instance.BGMSetVolume (musicValue);
		AudioController.Instance.ClipSetVolume (soundValue);
		NGUITools.soundVolume = 0;
	}
}
