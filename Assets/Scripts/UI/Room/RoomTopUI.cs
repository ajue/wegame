using UnityEngine;
using System.Collections;

public class RoomTopUI : SingletonMono<RoomTopUI> {
	float totalzhu;
	float dizhu;
	float curzhu;

	GameObject totalzhuObj;
	GameObject dizhuObj;
	// Use this for initialization
	void Start () {
		totalzhuObj = transform.FindChild ("totalzhu").gameObject;
		dizhuObj = transform.FindChild ("dizhu").gameObject;
	}
	public void reset(){
		this.setTotalzhu (0);
	}

	public void setData(float totalzhu,float dizhu){
		this.totalzhu = totalzhu;
		this.dizhu = dizhu;
		this.curzhu = dizhu;

		totalzhuObj.GetComponent<UILabel> ().text = this.totalzhu.ToString ();
		dizhuObj.GetComponent<UILabel> ().text = this.dizhu.ToString ();

	}

	public void addTotalzhu(float add){
		this.totalzhu += add;
		totalzhuObj.GetComponent<UILabel> ().text = this.totalzhu.ToString ();
	}
	public void setTotalzhu(float total){
		this.totalzhu = total;
		totalzhuObj.GetComponent<UILabel> ().text = this.totalzhu.ToString ();
	}

	public float Curzhu{
		get{
			return curzhu;
		}
		set{
			curzhu = value;
		}
	}
	public float Dizhu{
		get{
			return dizhu;
		}
	}




}
