using UnityEngine;
using System.Collections;

public class RankUIHandler : GameUIHandler<RankUIHandler> {

	void Awake ()
	{
		mInstance = this;
	}

	RankUIComponent uiComponent;

	public RankUIComponent getUIComponent()
	{
		return uiComponent;
	}
	public override void onRelease()
	{
		uiComponent = null;
	}
	public override void onInit()
	{
		uiComponent = uiObject.GetComponent< RankUIComponent >();
	}
	public override void onOpen()
	{
		this.setAnimation ();

	}

	public override void onClose()
	{
		
	}
}
