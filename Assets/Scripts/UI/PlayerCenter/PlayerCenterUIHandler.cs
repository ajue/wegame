using UnityEngine;
using System.Collections;

public class PlayerCenterUIHandler : GameUIHandler<PlayerCenterUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	PlayerCenterUIComponent uiComponent;

	public PlayerCenterUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< PlayerCenterUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();

	}

	public override void onClose()
	{
//		KBEngine.Event.fireIn("reqPlayerInfo", new object[] {});
	}
}
