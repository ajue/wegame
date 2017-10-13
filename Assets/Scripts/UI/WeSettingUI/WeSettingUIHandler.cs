using UnityEngine;
using System.Collections;

public class WeSettingUIHandler : GameUIHandler<WeSettingUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	WeSettingUIComponent uiComponent;

	public WeSettingUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< WeSettingUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();

	}

	public override void onClose()
	{

	}
}
