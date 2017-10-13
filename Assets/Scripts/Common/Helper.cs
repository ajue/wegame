using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Helper {

	public static GameObject primitiveLoad(string tarStr,string parentStr,Vector3 localpos){
		//load path 至 Resource目录下
		GameObject parent = GameObject.Find (parentStr);
		GameObject target = Resources.Load<GameObject> (tarStr);
		GameObject role = MonoBehaviour.Instantiate<GameObject>(target);
		role.transform.parent = parent.transform;
		role.transform.localPosition = localpos;
		role.transform.localScale = target.transform.localScale;
		role.transform.localRotation = target.transform.localRotation;

		return role;
	}
	public static GameObject primitiveLoad(string tarStr,Transform parent,Vector3 localpos){
		//load path 至 Resource目录下
//		Debug.Log ("Helper.primitiveLoad target = " + tarStr + " parent = " + parent.gameObject.name);
		GameObject target = Resources.Load<GameObject> (tarStr);
		GameObject role = MonoBehaviour.Instantiate<GameObject>(target);
		role.transform.parent = parent;
		role.transform.localPosition = localpos;
		role.transform.localScale = target.transform.localScale;
		role.transform.localRotation = target.transform.localRotation;
		
		return role;
	}
	public static GameObject primitiveLoad(string tarStr,Transform parent){
		//load path 至 Resource目录下
//		Debug.Log ("Helper.primitiveLoad target = " + tarStr + " parent = " + parent.gameObject.name);
		GameObject target = Resources.Load<GameObject> (tarStr);
		GameObject role = MonoBehaviour.Instantiate<GameObject>(target);
		role.transform.parent = parent;
		role.transform.localPosition = target.transform.localPosition;
		role.transform.localScale = target.transform.localScale;
		role.transform.localRotation = target.transform.localRotation;
		
		return role;
	}
	//设置服务器状态
	public static void setServerStatus(int status,UILabel label){
		if (status == 1) {
			label.text = "（流畅）";
			label.color = Color.green;
		}else if (status == 2) {
			label.text = "（良好）";
			label.color = Color.yellow;
		}else if (status == 3) {
			label.text = "（火爆）";
			label.color = Color.red;
		}
	}

	public static string getPassword(string phone){
		return phone + "lele2017";
	}
}
