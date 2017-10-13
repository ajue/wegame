using UnityEngine;
using System.Collections;

public class MoneyTipUIHandler : GameUIHandler<MoneyTipUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	MoneyTipUIComponent uiComponent;

	public MoneyTipUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< MoneyTipUIComponent >();
	}
	public override void onOpen()
	{
	}

	public override void onClose()
	{

	}
}
