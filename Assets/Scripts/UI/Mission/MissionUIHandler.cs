using UnityEngine;
using System.Collections;

public class MissionUIHandler : GameUIHandler<MissionUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	MissionUIComponent uiComponent;

	public MissionUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< MissionUIComponent >();
	}
	public override void onOpen()
	{

	}

	public override void onClose()
	{

	}
}
