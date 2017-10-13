using UnityEngine;
using System.Collections;
using LitJson;

public class RankItemComponent : GameUIComponent {

	UISprite icon_sp;
	UILabel name_lbl;
	UILabel gold_lbl;
	UILabel rank_lbl;

	void Awake()
	{
		icon_sp = transform.FindChild ("icon").GetComponent<UISprite> ();
		name_lbl = transform.FindChild ("name").GetComponent<UILabel> ();
		gold_lbl = transform.FindChild ("gold").GetComponent<UILabel> ();
		rank_lbl = transform.FindChild ("rank").GetComponent<UILabel> ();
		rank_lbl.gameObject.SetActive (false);
	}

	public void setData(JsonData jsonData)
	{
		string rank = jsonData [0].ToString();
		string name = jsonData [1].ToString ();
		float gold = float.Parse (jsonData [2].ToString ());
		if (int.Parse (rank) < 6) {
			icon_sp.spriteName = "rank_" + rank;
		} else {
			icon_sp.gameObject.SetActive (false);
			rank_lbl.gameObject.SetActive (true);
			rank_lbl.text = rank;
		}

		name_lbl.text = name;
		gold_lbl.text = gold.ToString ();

	}
}
