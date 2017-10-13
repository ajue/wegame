using UnityEngine;
using System.Collections;


public class RoomUIHandler : GameUIHandler<RoomUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}
	
	RoomUIComponent uiComponent;
	
	public RoomUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< RoomUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();

	}
	
	public override void onClose()
	{
		
	}
}
