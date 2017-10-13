using UnityEngine;
using System.Collections;

public class MoneyTipUIComponent : GameUIComponent {

	GameObject back;
	GameObject charge;
	// Use this for initialization
	void Awake()
	{
		back = transform.FindChild ("sure").gameObject;
		UIEventListener.Get (back).onClick = onClick ;

		charge = transform.FindChild ("giveup").gameObject;
		UIEventListener.Get (charge).onClick = onClick;
	}
	void Start () {
	
	}
	public void onClick(GameObject obj)
	{
		if (obj == back) {
			MoneyTipUIHandler.instance.UnShow ();
		} else if (obj == charge) {
			HallCenterUIHandler.instance.UnShow ();
			MoneyTipUIHandler.instance.UnShow ();
			PayCheckUIHandler.instance.Show ();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
