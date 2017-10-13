using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallsecondUIHandler : GameUIHandler<HallsecondUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	HallsecondUIComponent uiComponent;

	public HallsecondUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< HallsecondUIComponent >();
	}

	public override void onOpen()
	{
		this.setAnimation ();
	}

	public override void onClose()
	{

	}
}
