using UnityEngine;
using System.Collections;

public class HallCenterUIHandler : GameUIHandler<HallCenterUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}
	
	HallCenterUIComponent uiComponent;
	
	public HallCenterUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< HallCenterUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();
	}
	
	public override void onClose()
	{
		
	}
}
