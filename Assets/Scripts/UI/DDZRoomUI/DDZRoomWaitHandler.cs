using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDZRoomWaitHandler : GameUIHandler<DDZRoomWaitHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	DDZRoomWaitHandler uiComponent;

	public DDZRoomWaitHandler getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< DDZRoomWaitHandler >();
	}
	public override void onOpen()
	{
		this.setAnimation ();
	}

	public override void onClose()
	{

	}
}
