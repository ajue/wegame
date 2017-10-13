using UnityEngine;
using System.Collections;

public class NoticeUIHandler : GameUIHandler<NoticeUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	NoticeUIComponent uiComponent;

	public NoticeUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< NoticeUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();

	}

	public override void onClose()
	{

	}
}
