using UnityEngine;
using System.Collections;

public class GameOverTipUIComponent : GameUIComponent {

	Transform sureBtn;
	Transform giveupBtn;

	void Awake()
	{
		sureBtn = transform.FindChild ("sure");
		giveupBtn = transform.FindChild ("giveup");
		UIEventListener.Get (sureBtn.gameObject).onClick = this.onSureBtn;
		UIEventListener.Get (giveupBtn.gameObject).onClick = this.onGiveUpBtn;
	}

	public void onSureBtn(GameObject obj)
	{
		if(Users.instance.GameID != GAME_ID.GAME_NONE){
			KBEngine.Event.fireIn("reqLeaveGame",new object[]{});
		}
		Application.Quit ();
	}

	public void onGiveUpBtn(GameObject obj)
	{
		GameOverTipUIHandler.instance.UnShow ();
	}
}
