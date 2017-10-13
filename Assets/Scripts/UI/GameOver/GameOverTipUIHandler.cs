using UnityEngine;
using System.Collections;

public class GameOverTipUIHandler : GameUIHandler<GameOverTipUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	GameOverTipUIComponent uiComponent;

	public GameOverTipUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< GameOverTipUIComponent >();
	}
	public override void onOpen()
	{

	}

	public override void onClose()
	{

	}
}
