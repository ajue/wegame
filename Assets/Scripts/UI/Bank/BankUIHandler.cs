using UnityEngine;
using System.Collections;

public class BankUIHandler : GameUIHandler<BankUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	BankUIComponent uiComponent;

	public BankUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< BankUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();
	}

	public override void onClose()
	{

	}
}
