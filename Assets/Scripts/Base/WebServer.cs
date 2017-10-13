using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;  
using LitJson;

class WebServer:SingletonMono<WebServer>
{
	//高德IP
	public static string godUrl = "http://restapi.amap.com/v3/ip?key=2232bf05cec3a0e2c1690d61c66c0fe2";
	//百度IP
//	public static string url = "http://api.map.baidu.com/location/ip?ak=g6EfQCr1WeHvfDPTGkceiNRqEUIff5Zm&coor=bd09ll";

	public void autoPositionIP(){
		StartCoroutine(iplocal(godUrl));
	}
	IEnumerator iplocal(string url)
	{
		WWW getData = new WWW(url);  
		yield return getData;  
		if(getData.error != null)  
		{  
			Debug.Log(getData.error);
		}  
		else  
		{  
			JsonData json = JsonMapper.ToObject (getData.text);

			JsonData province = json ["province"];
			JsonData city = json ["city"];

			Users.instance.Addr = province.ToString()+city.ToString();
			Debug.Log("iplocal = "+Users.instance.Addr);
		}
	}



}