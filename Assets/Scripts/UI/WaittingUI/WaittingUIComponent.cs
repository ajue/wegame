using UnityEngine;
using System.Collections;

public class WaittingUIComponent : GameUIComponent {
	public enum Type{
		Loading,
		NetworkError,
		AccountError
	}
	Type type;
	UISprite sp;
	GameObject btn;
	void Awake()
	{
		type = Type.Loading;
		btn = transform.FindChild ("Button").gameObject;
		btn.SetActive (false);
		UIEventListener.Get (btn).onClick = onClick;
	}

	void Update(){
		if (type == Type.Loading &&  Input.GetKeyDown(KeyCode.Return)){
			WaittingUIHandler.instance.UnShow ();
		}
	}
	public void OnDisable(){
		type = Type.Loading;
		setText ("");
	}

	public void setText(string str){
		if (type == Type.AccountError) {
			return;
		}
		transform.FindChild ("Label").GetComponent<UILabel> ().text = str;
	}
	public void setType(Type type){
		this.type = type;
		if (type == Type.Loading) {
			transform.FindChild ("Button").gameObject.SetActive (false);
		} else if (type == Type.NetworkError) {
			transform.FindChild ("Button").gameObject.SetActive (true);
		} else if (type == Type.AccountError) {
			transform.FindChild ("Button").gameObject.SetActive (true);
		}
	}

	public void onClick(GameObject go){
		GameObject obj = GameObject.Find ("NetworkClient");
		DestroyImmediate (obj);

		GameObject client = (GameObject)Instantiate (Resources.Load(GameSetting.UIPath + "NetworkClient"),new Vector3(0,0,0),Quaternion.identity);		
		client.name = "NetworkClient";
		Invoke ("gotoLoginScene", 0.5f);
	}
	void gotoLoginScene(){
		GameSceneManager.instance.loadScene(SceneType.LOGIN);
	}
}
