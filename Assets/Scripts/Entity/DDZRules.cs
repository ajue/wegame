using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using KBEngine;

public class DDZRules : SingletonNew<DDZRules> {

	public List<DDZCard> dapaiCardsList = new List<DDZCard> ();
	public int powerID = 0;
	public JsonData powerCards;
	public void clearList()
	{
		dapaiCardsList.Clear ();
	}

	public void sortByHTOL()//从高ID---低ID
	{
		DDZCard temp;
		for (int i = dapaiCardsList.Count; i>0; i--) 
		{
			for (int j = 0; j < i - 1; j++) 
			{
				if (dapaiCardsList [j].ID < dapaiCardsList [j + 1].ID)
				{
					temp = dapaiCardsList [j ];
					dapaiCardsList [j] = dapaiCardsList [j + 1];
					dapaiCardsList [j + 1] = temp;
				}
			}
		}
	}

	public void sendCardsMessage()
	{
		JsonWriter writer = new JsonWriter ();
		writer.WriteObjectStart ();
		writer.WritePropertyName ("curCid");
		writer.Write (Users.instance.cid);
		writer.WritePropertyName ("state");
		writer.Write (1);
		writer.WritePropertyName ("cards");
		writer.WriteArrayStart ();
		for (int i = 0; i < dapaiCardsList.Count; i++)
		{
			writer.Write(dapaiCardsList [i].ID);
		}
		writer.WriteArrayEnd ();
		writer.WriteObjectEnd ();
		JsonData data = JsonMapper.ToObject (writer.ToString ());
		JsonData myCardsData = data ["cards"];
		CardsType myCardsType = this.getCardsType (myCardsData);

		if (this.powerID == Users.instance.cid) {
			if (this.getCardsType (myCardsData) == CardsType.ErrType) {
				DDZRoomUIHandler.instance.getUIComponent().showTips(1);
				Debug.Log("你出的牌不符合，请重新选择");
			} else {
				DDZRoomUIHandler.instance.getUIComponent().unShowTips();
				KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
			}

		} else {
			CardsType lastCardsType = this.getCardsType (this.powerCards);
			if (lastCardsType == CardsType.Wangzha) {
				return;
				//王炸不能出任何牌
			}
			if (myCardsType == CardsType.Wangzha) {
				//王炸大于一切
				KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
				return;
			}
			if (lastCardsType != myCardsType){
				
				if (myCardsType == CardsType.Zhadan) {
					DDZRoomUIHandler.instance.getUIComponent().unShowTips();
					KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
					return;
				}
				DDZRoomUIHandler.instance.getUIComponent().showTips(1);
				return;

			} else {
				if (lastCardsType == CardsType.One) {
					if ((int)myCardsData [0] > 52) {
						if ((int)myCardsData [0] > (int)this.powerCards [0]) {
							DDZRoomUIHandler.instance.getUIComponent().unShowTips();
							KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
							return;
						}
					}
					if (((int)myCardsData [0] - 1) / 4 > ((int)this.powerCards [0] - 1) / 4) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
						//不符合出牌
					}
				} else if (lastCardsType == CardsType.Duizi) {
					if ((int)myCardsData [0] > (int)this.powerCards [0]) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
						//不符合出牌
					}
				} else if (lastCardsType == CardsType.Shunzi && this.powerCards.Count == myCardsData.Count) {
					if ((int)myCardsData [0] > (int)this.powerCards[0]) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
						//不符合出牌
					}
				} else if (lastCardsType == CardsType.LianDui && this.powerCards.Count == myCardsData.Count) {
					if ((int)myCardsData [0] > (int)this.powerCards[0]) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
						//不符合出牌
					}

				} else if (lastCardsType == CardsType.Three || //三条
				           lastCardsType == CardsType.Zhadan) { //炸弹
					if ((int)myCardsData [0] > (int)this.powerCards[0]) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
						//不符合出牌
					}
				} else if (lastCardsType == CardsType.ThreeTakeTwo) {
					int lastThreeId = this.getThreeMaxID (this.powerCards);
					int myThreeId = this.getThreeMaxID (myCardsData);
					if (myThreeId > lastThreeId) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
					}
					
				} else if (lastCardsType == CardsType.ThreeTakeOne) {
					int lastThreeId = this.getThreeMaxID (this.powerCards);
					int myThreeId = this.getThreeMaxID (myCardsData);
					if (myThreeId > lastThreeId) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
					}
					
				} else if (lastCardsType == CardsType.Feiji && this.powerCards.Count == myCardsData.Count) {
					if ((int)myCardsData [0] > (int)this.powerCards [0]) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
						//不符合出牌
					}
				} else if (lastCardsType == CardsType.FeijiTakeWind_Dan) {
					if (this.getFeijiTakeWingMaxSection (myCardsData) > this.getFeijiTakeWingMaxSection (this.powerCards)) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
					}

				} else if (lastCardsType == CardsType.FourTakeTwo || lastCardsType == CardsType.FourTakeDui) {
					Dictionary<int,int> lastDic = new Dictionary<int,int> ();
					Dictionary<int,int> mytDic = new Dictionary<int,int> ();
					lastDic = this.getTypeDic (this.powerCards);
					mytDic = this.getTypeDic (myCardsData);
					if (this.getFourTakeMaxID (lastDic) < this.getFourTakeMaxID (mytDic)) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
					}
				} else if (lastCardsType == CardsType.FeijiTakeWind_Dui) {
					if (this.getFeijiTakeWingMaxSection (myCardsData) > this.getFeijiTakeWingMaxSection (this.powerCards)) {
						DDZRoomUIHandler.instance.getUIComponent().unShowTips();
						KBEngineApp.app.player ().cellCall ("reqMessageC", DDZConst.ACTION_ROOM_CHUPAI, writer.ToString ());
						return;
					} else {
						DDZRoomUIHandler.instance.getUIComponent().showTips(1);
						return;
					}
				}
			}
		}
	}
	public int getThreeMaxID(JsonData data)
	{
		if (data.Count == 5) {
			if (((int)data [0] - 1) / 4 == ((int)data [1] - 1) / 4 &&
			    ((int)data [0] - 1) / 4 == ((int)data [2] - 1) / 4) {
				return (int)data [0];
			}
			if (((int)data [2] - 1) / 4 == ((int)data [3] - 1) / 4 &&
			    ((int)data [2] - 1) / 4 == ((int)data [4] - 1) / 4) {
				return (int)data [2];
			}
		} else if (data.Count == 4) {
			if (((int)data [0] - 1) / 4 == ((int)data [1] - 1) / 4) {
				return (int)data [0];
			} else {
				return (int)data [1];
			}
		}

		return -1;
	}

	public int getFourTakeMaxID(Dictionary<int,int> dic)
	{
		List<int> arrayList = new List<int> ();
		int tempMaxID = -1;
		foreach (var id in dic) {
			if (id.Value == 4) {
				tempMaxID = id.Key;
			}
		}
		return tempMaxID;
	}

	public CardsType getCardsType(JsonData jsonData)
	{
		if (this.isOne (jsonData)) {
			return CardsType.One;
		} else if (this.isDuiZi (jsonData)) {
			return CardsType.Duizi;
		} else if (this.isShunzi (jsonData)) {
			return CardsType.Shunzi;
		} else if (this.isZhaDan (jsonData)) {
			return CardsType.Zhadan;
		} else if (this.isThreeTakeOne (jsonData)) {
			return CardsType.ThreeTakeOne;
		} else if (this.isLianDui (jsonData)) {
			return CardsType.LianDui;
		} else if (this.isWangZha (jsonData)) {
			return CardsType.Wangzha;
		} else if (this.isThreeTakeTwo (jsonData)) {
			return CardsType.ThreeTakeTwo;
		} else if (this.isZhaDan (jsonData)) {
			return CardsType.Zhadan;
		} else if (this.isFeijiTakeWind_Dan (jsonData)) {
			return CardsType.FeijiTakeWind_Dan;
		} else if (this.isThree (jsonData)) {
			return CardsType.Three;
		} else if (this.isFourTakeTwo_Dan (jsonData)) {
			return CardsType.FourTakeTwo;
		} else if (this.isFourTakeTwo_Dui (jsonData)) {
			return CardsType.FourTakeDui;
		} else if (this.isFeijiTakeWind_Dui (jsonData)) {
			return CardsType.FeijiTakeWind_Dui;
		} else if (this.isFeiji (jsonData)) {
			return CardsType.Feiji;
		}
		return CardsType.ErrType;

	}

	public bool isOne(JsonData data)
	{
		return (data.Count == 1);
	}
	public bool isDuiZi(JsonData data)
	{
		if (data.Count != 2) {
			return false;
		} else {
			for (int i = 0; i < data.Count; i++) {
				if ((int)data [i] == 54 || (int)data [i] == 53) {
					return false;
				}
			}
			return (((int)data [0]-1) / 4 == ((int)data [1]-1)/ 4);
		}
	}
	public bool isWangZha(JsonData data)
	{
		if (data.Count != 2) {
			return false;
		} else {
			return 	((int)data [0] == 54 && (int)data [1] == 53);
		}
	}
	public bool isShunzi(JsonData data)
	{
		if (data.Count < 5 || data.Count > 12 || (int)data[0]>48) {
			return false;
		} else {
			for(int i =0;i<data.Count-1;i++)
			{
				if (((int)data [i]-1)/4 - 1 != ((int)data [i + 1]-1)/4) {
					return  false;
				} else {
					
				}
			}
			return true;
		}

	}



	public bool isZhaDan(JsonData data)
	{
		if (data.Count != 4) {
			return false;
		} else {
			if ((int)data [0] == 54 || (int)data [0] == 53) {
				return false;
			}
			for (int i = 1; i < data.Count; i++) {
				if (((int)data [0]-1) / 4 != ((int)data [i]-1) / 4) {
					return false;
				}
			}
			return true;
		}
	}
	public bool isThree(JsonData data)
	{
		if (data.Count != 3) {
			return false;
		} else {
			if ((int)data [0] == 54 || (int)data [0] == 53) {
				return false;
			}
			return (((int)data [0] - 1) / 4 == ((int)data [1] - 1) / 4 && ((int)data [0] - 1) / 4 == ((int)data [2] - 1) / 4);
		}
	}

	public bool isThreeTakeOne(JsonData data)
	{
		if (data.Count != 4) {
			return false;
		} else {
			bool b = false;
			if (((int)data [0] - 1) / 4 == ((int)data [1] - 1) / 4 &&
				((int)data [0] - 1) / 4 == ((int)data [2] - 1) / 4 &&
				((int)data [3] - 1) / 4 != ((int)data [2] - 1) / 4)
			{
				b  = true;
			}else if(((int)data [1] - 1) / 4 == ((int)data [2] - 1) / 4 &&
				((int)data [1] - 1) / 4 == ((int)data [3] - 1) / 4 &&
				((int)data [0] - 1) / 4 != ((int)data [1] - 1) / 4){
				b = true;
				
			}
			else {
				b =  false;
			}
			return b;
		}

	}

	public bool isThreeTakeTwo(JsonData data)
	{
		if (data.Count != 5) {
			return false;
		} else {
			bool b = false;
			if (((int)data [0] - 1) / 4 == ((int)data [1] - 1) / 4 &&
				((int)data [0] - 1) / 4 == ((int)data [2] - 1) / 4 &&
				((int)data [3] - 1) / 4 != ((int)data [2] - 1) / 4 &&
				((int)data [3] - 1) / 4 == ((int)data [4] - 1) / 4) 
			{
				b = true ;
			} else if(((int)data [2] - 1) / 4 == ((int)data [3] - 1) / 4 &&
				((int)data [2] - 1) / 4 == ((int)data [4] - 1) / 4 &&
				((int)data [2] - 1) / 4 != ((int)data [1] - 1) / 4 &&
				((int)data [0] - 1) / 4 == ((int)data [1] - 1) / 4){
				b = true;
			}
			else {
				b = false;
			}
			return b;
		}

	}
	public bool isLianDuiForElse(JsonData data)//专门为检测四带2的时候用到，因此牌的类型不用限制大王小王
	{
		if (data.Count < 6) {
			return false;
		}
		bool b = false;
		for (int i = 0; i < data.Count - 2; i += 2) {
			if (((int)data [i] - 1) / 4 == ((int)data [i + 1] - 1) / 4 &&//首先比较是否都为对子
				((int)data [i] - 1) / 4 - 1 == ((int)data [i + 2] - 1) / 4 && //再比较第一个对子的点数-1是否等于第二个对子
				((int)data [i + 2] - 1) / 4 == ((int)data [i + 3] - 1) / 4) {//最后检察最小的两个是否为对子（这里的for循环无法检测到最小的两个，所以需要拿出来单独检查） 
				b = true;
			} else {
				b =  false;
			}
		}
		return b;
	}

	public bool isLianDui(JsonData data)
	{
		if (data.Count < 6 || (int)data[0]>48) {
			return false;
		}
		for (int i = 0; i < data.Count - 2; i += 2) {
			if (((int)data [i] - 1) / 4 != ((int)data [i + 1] - 1) / 4 ||//首先比较是否都为对子
				((int)data [i] - 1) / 4 - 1 != ((int)data [i + 2] - 1) / 4 || //再比较第一个对子的点数-1是否等于第二个对子
				((int)data [i + 2] - 1) / 4 != ((int)data [i + 3] - 1) / 4){//最后检察最小的两个是否为对子（这里的for循环无法检测到最小的两个，所以需要拿出来单独检查） 
				return false;
			} 
		}
		return true;
	}
	public bool isFeijiTakeWind_Dui(JsonData data)
	{
		Dictionary<int,int> dic = new Dictionary<int,int> ();
		List<int> sectionList = new List<int> ();
		if (data.Count < 10 || data.Count % 5 != 0) {
			return false;
		}else {
			dic = this.getTypeDic (data);
			if ((data.Count == 10 && dic.Count == 4) ||
				(data.Count == 15 && dic.Count == 6) ||
				(data.Count == 20 && dic.Count == 8)) {
				sectionList = this.getSectionlist (dic);
				if (this.isRuleFeiji (sectionList)) {
					return true;
				} else {
					return false;
				}
			}
		}
		return false;
	}

	public bool isFeijiTakeWind_Dan(JsonData data)
	{
		Dictionary<int,int> dic = new Dictionary<int,int> ();
		List<int> sectionList = new List<int> ();
		if (data.Count % 4 != 0 || data.Count < 8) {
			return false;
		} else {
			dic = this.getTypeDic (data);
			if ((data.Count == 8 && dic.Count >= 3) ||
			    (data.Count == 12 && dic.Count >= 4) ||
			    (data.Count == 16 && dic.Count >=5) ||
			    (data.Count == 20 && dic.Count >=7)) {
				sectionList = this.getSectionlist (dic);
				if (data.Count == 8 && sectionList.Count != 2) {
					return false;
				} else if (data.Count == 12 && sectionList.Count != 3) {
					return false;
				}
				else if (data.Count == 16 && sectionList.Count != 4) {
					return false;
				}
				else if (data.Count == 20 && sectionList.Count != 5) {
					return false;
				}
				if (this.isRuleFeiji (sectionList)) {
					return true;
				} else {
					return false;
				}
			}
		}
		return false;
	}
	public List<int> getSectionlist (Dictionary<int,int> dic)
	{
		List<int> sectionList = new List<int> ();
		foreach (var item in dic) {
			if (item.Value == 3 && item.Key != 12) {
				sectionList.Add (item.Key);
			}
		}
		sectionList.Sort ();//从小往高排序
		return sectionList;
	}
	public int getFeijiTakeWingMaxSection(JsonData data)
	{
		Dictionary<int,int> dic = new Dictionary<int,int> ();
		List<int> sectionList = new List<int> ();
		dic = this.getTypeDic (data);
		sectionList = this.getSectionlist (dic);
		int maxIndex = sectionList[sectionList.Count - 1];
		return maxIndex;
	}
	public bool isRuleFeiji(List<int> sectionList){
		if (sectionList.Count < 2) {
			return false;
		} else {
			for (int i = 0; i < sectionList.Count-1; i++) {
				if (sectionList [i] + 1 != sectionList [i + 1]) {
					return false;
				}
			}
		}
		return true;
	}

	public bool isFourTakeTwo_Dan(JsonData data)
	{
		if (data.Count != 6) {
			return false;
		} else {
			Dictionary<int,int> dic = new Dictionary<int, int> ();
			dic = this.getTypeDic (data);
			if (dic.Count != 3) {
				return false;
			} else {
				foreach (var item in dic) {
					if (item.Value == 4) {
						return true;
					}
				}
				return false;
			}
		}
	}
	public bool isFourTakeTwo_Dui(JsonData data)
	{
		if (data.Count != 8) {
			return false;
		} else {
			Dictionary<int,int> dic = new Dictionary<int, int> ();
			dic = this.getTypeDic (data);
			if (dic.Count != 3) {
				return false;
			} else {
				foreach (var item in dic) {
					if (item.Value == 1 || item.Value == 3) {
						return false;
					}
				}
				return true;
			}
		}
	}

	public Dictionary<int,int> getTypeDic(JsonData data)//获得区间有多少以及对应的数量key为区间ID，values为数量
	{
		Dictionary<int,int> array = new Dictionary<int,int> ();
		for (int i = 0; i < data.Count; i++) {
			int lev = ((int)data [i] - 1) / 4;
			if (!array.ContainsKey (lev)) {
				array.Add (lev, 1);
			} else {
				array [lev]++;
			}
		}
		return array;
	}
		
	public bool isFeiji(JsonData data)
	{
		if (data.Count < 6 || data.Count % 3 != 0 || (int)data[0] >52 ) {
			return false;
		} else {

			Dictionary<int,int> dic = new Dictionary<int, int> ();
			dic = this.getTypeDic (data);
			if (dic.Count != data.Count/3 ) {
				return false;
			} else {
				List<int> keyList = new List<int> (dic.Keys);
				for (int i = 0; i < dic.Count-1; i++) {
					if (keyList[i] - 1 != keyList [i + 1] || dic[keyList[i]] != 3) {
						return false;
					}
				}
				return true;
			}
		}

	}
}
