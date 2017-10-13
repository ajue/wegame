using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadUIHandler : GameUIHandler<DownloadUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	DownloadUIComponent uiComponent;

	public DownloadUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< DownloadUIComponent >();
	}
	public override void onOpen()
	{
	}

	public override void onClose()
	{

	}
}
