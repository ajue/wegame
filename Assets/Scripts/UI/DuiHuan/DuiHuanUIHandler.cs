using UnityEngine;
using System.Collections;

public class DuiHuanUIHandler : GameUIHandler<DuiHuanUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	DuiHuanUIComponent uiComponent;

	public DuiHuanUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< DuiHuanUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();
	}

	public override void onClose()
	{

	}
}
