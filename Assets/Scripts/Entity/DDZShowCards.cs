using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using KBEngine;

public class DDZShowCards : MonoBehaviour {
	
	public void showCards(Entity entity,List<object> cardsList)
	{
		transform.DestroyChildren ();

		int cid = (int)entity.getDefinedProperty ("cid");
		int sex	= (int)entity.getDefinedProperty ("sexC");

		for (int i = 0; i < cardsList.Count; i++) {
			GameObject item;
			item = (GameObject)Instantiate (Resources.Load(GameSetting.UIPath + "Card1"),transform.position,Quaternion.identity);	
			item.transform.parent = transform;
			item.transform.localScale = new Vector3 (1f, 1f, 1f);
			item.GetComponent<DDZCard> ().ID = (int)cardsList [i];
			item.GetComponent<DDZCard> ().setCardData ();
			Destroy (item.GetComponent<BoxCollider> ());
		}
		transform.GetComponent<UIGrid> ().Reposition();

		string cards_string = JsonMapper.ToJson (cardsList);
		JsonData cards = JsonMapper.ToObject(cards_string);

		CardsType type = DDZTips.instance.checkType (cards);

		JsonData lastCardsData = DDZTips.instance.getLastCardsJson(); 
		CardsType lasttype = CardsType.ErrType;
		int dani = Random.Range(1,3);
		if (lastCardsData != null) {
			lasttype = DDZTips.instance.checkType (DDZTips.instance.getLastCardsJson ());
		}
		if ( type == CardsType.One) {
			int nameID = this.getCardsName (cards);
			AudioController.Instance.SoundDDZPlay (nameID.ToString(),sex);

		}else if (type == CardsType.Duizi) {
			int nameID = this.getCardsName (cards);
			AudioController.Instance.SoundDDZPlay ("dui"+nameID.ToString(),sex);

		}else if (type == CardsType.Three) {
			int nameID = this.getCardsName (cards);
			AudioController.Instance.SoundDDZPlay ("tuple"+nameID.ToString(),sex);
		}else if (type == CardsType.ThreeTakeOne) {
			if (lasttype != CardsType.ThreeTakeOne) {
				AudioController.Instance.SoundDDZPlay ("sandaiyi",sex);
			} else {
				AudioController.Instance.SoundDDZPlay ("dani"+dani.ToString(),sex);
			}
		}else if (type == CardsType.ThreeTakeTwo) {
			if (lasttype != CardsType.ThreeTakeTwo) {
				AudioController.Instance.SoundDDZPlay ("sandaiyidui",sex);
			} else {
				AudioController.Instance.SoundDDZPlay ("dani"+dani.ToString(),sex);
			}
		}else if (type == CardsType.Shunzi) {
			if (lasttype != CardsType.Shunzi) {
				AudioController.Instance.SoundDDZPlay ("shunzi",sex);
			} else {
				AudioController.Instance.SoundDDZPlay ("dani"+dani.ToString(),sex);
			}
		}else if (type == CardsType.LianDui) {
			if (lasttype != CardsType.LianDui) {
				AudioController.Instance.SoundDDZPlay ("liandui",sex);
			} else {
				AudioController.Instance.SoundDDZPlay ("dani"+dani.ToString(),sex);
			}
		}else if (type == CardsType.Feiji) {
			if (lasttype != CardsType.Feiji) {
				AudioController.Instance.SoundDDZPlay ("feiji",sex);
			} else {
				AudioController.Instance.SoundDDZPlay ("dani"+dani.ToString(),sex);
			}
		}else if (type == CardsType.FeijiTakeWind_Dan) {
			if (lasttype != CardsType.FeijiTakeWind_Dan) {
				AudioController.Instance.SoundDDZPlay ("feiji",sex);
			} else {
				AudioController.Instance.SoundDDZPlay ("dani"+dani.ToString(),sex);
			}
		}else if (type == CardsType.FeijiTakeWind_Dui) {
			if (lasttype != CardsType.FeijiTakeWind_Dui) {
				AudioController.Instance.SoundDDZPlay ("feiji",sex);
			} else {
				AudioController.Instance.SoundDDZPlay ("dani"+dani.ToString(),sex);
			}
		}else if (type == CardsType.Zhadan) {
				AudioController.Instance.SoundDDZPlay ("zhadan",sex);
		}else if (type == CardsType.FourTakeTwo) {
			AudioController.Instance.SoundDDZPlay ("sidaier",sex);
		}else if (type == CardsType.FourTakeDui) {
			AudioController.Instance.SoundDDZPlay ("sidaier",sex);
		}else if (type == CardsType.FeijiTakeWind_Dui) {
			AudioController.Instance.SoundDDZPlay ("sidailiangdui",sex);
		}else if (type == CardsType.Wangzha) {
			AudioController.Instance.SoundDDZPlay ("wangzha",sex);
		}

	}

	public int getCardsName(JsonData jsonData)
	{
		if (jsonData.Count <1) {
			return -1;
		} else {
			int index = 0;
			int id = (int)jsonData[0];
			//处理卡片A 2
			if (id > 44 && id < 53) 
			{
				index = id - 44;
			} 
			else if (id < 45) 
			{
				index = (id + 8);
			} 				
			else if (id == 53) //小王
			{
				return 14;
			}
			else if (id == 54) //大王
			{
				return 15;
			}	
			return (((int)index-1) / 4 ) + 1;
		}
	}


}
