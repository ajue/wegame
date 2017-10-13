using UnityEngine;
using System.Collections;


public class HallScene : GameScene {
	
	public override void initSingletonMono()
	{
		loadScene ();
	}
	
	public override void loadScene()
	{
	}
	
	public override void unloadScene()
	{
		Resources.UnloadUnusedAssets ();
	}
}
