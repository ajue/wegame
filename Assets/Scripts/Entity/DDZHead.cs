using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using KBEngine;

public class DDZHead : MonoBehaviour {

	UILabel nikeName;
	UILabel jinbiNum;
	UILabel addrLab;
	UILabel cardsNum;
	UILabel typeLab;
	UILabel multLab;
	UISprite headSp;
	UILabel saybox;
	UIGrid gird;
	Entity entity;

	DDZShowCards showCards;

	int sex = 1;

	void Awake()
	{
		typeLab = transform.FindChild ("type").GetComponent<UILabel> ();
		typeLab.gameObject.SetActive (false);

		nikeName = 	transform.FindChild ("nikename").GetComponent<UILabel> ();
		jinbiNum = 	transform.FindChild ("jinqian_num").GetComponent<UILabel> ();
		cardsNum = 	transform.FindChild ("card_num").GetComponent<UILabel> ();
		headSp 	= 	transform.FindChild ("touxiang").GetComponent<UISprite> ();
		multLab = 	transform.FindChild ("beishu").GetComponent<UILabel> ();
		addrLab	=	transform.FindChild ("addr").GetComponent<UILabel>();
		saybox	=	transform.FindChild ("saybox").GetComponent<UILabel> ();
		gird	=	transform.FindChild ("Grid").GetComponent<UIGrid> ();
		showCards	=	transform.FindChild ("showCards").GetComponent<DDZShowCards> ();
	}
	public Entity Player{
		set{
			entity = value;
			sex = (int)entity.getDefinedProperty ("sexC");
		}
	}
	public string Name{
		set{
			nikeName.text = value;
		}
	}

	public string Addr{
		set{
			addrLab.text = value;
		}
	}

	public string Head{
		set{
			headSp.spriteName = value;
		}
	}

	public int CID{
		set{
			if (value == Users.instance.cid){
				transform.localPosition = new Vector3 (-500, 137, 0);
				saybox.transform.localPosition = new Vector3 (500, -210, 0);
				this.name = "myHeadInfo";
			}else if(value == Users.instance.cid%3+1){
				transform.localPosition = new Vector3 (600, 446, 0);

				gird.transform.localPosition = new Vector3 (-145, -200, 0);
				gird.pivot = UIWidget.Pivot.TopRight;

				showCards.transform.localPosition = new Vector3 (-300, -350, 0);

				saybox.transform.FindChild ("Sprite").localEulerAngles = new Vector3 (0, 180, 0);
				saybox.transform.localPosition	= new Vector3 (-190, -210, 0);
				this.name = "nextHeadInfo";
			}else{
				transform.localPosition = new Vector3 (-500, 446, 0);

				showCards.transform.localPosition = new Vector3 (200, -350, 0);
				this.name = "backHeadInfo";
			}
		}
	}

	public string Gold{
		set{
			this.jinbiNum.text = value;
		}
	}

	public void updateCardsCount(int cardCount)
	{
		cardsNum.text = cardCount.ToString ();
	}
	public void setMultiple(int beishu)
	{
		multLab.text = "x" + beishu.ToString ();
	}
	public void setTuoGuan(bool isShow)
	{
		transform.FindChild ("tuoguan").gameObject.SetActive (isShow);
	}
	public void setPlayerType(int type)
	{
		if (type == 1) {
			typeLab.text = "地主";
			typeLab.gameObject.SetActive (true);
		} else if (type == 2) {
			typeLab.text = "农民";
			typeLab.gameObject.SetActive (true);
		} else {
			typeLab.text = "";
			typeLab.gameObject.SetActive (false);
		}
	}

	public void showRemainCards(JsonData cards){
		for (int i = 0; i < cards.Count; i++) {
			GameObject item = (GameObject)Instantiate (Resources.Load(GameSetting.UIPath + "Card1"),transform.position,Quaternion.identity);	
			item.transform.name =cards[i].ToString();
			item.transform.parent = this.gird.transform;
			item.transform.localScale = new Vector3 (1f, 1f, 1f);
			item.GetComponent<DDZCard>().ID = int.Parse(cards[i].ToString());
			item.GetComponent<DDZCard> ().BSelect = false;
			item.GetComponent<DDZCard> ().setCardData ();
			item.GetComponent<UISprite> ().depth = i;
		}

		this.gird.repositionNow = true;
		this.gird.Reposition ();
	}
	public void unshow(){
		this.saybox.gameObject.SetActive (false);
		this.showCards.transform.DestroyChildren ();
	}
	public void setSaybox(int score){
		if (entity == null)
			return;
		saybox.gameObject.SetActive (true);
	    sex	= (int)entity.getDefinedProperty ("sexC");
//		Debug.Log ("setSayboxsex" + sex);

		if (score == 0) {
			saybox.text = "不叫";
			AudioController.Instance.SoundDDZPlay ("ScoreNoOrder", sex);
		} else if (score == 11) {
			saybox.text = "不加倍";
			AudioController.Instance.SoundDDZPlay ("bujiabei", sex);
		} else if (score == 12) {
			saybox.text = "加倍";
			AudioController.Instance.SoundDDZPlay ("jiabei", sex);
		} else if (score > 0) {
			saybox.text = score.ToString () + "分";
			AudioController.Instance.SoundDDZPlay ("ScoreOrder" + score.ToString (), sex);
		} else {
			saybox.gameObject.SetActive (false);
		}
	}
	public void setShowCards(List<object> cards){
		if (entity == null)
			return;

		int type = (int)entity.getDefinedProperty ("type");
		if (type == 0)
			return;

		if (cards.Count <= 0) {
			saybox.text = "不出";
			saybox.gameObject.SetActive (true);
			int id = Random.Range(1,4);
			sex	= (int)entity.getDefinedProperty ("sexC");
			AudioController.Instance.SoundDDZPlay ("buyao"+id.ToString(),sex);
		} else {
			showCards.showCards (entity, cards);
		}

	}
}
