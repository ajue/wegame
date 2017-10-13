using UnityEngine;
using System.Collections;

public class BoxUIComponent : GameUIComponent {
	public delegate void Callback();
	Callback cbFunc;

	int type;
	UILabel content;
	UILabel title;
	Transform sureBtn;
	Transform cancleBtn;
	bool bInit = false;

	void Awake () {
		this.init ();
	}
	void init(){
		bInit = true;
		content = transform.FindChild ("Content").GetComponent<UILabel> ();
		title = transform.FindChild ("Title").GetComponent<UILabel> ();
		sureBtn = transform.FindChild ("BtnSure");
		cancleBtn = transform.FindChild ("BtnCancel");

		UIEventListener.Get (sureBtn.gameObject).onClick = onClickSure;
		UIEventListener.Get (cancleBtn.gameObject).onClick = onClickCancel;
	}
	public void setType(int type){
		if (!bInit) {
			this.init ();
		}
		this.type = type;

		if (type == 1) {
			sureBtn.localPosition = new Vector3 (-140, -176, 0);
			sureBtn.gameObject.SetActive (true);
			cancleBtn.gameObject.SetActive (true);
		} else if (type == 2) {
			sureBtn.localPosition = new Vector3 (0, -176, 0);
			sureBtn.gameObject.SetActive (true);
			cancleBtn.gameObject.SetActive (false);
		}else if (type == 3) {
			cancleBtn.localPosition = new Vector3 (0, -176, 0);
			cancleBtn.gameObject.SetActive (true);
			sureBtn.gameObject.SetActive (false);
		}else if (type == 4) {
			sureBtn.localPosition = new Vector3 (0, -176, 0);
			sureBtn.gameObject.SetActive (true);
			cancleBtn.gameObject.SetActive (false);
		}
	}
	public void setTitle(string str){
		if (!bInit) {
			this.init ();
		}
		title.text = str;
	}
	public void setContent(string str){
		if (!bInit) {
			this.init ();
		}
		content.text = str;
	}
	public void setCallback(Callback cb){
		cbFunc += cb;
	}

	public void onClickSure(GameObject go){
		if (this.type == 1 || this.type == 4) {
			cbFunc();
		}
		BoxUIHandler.instance.UnShow ();
	}
	public void onClickCancel(GameObject go){
		BoxUIHandler.instance.UnShow ();
	}

}
