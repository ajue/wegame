using System;
using UnityEngine;
using System.Collections;

public abstract class GameUIHandler< T > : Singleton< T > , GameUIHandlerInterface
{
	public string uiName;
	public Transform anchor;
	public int renderQ = 0;
	public bool allways = false;
	public bool single = false;
	
	public abstract void onRelease();
	public abstract void onInit();
	public abstract void onOpen();
	public abstract void onClose();
	
	
	[ HideInInspector ] public bool isShow = false;
	[ HideInInspector ] public GameObject uiObject = null;
	[ HideInInspector ] public GameObject cloneObject = null;
	[ HideInInspector ] public bool isLoaded = false;
	
	public void move( float x , float y )
	{
		uiObject.transform.localPosition = new Vector3( x , y , uiObject.transform.position.z );
	}
	
	public bool isSingle()
	{
		return single;
	}
	public  bool isAllways()
	{
		return allways;
	}
	
	public A getUIComponent< A >() where A : Component 
	{
		return uiObject.GetComponent< A >();
	}

	
	public void Show()
	{

		if ( !isLoaded )
		{
			if (GameObject.Find (uiName) == null) 
			{
				string s = GameSetting.UIPath;
				s += uiName;
				
				#if UNITY_EDITOR
				Debug.Log(s);
				#endif
				
				cloneObject = (GameObject)Resources.Load( s );
				
				if ( !cloneObject )
				{
					#if UNITY_EDITOR
					Debug.LogError( "res not found " + s );
					#endif
					
					return;
				}
				uiObject = NGUITools.AddChild( anchor ? anchor.gameObject : null , cloneObject );
				uiObject.transform.localScale = cloneObject.transform.localScale;
			}
			else{
				uiObject = anchor.FindChild(uiName).gameObject;
			}
			//when uiObject is ture, add *.cs
			if(uiObject)
			{
				uiObject.AddComponent(System.Type.GetType(uiName+"Component"));
			}
			UIPanel panel = uiObject.GetComponent< UIPanel >();
			if ( panel ) 
			{
				panel.renderQueue = UIPanel.RenderQueue.Automatic;
				panel.startingRenderQueue = renderQ;
			}
			
			uiObject.name = uiName;
			
			isLoaded = true;
			
			GameUIManager.instance.setHandler( uiName , this );
			GameUIManager.instance.AddUIObject(uiName,uiObject);
			
			onInit();
		}
		else
		{
			uiObject.SetActive( true );
		}
		
		
		isShow = true;
		
		onOpen();
		
		if ( single )
		{
			GameUIManager.instance.checkSingel( uiName );
		}

	}
		
	public void UnShow()
	{
		if ( !uiObject )
		{
			return;
		}
		
		isShow = false;
		uiObject.SetActive (false);
		
		onClose ();
	}
	
	public void ReleaseUnused()
	{
		if ( !isShow )
		{
			Release();
		}
	}
	
	public void Release()
	{
		if ( !isLoaded ) 
		{
			return;
		}
		
		if ( isShow )
		{
			onClose ();
			isShow = false;
			uiObject.SetActive (false);
		}
		
		onRelease ();
		
		NGUITools.Destroy( uiObject );
		
		uiObject = null;
		cloneObject = null;
		//DestroyImmediate( cloneObject );
		//Resources.UnloadUnusedAssets();
		
		isLoaded = false;
	}
	
	public void SetUISize(Transform trans = null)
	{
		if(trans == null)
		{
			trans = uiObject.transform;
		}
		
		float standard_aspect = 1920f / 1080f;
		float device_aspect  =  (float)Screen.width / Screen.height;
		
		if (device_aspect < standard_aspect)
		{
			if(trans.gameObject.name == "ScoreUI")
			{
				trans.localScale = new Vector3(0.9f,0.9f,0.9f);
				return;
			}
			if(trans.gameObject.name == "NewPetHatchUI")
			{
				trans.localScale = new Vector3(1.2f,1.2f,1.2f);
				return;
			}
			float size = device_aspect / standard_aspect;
			trans.localScale = new Vector3(size , size , 1);
		}

	}

	public void setAnimation(){
		UIPanel panel = uiObject.GetComponent<UIPanel> ();
		if(panel != null)
			StartCoroutine (runAlphe (panel,0.5f));
	}

	IEnumerator runAlphe(UIPanel panel,float time){
		float curTime = 0;
		while (curTime < 1)
		{
			panel.alpha = Mathf.Lerp (0, 1, curTime);
			curTime += Time.fixedDeltaTime/time;
			yield return null;
		}
	}
}


