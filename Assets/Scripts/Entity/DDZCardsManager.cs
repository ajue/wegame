using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using KBEngine;


public enum CardsType
{
	//先经常打的方法写前面，是对应类型的时候就return 后面的就不用检测了。
	ErrType = -1,
	One = 1,
	Duizi = 2,
	Shunzi = 3,
	Wangzha = 4,
	ThreeTakeOne = 5,
	Zhadan = 6,
	Three = 7,
	ThreeTakeTwo = 8,
	LianDui = 9,
	FeijiTakeWind_Dan = 10,
	Feiji = 11,
	FourTakeTwo = 12,
	FourTakeDui = 13,
	FeijiTakeWind_Dui = 14
};

public class DDZCardsManager : MonoBehaviour {
	UIGrid grid;
	bool bSmoothly;
	void Awake(){
		grid = transform.GetComponent<UIGrid> ();
		reset ();
	}
	public void reset(){
		bSmoothly = true;
		grid.animateSmoothly = true;
		transform.DestroyChildren ();
	}
	public void DeleAndUpdateCards(List<object> cards)
	{
		
		grid.animateSmoothly = (cards.Count == 17 && bSmoothly)?true:false;
		if (cards.Count != 0) {
			bSmoothly = false;
		}

		transform.DestroyChildren ();
		int cardCount = cards.Count;
		for (int i = 0; i<cards.Count; i++) {
			GameObject item = null;
			item = (GameObject)Instantiate (Resources.Load(GameSetting.UIPath + "Card1"),transform.position,Quaternion.identity);	
			item.transform.name = cards[i].ToString();
			item.transform.parent = transform;
			item.transform.localScale = new Vector3 (1f, 1f, 1f);
			//最后一张牌整张牌都可以点击
			if (i != cards.Count - 1) 
			{
				item.GetComponent<BoxCollider> ().center = new Vector3 (-52, 0, 0);
				item.GetComponent<BoxCollider> ().size = new Vector3 (36, 192, 0);
			}
			item.GetComponent<DDZCard>().ID = int.Parse(cards[i].ToString());
			item.GetComponent<DDZCard> ().BSelect = false;
			item.GetComponent<DDZCard> ().setCardData ();
		}
		grid.Reposition ();
	}
	public void reSetCards()
	{
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).GetComponent<DDZCard> ().BSelect = false;
		}
		grid.Reposition ();
	}
	public void resetTip()
	{
		for(int j =0;j<transform.childCount;j++)
		{
			if (transform.GetChild (j).GetComponent<DDZCard> ().BSelect) {
				transform.GetChild (j).GetComponent<DDZCard> ().BSelect = false;
				transform.GetChild (j).localPosition = new Vector3 (transform.GetChild (j).localPosition.x, transform.GetChild (j).localPosition.y - 20, 0);
			}
		}
	}
	public void ShowTipCards()
	{
		this.resetTip ();
		JsonData tipsData = DDZTips.instance.getTips ();

		if (tipsData == null && DDZTips.instance.IsFirstTips) {
			DDZRoomUIHandler.instance.getUIComponent ().unShowTips ();
			JsonData data = new JsonData ();
			JsonData cards = new JsonData ();
			cards.SetJsonType (JsonType.Array);
			data ["cards"] = cards;
			string json = data.ToJson ();
			KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, json);

		}
		if (tipsData == null) {
			return;
		}
		for (int i = 0; i < tipsData.Count; i++) {
			for(int j =0;j<transform.childCount;j++)
			{
				int id = transform.GetChild (j).GetComponent<DDZCard> ().ID;
				if ((int)tipsData [i] == id) {
					transform.GetChild (j).GetComponent<DDZCard> ().BSelect = true;
					transform.GetChild (j).localPosition = new Vector3 (transform.GetChild (j).localPosition.x, transform.GetChild (j).localPosition.y + 20, 0);
				}
			}
		}
	}
	public void setSelectCards()
	{
		for (int i = 0; i < transform.childCount; i++) 
		{
			DDZCard tempCard = transform.GetChild (i).GetComponent<DDZCard> ();
			if (tempCard.BSelect) 
			{
				DDZRules.instance.dapaiCardsList.Add (tempCard);
			}
		}
	}

	public JsonData getHandCardsJson()
	{
		List<int> list = new List<int> ();
		for (int i = 0; i < transform.childCount; i++) {
			int id = transform.GetChild (i).GetComponent<DDZCard> ().ID;
			list.Insert(i,id);
		}
		string json_data = JsonMapper.ToJson (list);
		JsonData cards = JsonMapper.ToObject (json_data);
		return cards;
	}

}
