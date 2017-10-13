using UnityEngine;
using System.Collections.Generic;
using LitJson;
//炸金花卡牌管理
public class Cards : MonoBehaviour {
	List<Card> cardList = new List<Card>();
	Vector3[] cardVec;
	void Awake(){
		
		cardVec = new Vector3[3]{
			new Vector3 (-75, 0, 0),
			new Vector3 (0, 0, 0),
			new Vector3 (75, 0, 0)};

		for (int i=0;i< cardVec.Length;i++) {
			GameObject go = transform.FindChild("Card"+(i+1)).gameObject;
			go.transform.localPosition = cardVec[0];
			Card card = go.AddComponent<Card>();
			card.look(false);
			card.setMoveTo(cardVec[i]);
			cardList.Add(card);
		}
	}
	public void play(bool bShow){
		for (int i = 0; i<cardList.Count; i++) {
			Card card = cardList[i];
			if(bShow){
				card.PlayForward();
			}
			else{
				card.PlayReverse();
			}
		}
	}
	public void lookCards(bool bLook){
		foreach (Card card in cardList) {
			card.look(bLook);
		}
	}
	
	public void setCardsJson(string json,bool look){
		JsonData data = JsonMapper.ToObject (json);
		for (int i=0; i<cardList.Count; i++) {
			cardList[i].ID = (int)data[i];
			cardList[i].look(look);
		}
	}
}
