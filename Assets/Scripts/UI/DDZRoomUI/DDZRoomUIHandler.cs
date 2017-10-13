using UnityEngine;
using System.Collections;

public class DDZRoomUIHandler : GameUIHandler<DDZRoomUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	DDZRoomUIComponent uiComponent;

	public DDZRoomUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< DDZRoomUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();
	}

	public override void onClose()
	{

	}
}
