using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSelectHandler : GameUIHandler<HeadSelectHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	HeadSelectUIComponent uiComponent;

	public HeadSelectUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< HeadSelectUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();

	}

	public override void onClose()
	{
	}
}
