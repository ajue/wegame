using UnityEngine;
using System.Collections;

public class LoginUIHandler : GameUIHandler<LoginUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}
	
	LoginUIComponent uiComponent;
	
	public LoginUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< LoginUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();
	}
		
	public override void onClose()
	{
		
	}
}
