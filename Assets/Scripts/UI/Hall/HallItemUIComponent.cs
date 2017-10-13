using UnityEngine;
using System.Collections;

public class HallItemUIComponent : GameUIComponent {
	UISprite bg;
	UILabel difen;
	UILabel xianzhi;
	UILabel online;

	GameHallData data;
	void Awake()
	{
		bg = transform.FindChild ("Animation/bg").GetComponent<UISprite> ();
		difen = transform.FindChild ("difen").GetComponent<UILabel> ();
		xianzhi = transform.FindChild ("xianzhi").GetComponent<UILabel> ();
		online = transform.FindChild ("Online").GetComponent<UILabel> ();
	}
	public void setData(GameHallData d)
	{
		bg.spriteName = d.bg_name;
		difen.text = d.difen.ToString();
		xianzhi.text = d.xianzhi.ToString();
		online.text = d.playerCount.ToString();
		this.data = d;
	}
	void Update()
	{
	
	}

	public GameHallData HallData{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
		}
	}


}
