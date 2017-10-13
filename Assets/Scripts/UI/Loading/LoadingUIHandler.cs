using UnityEngine;
using System.Collections;


public class LoadingUIHandler : GameUIHandler<LoadingUIHandler> {
	
	void Awake ()
	{
		mInstance = this;
	}
	
	LoadingUIComponent uiComponent;
	
	public LoadingUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< LoadingUIComponent >();
	}
	public override void onOpen()
	{
		
	}
	
	public override void onClose()
	{
		
	}
}
