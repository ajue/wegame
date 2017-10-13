using UnityEngine;
using System.Collections.Generic;
using LitJson;

/// <summary>
/// 斗地主提示逻辑
/// </summary>
public class DDZTips : SingletonNew<DDZTips> {

	public static Dictionary<CardsType,string> CardsType_String = new Dictionary<CardsType,string> (){
		{ CardsType.One,"CardsType.One" },
		{ CardsType.Duizi,"CardsType.Duizi" },
		{ CardsType.Shunzi,"CardsType.Shunzi" },
		{ CardsType.Wangzha,"CardsType.Wangzha" },
		{ CardsType.ThreeTakeOne,"CardsType.ThreeTakeOne" },
		{ CardsType.Zhadan,"CardsType.Zhadan" },
		{ CardsType.Three,"CardsType.Three" },
		{ CardsType.ThreeTakeTwo,"CardsType.ThreeTakeTwo" },
		{ CardsType.LianDui,"CardsType.LianDui" },
		{ CardsType.FeijiTakeWind_Dan,"CardsType.FeijiTakeWind_Dan" },
		{ CardsType.Feiji,"CardsType.Feiji" },
		{ CardsType.FourTakeTwo,"CardsType.FourTakeTwo" },
		{ CardsType.FourTakeDui,"CardsType.FourTakeDui" },
		{ CardsType.FeijiTakeWind_Dui,"CardsType.FeijiTakeWind_Dui" }
	};

	int tipsCount;              //提示计数,会循环
	int allCount;              //总提示次数,用于是否判断是否是初始
	int seqStartLev;          //记录顺子连队等成连后的起始Leve
	JsonData handCardsJson;
	JsonData lastCardsJson;

	int lastMaxLev;     	   	//上家出牌最大的levID
	int lastMinLev;				//上家出牌最小的levID，用于处理顺子
	int lastCardsCount;     	//上家出牌数量
	JsonData result = null;		//提示结果

	public Dictionary<int,JsonData> levelCardsDic = new Dictionary<int,JsonData>();
	Dictionary<CardsType,List<int>> typeLevelsDic = new Dictionary<CardsType,List<int>> ();

	/// <param name="myCards">玩家手牌</param>
	/// <param name="lastCards">上家出牌</param>
	public void initTips(JsonData handCards,JsonData lastCards){
		this.tipsCount = 0;
		this.allCount = 0;
		this.lastMaxLev = -1;
		this.lastMinLev = -1;
		this.lastCardsCount = 0;
		this.bWangZha = false;
		this.zhadanCount = 0;
		this.seqStartLev = 0;
		this.handCardsJson = null;
		this.lastCardsJson = null;
		this.result = null;

		this.handCardsJson = handCards;

		if (lastCards != null && lastCards.Count > 0) {
			this.lastCardsJson = lastCards;
			//计算lastCards最大值
			this.initLastCardsInfos ();
		}
		//拆分手牌
		this.levelCardsDic = assordLevelCards (handCardsJson);
		this.typeLevelsDic = assordTypeLevels (levelCardsDic);
	}
	/// <summary>
	/// 计算上家手牌的信息
	/// </summary>
	int initLastCardsInfos(){
		//count
		lastCardsCount = lastCardsJson.Count;
		Dictionary<int,JsonData> data = assordLevelCards (lastCardsJson);
		//min
		int curLevCount = 0;
		foreach (var k in data.Keys) {
			if (this.lastMinLev == -1) {
				lastMinLev = k;
				curLevCount = data [k].Count;
			} else if (curLevCount < data [k].Count) {
				lastMinLev = k;
				curLevCount = data [k].Count;
			} else if (curLevCount == data[k].Count) {
				if (lastMinLev > k) {
					lastMinLev = k;
				}
			}
		}
		//max
		curLevCount = 0;
		foreach (var k in data.Keys) {
			if (data [k].Count > curLevCount) {
				curLevCount = data [k].Count;
				lastMaxLev = k;
			} else if(data [k].Count == curLevCount){
				if (k > lastMaxLev) {
					lastMaxLev = k;
				}
			}
		}

		if (lastMaxLev == -1 || lastMinLev == -1) {
			Debug.LogError ("error lastMaxLev || lastMinLev == -1");
		}
		return lastMaxLev;
	}
	Dictionary<int,JsonData> assordLevelCards(JsonData data){
		
		Dictionary<int,JsonData> cardsLevDic = new Dictionary<int, JsonData>();
		for (int i = 0; i < data.Count; i++) {
			int id = (int)data [i];
			int lev = (id - 1) / 4;
			if (cardsLevDic.ContainsKey (lev)) {
				cardsLevDic [lev].Add (data [i]);
			} else {
				cardsLevDic.Add (lev, new JsonData (){data [i]});
			}
		}
//		foreach (var k in cardsLevDic.Keys) {
//			Debug.Log (k + ": "+cardsLevDic[k].ToJson());
//		}
		return cardsLevDic;
	}
	Dictionary<CardsType,List<int>> assordTypeLevels(Dictionary<int,JsonData> cardsLevDic){
		Dictionary<CardsType,List<int>> levDic = new Dictionary<CardsType, List<int>> ();
		foreach (var k in cardsLevDic.Keys) {
			if (cardsLevDic [k].Count == 1) {
				if (levDic.ContainsKey (CardsType.One)) {
					levDic[CardsType.One].Add(k);
				} else {
					levDic.Add (CardsType.One, new List<int> (){ k });
				}
			}else if (cardsLevDic [k].Count == 2 && k != 13){
				if (levDic.ContainsKey (CardsType.Duizi)) {
					levDic[CardsType.Duizi].Add(k);
				} else {
					levDic.Add (CardsType.Duizi, new List<int> (){ k });
				}
			}else if (cardsLevDic [k].Count == 3){
				if (levDic.ContainsKey (CardsType.Three)) {
					levDic[CardsType.Three].Add(k);
				} else {
					levDic.Add (CardsType.Three, new List<int> (){ k });
				}
			}else if (cardsLevDic [k].Count == 4){
				if (levDic.ContainsKey (CardsType.Zhadan)) {
					levDic[CardsType.Zhadan].Add(k);
				} else {
					levDic.Add (CardsType.Zhadan, new List<int> (){ k });
				}
			}else if (cardsLevDic [k].Count == 2){
				if (levDic.ContainsKey (CardsType.Wangzha)) {
					levDic[CardsType.Wangzha].Add(k);
				} else {
					levDic.Add (CardsType.Wangzha, new List<int> (){ k });
				}
			}
		}

//		foreach (var k in levDic.Keys) {
//			Debug.Log ("key = " + k + " count:"+levDic[k].Count);
//		}
		return levDic;
	}
	/// <summary>
	/// 调用该方法时，应在每回合initTips()后。
	/// 玩家每点击一次提示按钮，则客户端调用一次getTips获取对应提示结果
	/// 当第一次getTips获取结果为null,则为没牌出，直接为玩家选择不出。
	/// 后续getTips结果为null，则为一次提示轮回，客户端表现为重置牌组。
	/// </summary>
	/// <returns>The tips.</returns>
	public JsonData getTips(){

		this.allCount++;
		if (result == null && (lastCardsJson == null || lastCardsJson.Count <= 0)) {
			tipsCount++;
			return this.getMinTips (tipsCount);
		}

		CardsType type = checkType (this.lastCardsJson);
		result = null;
		if ( type == CardsType.One) {
			result = returnDanzhang (tipsCount);
		}else if (type == CardsType.Duizi) {
			result = returnDuizi (tipsCount);
		}else if (type == CardsType.Three) {
			result = returnThree (tipsCount);
		}else if (type == CardsType.ThreeTakeOne) {
			result = returnThreeTakeOne (tipsCount);
		}else if (type == CardsType.ThreeTakeTwo) {
			result = returnThreeTakeTwo (tipsCount);
		}else if (type == CardsType.Shunzi) {
			result = returnShunzi (tipsCount);
		}else if (type == CardsType.LianDui) {
			result = returnLiandui (tipsCount);
		}else if (type == CardsType.Feiji) {
			result = returnFeiji (tipsCount);
		}else if (type == CardsType.FeijiTakeWind_Dan) {
			result = returnFeijiTakeDan(tipsCount);
		}else if (type == CardsType.FeijiTakeWind_Dui) {
			result = returnFeijiTakeDuizi(tipsCount);
		}else if (type == CardsType.Zhadan) {
			result = returnFour (tipsCount);
		}

		sortJsonData (result);

		if (result != null) 
		{
			tipsCount++;
			return result;
		}
		else if(result == null && type != CardsType.Zhadan && type != CardsType.Wangzha){
			return returnZhadan ();
		}
		return null;
	}

	JsonData getMinTips(int count)
	{
		//因为提示只有一个，造成顿挫感
		if(count%2 == 0){
			return null;
		}
		int lev = this.getHandCardsMinLev ();
		return levelCardsDic [lev];
	}
	public int getHandCardsMinLev()
	{
		List<int> levList = new List<int> (levelCardsDic.Keys);
		return levList [levelCardsDic.Count - 1];
	}
	//查找牌型的牌比指定Level大的牌
	protected int findMorethanLevel(CardsType type,int maxLev){
		int local = -1;
		for (int i = typeLevelsDic [type].Count - 1; i >= 0; i--) {
			if (typeLevelsDic [type][i] > lastMaxLev || typeLevelsDic [type][i] == 13) {
				local = i;
				break;
			}
		}
		return local;
	}
	//返回单张
	JsonData returnDanzhang(int count){
		JsonData result = new JsonData ();
		result.SetJsonType (JsonType.Array);
	
		//如果有单张,则提示单牌
		int local = -1;
		if (typeLevelsDic.ContainsKey (CardsType.One)) {
			//如果牌形不够大，则返回-1
			local = this.findMorethanLevel(CardsType.One,this.lastMaxLev);
			if (local - count >= 0) {
				int lev = typeLevelsDic [CardsType.One] [local - count];
				int cid = (int)levelCardsDic [lev][0];
				//手牌是大王，或者上家牌不出王，才有牌可出
				if (cid > 53 || lastMaxLev != 13) {
					result.Add (cid);
					return result;
				}
			}
		}
		//如果没有单牌或者单排不够大的情况，则按打大牌轮询
		if (result.Count <= 0 && local == -1){
			for (int i = handCardsJson.Count-1; i >=0 ; i--) {
				int lev = ((int)handCardsJson [i] - 1) / 4;
				if (lev > lastMaxLev) {
					local = i;
					break;
				}
			}
			if (local - count >= 0) {
				result.Add(handCardsJson [local - count]);
				return result;
			}
		}
		return null;
	}
	//返回对子
	JsonData returnDuizi(int count){
		JsonData result = new JsonData ();
		result.SetJsonType (JsonType.Array);

		int local = -1;
		if (typeLevelsDic.ContainsKey (CardsType.Duizi)) {
			local = findMorethanLevel (CardsType.Duizi, lastMaxLev);
			if (local - count >= 0) {
				int lev = typeLevelsDic [CardsType.Duizi] [local - count];
				result = levelCardsDic [lev];
				return result;
			}
		}
		//如果没有对子,或对子不够大，则拆三张
		if(typeLevelsDic.ContainsKey (CardsType.Three) && result.Count <= 0 && local == -1){
			local = findMorethanLevel (CardsType.Three, lastMaxLev);
			if (local - count >= 0) {
				int lev = typeLevelsDic [CardsType.Three] [local - count];
				for (int i = 1; i < levelCardsDic [lev].Count; i++) {
					result.Add (levelCardsDic [lev] [i]);
				}
				return result;
			}
		}
		return null;
	}
	//返回三张
	JsonData returnThree(int count){
		if (typeLevelsDic.ContainsKey (CardsType.Three)) {
			int local = findMorethanLevel (CardsType.Three, lastMaxLev);
			if (local - count >= 0) {
				int lev = typeLevelsDic [CardsType.Three] [local - count];
				return levelCardsDic [lev];
			}
		}
		return null;
	}
	//返回三带1
	JsonData returnThreeTakeOne(int count){
		if (handCardsJson.Count >= 4) {
			JsonData data = copyJsonData(returnThree (count));
			if (data != null && data.Count > 0) {
				if (typeLevelsDic.ContainsKey (CardsType.One)) {
					int size = typeLevelsDic [CardsType.One].Count;
					int lev = typeLevelsDic [CardsType.One] [size - 1];
					JsonData add = levelCardsDic [lev];
					data.Add (add[0]);
					return data;
				} else if(typeLevelsDic.ContainsKey (CardsType.Duizi)){
					int size = typeLevelsDic [CardsType.Duizi].Count;
					int lev = typeLevelsDic [CardsType.Duizi] [size - 1];
					JsonData add = levelCardsDic [lev];
					data.Add (add[1]);
					return data;
				}else if(typeLevelsDic.ContainsKey (CardsType.Three)){
					int size = typeLevelsDic [CardsType.Three].Count;
					//如果不是已选择的三张，则拿出一牌
					for (int i = size - 1; i >= 0 ; i--) {
						int tmpLev = typeLevelsDic [CardsType.Three] [i];
						JsonData tmpData = levelCardsDic [tmpLev];
						int lev1 = ((int)data [2] - 1) / 4;
						int lev2 = ((int)tmpData [2] - 1) / 4;
						if (lev1 != lev2) {
							data.Add (tmpData[2]);
							return data;
						}
					}
				}
			}
		}
		return null;
	}
	//返回三带2
	JsonData returnThreeTakeTwo(int count){
		if (handCardsJson.Count >= 5) {
			JsonData data = copyJsonData(returnThree (count));
			if (data != null && data.Count > 0) {
				if(typeLevelsDic.ContainsKey (CardsType.Duizi)){
					int size = typeLevelsDic [CardsType.Duizi].Count;
					int lev = typeLevelsDic [CardsType.Duizi] [size - 1];
					JsonData add = levelCardsDic [lev];
					for (int i = 0; i < add.Count; i++) {
						data.Add (add [i]);
					}
					return data;
				}else if(typeLevelsDic.ContainsKey (CardsType.Three)){
					int size = typeLevelsDic [CardsType.Three].Count;
					for (int i = size - 1; i >= 0 ; i--) {
						int tmpLev = typeLevelsDic [CardsType.Three] [i];
						JsonData tmpData = levelCardsDic [tmpLev];
						int lev1 = ((int)data [0] - 1) / 4;
						int lev2 = ((int)tmpData [0] - 1) / 4;
						if (lev1 != lev2) {
							for (int j = 1; j < tmpData.Count; j++) {
								data.Add (tmpData [j]);
							}
						}
					}
					return data;
				}
			}
		}
		return null;
	}
	//返回顺子
	JsonData returnShunzi(int count){

		if (handCardsJson.Count < lastCardsCount && lastCardsCount < 5)
			return null;
		
		//起始点为lastMinLev+1

		JsonData result = new JsonData ();
		int len = 0;
		int i = 0;
		if (this.seqStartLev > lastMinLev) {
			i = seqStartLev + count;
		} else {
			i = lastMinLev + 1 + count;
		}
		for(;i<12;i++){
			if (levelCardsDic.ContainsKey (i)) {
				if (len == 0) {
					seqStartLev = i;
				}
				len++;
				//取最小牌
				JsonData data = levelCardsDic [i] [levelCardsDic [i].Count - 1];
				result.Add (data);
				if (len >= lastCardsCount) {
					return sortJsonData(result);
				}
			} else {
				len = 0;
				result.Clear ();
			}
		}
		seqStartLev = 0;
		return null;
	}
	JsonData returnLiandui(int count){
		if (handCardsJson.Count < lastCardsCount && lastCardsCount < 6)
			return null;
		int seqLen = lastCardsCount / 2;
		//起始点为lastMinLev+1
		JsonData result = new JsonData ();
		int len = 0;
		int i = 0;
		if (this.seqStartLev > lastMinLev) {
			i = seqStartLev + count;
		} else {
			i = lastMinLev + 1 + count;
		}
		for(;i<12;i++){
			if (levelCardsDic.ContainsKey (i) && levelCardsDic[i].Count >=2) {
				if (len == 0) {
					seqStartLev = i;
				}
				len++;
				//取最小2张牌
				for(int j = levelCardsDic [i].Count-1;j >= levelCardsDic [i].Count-2;j--){
					JsonData data = levelCardsDic [i] [j];
					result.Add (data);
				}
				if (len >= seqLen) {
					return sortJsonData(result);
				}
			} else {
				len = 0;
				result.Clear ();
			}
		}
		seqStartLev = 0;
		return null;
	}
	JsonData returnFeiji(int count){
		if (handCardsJson.Count < lastCardsCount || lastCardsCount < 6)
			return null;
		int seqLen = lastCardsCount / 3;
		//起始点为lastMinLev+1
		int len = 0;
		int i = 0;
		if (this.seqStartLev > lastMinLev) {
			i = seqStartLev + count;
		} else {
			i = lastMinLev + 1 + count;
		}
		JsonData result = new JsonData ();
		for(;i<12;i++){
			if (levelCardsDic.ContainsKey (i) && levelCardsDic[i].Count >=3) {
				if (len == 0)
					seqStartLev = i;
				len++;
				//取最小3张牌
				for(int j = levelCardsDic [i].Count-1;j >= levelCardsDic [i].Count-3;j--){
					JsonData data = levelCardsDic [i] [j];
					result.Add (data);
				}
				if (len >= seqLen) {
					return sortJsonData(result);
				}
			} else {
				len = 0;
				result.Clear ();
			}
		}
		seqStartLev = 0;
		return null;
	}
	//返回飞机带单张
	JsonData returnFeijiTakeDan(int count){
		if (handCardsJson.Count < lastCardsCount || lastCardsCount < 8)
			return null;
		//连续长度
		int seqLen = lastCardsCount / 4;
		//copy对象
		Dictionary<int,JsonData> cardsLevDic = new Dictionary<int, JsonData> (this.levelCardsDic);

		JsonData result = new JsonData ();
		result.SetJsonType (JsonType.Array);
		int len = 0;
		int ii = 0;
		if (this.seqStartLev > lastMinLev) {
			ii = seqStartLev + count;
		} else {
			ii = lastMinLev + 1 + count;
		}
		JsonData keyResult = new JsonData ();
		for(;ii < 12;ii++){
			if (cardsLevDic.ContainsKey (ii) && cardsLevDic[ii].Count >=3) {
				if (len == 0)
					seqStartLev = ii;
				len++;
				//取最小3张牌
				for(int j = cardsLevDic [ii].Count-1;j >= cardsLevDic [ii].Count-3;j--){
					JsonData data = cardsLevDic [ii] [j];
					result.Add (data);
				}
				keyResult.Add (ii);
				if (len >= seqLen) {
					sortJsonData(result);
					//删除取出的牌
					for(int k=0;k < keyResult.Count;k++){
						cardsLevDic.Remove ((int)keyResult [k]);
					}
					break;
				}
			} else {
				len = 0;
				result.Clear ();
				keyResult.Clear ();
			}
		}
		if (len < seqLen) {
			len = 0;
			seqStartLev = 0;
			result.Clear ();
			keyResult.Clear ();
		}
		//从剩下的牌里抽取N张最小的牌
		if (result.Count > 0) {
			if (typeLevelsDic.ContainsKey (CardsType.One) && seqLen > 0) {
				for (int i = typeLevelsDic[CardsType.One].Count - 1; i >= 0; i--) {
					int lev = typeLevelsDic [CardsType.One] [i];
					result.Add (cardsLevDic [lev][0]);
					seqLen--;
					if (seqLen <= 0) {
						return sortJsonData(result);
					}
				}
			}
			//检查对子
			if (typeLevelsDic.ContainsKey (CardsType.Duizi) && seqLen > 0) {
				for (int i = typeLevelsDic[CardsType.Duizi].Count - 1; i >= 0; i--) {
					int lev = typeLevelsDic [CardsType.Duizi] [i];
					JsonData duizi = cardsLevDic [lev];
					for (int k = duizi.Count - 1; k >= 0; k--) {
						result.Add (duizi [k]);
						seqLen--;
						if (seqLen <= 0) {
							return sortJsonData(result);
						}
					}
				}
			}
			//检查三张
			if (typeLevelsDic.ContainsKey (CardsType.Three) && seqLen > 0) {
				for (int i = typeLevelsDic[CardsType.Three].Count - 1; i >= 0; i--) {
					int lev = typeLevelsDic [CardsType.Three] [i];
					JsonData duizi = cardsLevDic [lev];
					for (int k = duizi.Count - 1; k >= 0; k--) {
						result.Add (duizi [k]);
						seqLen--;
						if (seqLen <= 0) {
							return sortJsonData(result);
						}
					}
				}
			}
		}

		return null;
	}
	//返回飞机带对子
	JsonData returnFeijiTakeDuizi(int count){
		if (handCardsJson.Count < lastCardsCount || lastCardsCount < 10)
			return null;
		//连续长度
		int seqLen = lastCardsCount / 5;
		//copy对象
		Dictionary<int,JsonData> cardsLevDic = new Dictionary<int, JsonData> (this.levelCardsDic);
		int len = 0;
		int ii = 0;
		if (this.seqStartLev > lastMinLev) {
			ii = seqStartLev + count;
		} else {
			ii = lastMinLev + 1 + count;
		}
		JsonData result = new JsonData ();
		result.SetJsonType (JsonType.Array);
		JsonData keyResult = new JsonData ();
		for(;ii < 12;ii++){
			if (cardsLevDic.ContainsKey (ii) && cardsLevDic[ii].Count >=3) {
				if (len == 0)
					seqStartLev = ii;
				len++;
				//取最小3张牌
				for(int j = cardsLevDic [ii].Count-1;j >= cardsLevDic [ii].Count-3;j--){
					JsonData data = cardsLevDic [ii] [j];
					result.Add (data);
				}
				keyResult.Add (ii);
				if (len >= seqLen) {
					sortJsonData(result);
					//删除取出的牌
					for(int k=0;k < keyResult.Count;k++){
						cardsLevDic.Remove ((int)keyResult [k]);
					}
					break;
				}
			} else {
				len = 0;
				result.Clear ();
				keyResult.Clear ();
			}
		}
		if (len < seqLen) {
			len = 0;
			seqStartLev = 0;
			result.Clear ();
			keyResult.Clear ();
		}
		//从剩下的牌里抽取N张最小的牌
		if (result.Count > 0) {
			//检查对子
			if (typeLevelsDic.ContainsKey (CardsType.Duizi) && seqLen > 0) {
				for (int i = typeLevelsDic[CardsType.Duizi].Count - 1; i >= 0; i--) {
					int lev = typeLevelsDic [CardsType.Duizi] [i];

					JsonData duizi = cardsLevDic [lev];
					for (int k = 0; k < duizi.Count; k++) {
						result.Add (duizi [k]);
					}
					seqLen--;
					if (seqLen <= 0) {
						return sortJsonData(result);
					}
				}
			}
			//检查三张
			if (typeLevelsDic.ContainsKey (CardsType.Three) && seqLen > 0) {
				for (int i = typeLevelsDic[CardsType.Three].Count - 1; i >= 0; i--) {
					int lev = typeLevelsDic [CardsType.Three] [i];

					JsonData three = cardsLevDic [lev];
					for (int k = three.Count - 1; k >= 1; k--) {
						result.Add (three [k]);
					}
					seqLen--;
					if (seqLen <= 0) {
						return sortJsonData(result);
					}
				}
			}
		}
		return null;
	}
	//返回炸弹，王炸
	JsonData returnFour(int count){
		if (typeLevelsDic.ContainsKey (CardsType.Zhadan)) {
			int local = findMorethanLevel (CardsType.Zhadan,lastMaxLev);
			//再确定到提示位置的lev值
			if (local - count >= 0) {
				int lev = typeLevelsDic [CardsType.Zhadan] [local - count];
				return levelCardsDic [lev];
			}
		}
		if (typeLevelsDic.ContainsKey (CardsType.Wangzha) && !bWangZha) {
			int lev = typeLevelsDic [CardsType.Wangzha] [0];
			bWangZha = true;
			return levelCardsDic [lev];
		}
		bWangZha = false;
		this.tipsCount = 0;
		return null;
	}
	int zhadanCount = 0;
	bool bWangZha = false;

	JsonData returnZhadan(){
		if(typeLevelsDic.ContainsKey(CardsType.Zhadan)){
			int count = typeLevelsDic [CardsType.Zhadan].Count;
			if (count - zhadanCount > 0) {
				int lev = typeLevelsDic [CardsType.Zhadan] [count - zhadanCount - 1];
				zhadanCount++;
				return levelCardsDic [lev];
			}
		}
		if (typeLevelsDic.ContainsKey (CardsType.Wangzha) && !bWangZha) {
			int lev = typeLevelsDic [CardsType.Wangzha] [0];
			bWangZha = true;
			return levelCardsDic [lev];
		}
		bWangZha = false;
		this.tipsCount = 0;
		this.zhadanCount = 0;
		return null;
	}
	public CardsType checkType(JsonData data){
		CardsType type = DDZRules.instance.getCardsType (data);
		if (type == CardsType.ErrType) {
			Debug.Log("checkType error type = -1");
		}
		return type;
	}
	public JsonData copyJsonData(JsonData data){
		if (data == null)
			return null;
		JsonData result = new JsonData ();
		for (int i = 0; i < data.Count; i++) {
			result.Add (data [i]);
		}
		return result;
	}
	public JsonData sortJsonData(JsonData data){
		if (data == null)
			return null;
		for (int i = 0; i < data.Count; i++) {
			for (int j = i; j < data.Count; j++) {
				if ((int)data [i] < (int)data [j]) {
					int tmp1 = (int)data [i];
					int tmp2 = (int)data [j];
					data [i] = tmp2;
					data [j] = tmp1;
				}
			}
		}
		return data;
	}
	public int Count{
		get{
			return tipsCount;
		}
	}
	public bool IsFirstTips{
		get{
			if (this.allCount == 1)
				return true;
			else
				return false;
		}
	}
	public JsonData getLastCardsJson(){
		return this.lastCardsJson;
	}
}