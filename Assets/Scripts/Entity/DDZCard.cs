using UnityEngine;
using System.Collections;

public class DDZCard : MonoBehaviour {

	int id;
	UISprite sp;
	bool bSelect;
	TweenPosition tweenPos ;

	void Awake()
	{
		sp = gameObject.GetComponent<UISprite> ();
		UIEventListener.Get (gameObject).onClick += onClickCard;
//		UIEventListener.Get (gameObject).onPress += onPress;
		UIEventListener.Get(gameObject).onDragOver = Drag;
	}
	void Drag(GameObject obj)
	{
		if (!bSelect) {
			bSelect = true;
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 20, 0);
			AudioController.Instance.SoundPlay ("ddz/choose");
		} else {
			bSelect = false;
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + -20, 0);
		}
	}
	public void TweenMove()
	{
		transform.gameObject.AddComponent<TweenPosition> ();
		tweenPos = transform.GetComponent<TweenPosition> ();
		EventDelegate.Add (tweenPos.onFinished, removeTween);
		tweenPos.to = transform.localPosition;
	}
	public void removeTween()
	{
		Destroy (transform.GetComponent<TweenPosition> ());
	}
	public void onPress(GameObject obj,bool isDown)
	{
		if (isDown) {
			if (!bSelect) {
				bSelect = true;
				transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 20, 0);
				AudioController.Instance.SoundPlay ("ddz/choose");
			} else {
				bSelect = false;
				transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y -20, 0);
			}
		} else {
			return;
		}
	}
	public void onClickCard(GameObject obj)
	{
		if (bSelect) {
			bSelect = false;
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - 20, 0);
		} else 
		{
			bSelect = true;
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 20, 0);
			AudioController.Instance.SoundPlay ("ddz/choose");
		}
	}
	public void setCardData()
	{
		int index = 0;
		//处理卡片A 2
		if (id > 44 && id < 53) 
		{
			index = id - 44;
		} 
		else if (id < 45) 
		{
			index = (id + 8);
		} 
		//大王 小王
		else if (id > 52) 
		{
			index = id;
		}
		sp.spriteName = index.ToString ();
	}

	public int ID{
		get{
			return id;
		}
		set{
			if (value < 1 || value > 54) {
				Debug.LogError("Card (id < 1 || id > 54) 越界 value = "+value);
			}
			id = value;
		}
	}

	public bool BSelect{
		get{
			return bSelect;
		}
		set
		{ 
			bSelect = value;
		}
	}
}
