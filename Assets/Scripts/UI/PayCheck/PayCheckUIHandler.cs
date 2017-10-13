using UnityEngine;
using System.Collections;

public class PayCheckUIHandler : GameUIHandler<PayCheckUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	PayCheckUIComponent uiComponent;

	public PayCheckUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< PayCheckUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();

	}

	public override void onClose()
	{

	}
}
