using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

public class WapPay : SingletonMono<WapPay> {
	/// 商户id
	string partner_id;
	/// 应用id
	string app_id;
	/// 支付方式(1 微信H5,2 支付宝H5,3 银联H5,4 微信扫码,5 微信公众号,6 QQ钱包)
	int wap_type;
	//商户订单号(需保证同⼀app_id下唯一，交易结果回调通知将传递此参数，支付结果查询也使用此参数。)
	string out_trade_no;
	//商户渠道代码
	string qn;
	//支付回调url
	string returnUrl;
	/// 充值金额
	int money;
	//商品名称
	string subject;
	//秘钥
	string key;

	string reqUrl = "http://pay.csl2016.cn:8000";
	string createOrder  = "/createOrder.e";
	string queryOrder	=	"/queryOrder.e";

	string getSign32(string str)
	{
		string ps = "";
		MD5 md5 = MD5.Create();
		byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
		for (int i = 0; i < s.Length; i++)
		{
			ps += s[i].ToString("X2");
		}
		return ps;
	}

	public override void initSingletonMono () {
		DateTime dt = DateTime.Now;
		partner_id	= "1000100020001632";
		app_id		= "3869";
		wap_type	= 1;
		money		= 1;
		out_trade_no = string.Format ("{0:yyyyMMddHHmmssffff}", dt);
		qn					=	"15999969097";
		subject			=	"15999969097";
		key 				=	"65BAD4E7C8C8476DA08275D2BC136217";

//		string context2 = "app_id=" + app_id
//		                 + "&money=" + money
//		                 + "&out_trade_no=" + out_trade_no
//		                 + "&partner_id=" + partner_id
//		                 + "&qn=" + qn
//		                 + "&subject=" + System.Web.HttpUtility.UrlEncode (subject, Encoding.UTF8)
//		                 + "&wap_type=" + wap_type;
//
//		string context1 = "app_id="+app_id
//			+"&money="+money
//			+"&out_trade_no="+out_trade_no
//			+"&partner_id="+partner_id
//			+"&qn="+qn
//			+"&subject="+System.Web.HttpUtility.UrlEncode( subject,Encoding.UTF8)
//			+"&wap_type="+wap_type
//			+"&key="+key;	
//
//		MD5 md5 = MD5.Create ();
//		string sign = getSign32(context1);
//		string url = reqUrl + createOrder + "?" +context2+"&sign="+sign;

//		Application.OpenURL (url);
//		this.reqPay (1, "15521064110", "1");
	}
		
	public void reqPay(int wap_type,string account,string money){

		int iMoney = int.Parse (money) * 100;
		money = iMoney.ToString ();

		subject = account;

		string context = "app_id=" + app_id
			+ "&money=" + money
			+ "&out_trade_no=" + out_trade_no
			+ "&partner_id=" + partner_id
			+ "&qn=" + account
			+ "&subject=" + System.Web.HttpUtility.UrlEncode (subject, Encoding.UTF8)
			+ "&wap_type=" + wap_type;

		string url = reqUrl + createOrder + "?" + context;
		Debug.LogError (context);

		context += "&key=" + key;

		MD5 md5 = MD5.Create ();
		string sign = getSign32(context);

		url += "&sign="+sign;
		Application.OpenURL (url);
	}
		
}
