using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class GameScene : SingletonMono< GameScene >
{
	public virtual void loadScene()
	{
		
	}
	
	public virtual void unloadScene()
	{
		
	}
	
	public virtual void updateScene()
	{
		
	}
	
	public virtual void clearCache()
	{
		
	}
}
