using UnityEngine;
using System.Collections;

public class WaittingUIHandler : GameUIHandler<WaittingUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	WaittingUIComponent uiComponent;

	public WaittingUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< WaittingUIComponent >();
	}
	public override void onOpen()
	{

	}

	public override void onClose()
	{

	}
}
