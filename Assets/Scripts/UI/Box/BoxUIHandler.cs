using UnityEngine;
using System.Collections;

public class BoxUIHandler : GameUIHandler<BoxUIHandler>  {

	void Awake ()
	{
		mInstance = this;
	}

	BoxUIComponent uiComponent;

	public BoxUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent<BoxUIComponent>();
	}
	public override void onOpen()
	{
		this.setAnimation ();
	}

	public override void onClose()
	{

	}
}
