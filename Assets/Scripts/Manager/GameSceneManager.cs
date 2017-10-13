using UnityEngine;
using System.Collections;


public enum SceneType
{
	LOGIN,
	Games,
	DDZHall,
	DdzRoom,
	ZjhHall,
	ZjhRoom
};

public class GameSceneManager : SingletonNew<GameSceneManager> {

	public void loadScene(SceneType type)
	{
		GameUIManager.instance.StartCoroutine (loadSceneAsync (type));
	}

	IEnumerator loadSceneAsync(SceneType type)
	{
		GameUIManager.instance.hideAllUI ();
		GameUIManager.instance.releaseUnusedHandler ();
		switch(type)
		{
		case SceneType.LOGIN:
			LoginUIHandler.instance.Show ();
			break;
		case SceneType.Games:
			HallCenterUIHandler.instance.Show ();
			break;
		case SceneType.DDZHall:
			HallsecondUIHandler.instance.Show ();
			HallsecondUIHandler.instance.getUIComponent ().reqEnterGame (1);
			break;
		case SceneType.ZjhHall:
			HallsecondUIHandler.instance.Show ();
			HallsecondUIHandler.instance.getUIComponent ().reqEnterGame (2);
			break;
		case SceneType.DdzRoom:
			DDZRoomUIHandler.instance.Show ();
			break;
		case SceneType.ZjhRoom:
			RoomUIHandler.instance.Show ();
			break;
		}
		yield return null;
	}
}
