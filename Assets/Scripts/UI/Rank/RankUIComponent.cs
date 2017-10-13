using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using KBEngine;
public class RankUIComponent : GameUIComponent {

	Transform back;
	Transform scrollView;
	UILabel myRank_lbl;

	List<GameObject> rankList = new List<GameObject>();

	UIScrollView ScrollView;
	UIGrid Grid;


	void Awake()
	{
		KBEngine.Event.registerOut ("onRanksInfo", this, "onRanksInfo");
		KBEngine.Event.registerOut ("onMyRankInfo", this, "onMyRankInfo");

		back = transform.FindChild ("back_btn");
		scrollView = transform.FindChild ("rightPanel/scrollView");
		ScrollView = transform.FindChild ("rightPanel/scrollView").GetComponent<UIScrollView> ();
		Grid = transform.FindChild ("rightPanel/scrollView/grid").GetComponent<UIGrid> ();

		myRank_lbl = transform.FindChild ("myRank").GetComponent<UILabel> ();

		UIEventListener.Get (back.gameObject).onClick = this.onClickBack;

	}
	void OnEnable(){
		KBEngineApp.app.player ().baseCall ("reqRanksInfo");
		KBEngineApp.app.player ().baseCall ("reqMyRankInfo");
		WaittingUIHandler.instance.Show ();
	}
	public void onRanksInfo(string json)
	{
		Debug.Log (json);
		if (WaittingUIHandler.instance.isShow)
		{
			WaittingUIHandler.instance.UnShow ();
		}
		JsonData data = JsonMapper.ToObject (json);
		for (int i = 0; i < data.Count; i++) 
		{
			GameObject item = null;
			item = (GameObject)Instantiate (Resources.Load(GameSetting.UIItemPath + "RankItem"),transform.position,Quaternion.identity);	
			item.transform.parent = scrollView.FindChild("grid");
			item.transform.localScale = new Vector3 (1f, 1f, 1f);
			item.transform.localPosition = new Vector3 (0, i * -50, 0);
			item.name = "RankItem" + (i + 1).ToString ();
			item.AddComponent<RankItemComponent> ();
			rankList.Add (item);
		}

		for (int i = 0; i < data.Count; i++) {
			rankList [i].GetComponent<RankItemComponent> ().setData (data [i]);
		}

	}
	public void onMyRankInfo(string json)
	{
		if (WaittingUIHandler.instance.isShow)
		{
			WaittingUIHandler.instance.UnShow ();
		}
		JsonData data = JsonMapper.ToObject (json);
		string rank = data [0].ToString();
		myRank_lbl.text = "我的排名:第"+rank+"名";

	}

	public void onClickBack(GameObject obj)
	{
		rankList.Clear ();
		Grid.transform.DestroyChildren ();
		RankUIHandler.instance.UnShow ();
		HallCenterUIHandler.instance.Show ();
	}
	void OnDestroy()
	{
		Debug.Log ("KBEngine.Event.deregisterOut:" + gameObject.name);
		KBEngine.Event.deregisterOut (this);
	}
}
