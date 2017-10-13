using UnityEngine;
using System.Collections;

public class MissionUIComponent : GameUIComponent {

	Transform back;

	void Awake()
	{
		back = transform.FindChild ("back_btn");
		UIEventListener.Get (back.gameObject).onClick = this.onClickBack;
	}

	// Update is called once per frame
	void Update () {

	}
	public void onClickBack(GameObject obj)
	{
		MissionUIHandler.instance.UnShow ();
		HallCenterUIHandler.instance.Show ();
	}
}
