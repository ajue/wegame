using UnityEngine;
using System.Collections;

public class LoadingUIComponent : GameUIComponent {

	UITexture Frontgound;
	void Awake()
	{
		Frontgound = transform.FindChild ("progressBar/Frontgound").GetComponent<UITexture> ();
		Frontgound.fillAmount = 0;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void showProcess(float progress)
	{
		Frontgound.fillAmount = progress;
	}
}
