using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayTypeHandler :  GameUIHandler<PayTypeHandler> {
	void Awake ()
	{
		mInstance = this;
	}

	PayTypeComponent uiComponent;

	public PayTypeComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< PayTypeComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();

	}

	public override void onClose()
	{

	}
}
