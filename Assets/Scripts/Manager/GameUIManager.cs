using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIManager : SingletonMonoManager< GameUIManager >
{
	public Dictionary< string , GameHandler > uiDic = new Dictionary< string , GameHandler >();
	public Dictionary<string ,GameObject> uiObjDic = new Dictionary<string ,GameObject > ();
	
	public Transform anchor_Bottom;
	public Transform anchor_BottomLeft;
	public Transform anchor_BottomRight;
	public Transform anchor_Center;
	public Transform anchor_Left;
	public Transform anchor_Right;
	public Transform anchor_Top;
	public Transform anchor_TopLeft;
	public Transform anchor_TopRight;
	public Transform top_anchor_Center;
	
	public UIRoot root;
	public int width;
	public int height;
	
	public override void initSingletonMono()
	{		
		initUI();
		root = GameObject.FindObjectOfType<UIRoot>();
		float s = (float)root.activeHeight / Screen.height;
		
		height = Mathf.CeilToInt( Screen.height * s );
		width = Mathf.CeilToInt( Screen.width * s );
		
	}
	
	public void initUI()
	{
		Transform pf = gameObject.transform;
		anchor_Bottom = pf.FindChild("Bottom");
		anchor_BottomLeft = pf.FindChild("Bottom Left");
		anchor_BottomRight = pf.FindChild("Bottom Right");
		anchor_Center = pf.FindChild("Center");
		anchor_Left = pf.FindChild("Left");
		anchor_Right = pf.FindChild("Right");
		anchor_Top = pf.FindChild("Top");
		anchor_TopLeft = pf.FindChild("Top Left");
		anchor_TopRight = pf.FindChild("Top Right");

		instanceUI<RoomUIHandler> (anchor_Center, "RoomUI", false, false);
		instanceUI<HallCenterUIHandler> (anchor_Center, "HallCenterUI", false, false);
		instanceUI<LoginUIHandler> (anchor_Center, "LoginUI", false, false);
		instanceUI<PayCheckUIHandler> (anchor_Center, "PayCheckUI", false, false);
		instanceUI<HeadSelectHandler> (anchor_Center, "HeadSelectUI", false, false);
		instanceUI<RankUIHandler> (anchor_Center, "RankUI", false, false);
		instanceUI<DuiHuanUIHandler> (anchor_Center, "DuiHuanUI", false, false);
		instanceUI<WeSettingUIHandler> (anchor_Center, "WeSettingUI", false, false);
		instanceUI<BankUIHandler> (anchor_Center, "BankUI", false, false);
		instanceUI<NoticeUIHandler> (anchor_Center, "NoticeUI", false, false);
		instanceUI<MissionUIHandler> (anchor_Center, "MissionUI", false, false);
		instanceUI<PlayerCenterUIHandler> (anchor_Center, "PlayerCenterUI", false, false);
		instanceUI<WaittingUIHandler> (anchor_Center, "WaittingUI", false, false);
		instanceUI<DDZRoomUIHandler> (anchor_Center, "DDZRoomUI", false, false);
		instanceUI<GameOverTipUIHandler> (anchor_Center, "GameOverTipUI", false, false);
		instanceUI<BoxUIHandler> (anchor_Center, "BoxUI", false, false);
		instanceUI<DDZRoomWaitHandler> (anchor_Center, "DDZRoomWait", false, false);
		instanceUI<DownloadUIHandler> (anchor_Center, "DownloadUI", false, false);
		instanceUI<HallsecondUIHandler> (anchor_Center, "HallsecondUI", false, false);
		instanceUI<PayTypeHandler> (anchor_Center, "PayType", false, false);

		DownloadUIHandler.instance.Show();
	}
	
	public void instanceUI<T>( Transform anchor , string uiName , bool single , bool allways ) where T : GameUIHandler< T >{
		gameObject.AddComponent<T>();
		GameUIHandler< T >.instance.uiName = uiName;
		GameUIHandler< T >.instance.anchor = anchor;
		
		GameUIHandler< T >.instance.single = single;
		GameUIHandler< T >.instance.allways = allways;
	}
	
	
	public void releaseUnusedHandler()
	{
		foreach ( KeyValuePair< string, GameHandler > a in uiDic )
		{ 
			a.Value.ReleaseUnused();
		}
		Resources.UnloadUnusedAssets();
	}
	
	public void setHandler( string name , GameHandler handler )
	{
		uiDic[ name ] = handler;
	}
	public void AddUIObject(string name ,GameObject go)
	{
		GameObject obj = null;
		uiObjDic.TryGetValue (name, out obj);
		if(obj==null)
		{
			if(uiObjDic.ContainsKey(name))
			{
				uiObjDic.Remove(name);
				uiObjDic.Add(name,go);
			}
			else
			{
				uiObjDic.Add(name,go);
			}
		}
	}
	
	public GameObject createUI( string name )
	{
		string s = GameSetting.UIPath;
		s += name;
		
		GameObject cloneObject = (GameObject)Resources.Load (s);
		
		return (GameObject)Instantiate( cloneObject );
	}
	
	public GameObject getCloneUI( string name )
	{
		string s = GameSetting.UIPath;
		s += name;
		
		GameObject cloneObject = (GameObject)Resources.Load (s);
		
		return cloneObject;
	}
	
	public void checkSingel( string name )
	{
		foreach ( KeyValuePair< string, GameHandler > a in uiDic )
		{
			GameUIHandlerInterface uihander = (GameUIHandlerInterface)a.Value;
			
			if ( a.Key != name && !uihander.isAllways() )
			{
				uihander.UnShow();
			}
		}
	}
	
	public void hideAllUI()
	{
		foreach ( KeyValuePair< string, GameHandler > a in uiDic )
		{
			GameUIHandlerInterface uihander = (GameUIHandlerInterface)a.Value;
			
			if ( a.Key != name )
			{
				uihander.UnShow();
			}
		}
	}	
}
