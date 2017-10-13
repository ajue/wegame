using UnityEngine;
using System.Collections.Generic;


public enum GAME_ID{
	GAME_NONE = 0,
	GAME_DDZ = 1,
	GAME_ZJH = 2,
}

//用户数据
public class Users :SingletonNew<Users>{
	public string Account = "";
	public string Nickname = "";
	public string Alipay = "";
//	public string Phone = "";
	public string Addr = "";
	public float Gold = 0;
	public float BankGold = 0;
	public int Sex = 0;
	public int headID = 1;

	public int cid = 0;
	public int State = 0;
	public int Limitime = 0;

	public GAME_ID GameID = GAME_ID.GAME_NONE;


	//config
	public bool isReadConfig = false;
	public float base_money;
	public float duixian_base;
	public float duixian_base_fee;
	public float duixian_add;
	public float duixian_add_fee;

	public bool use_ui_duihuan;
	public bool use_pay_weixin;
	public bool use_pay_alipay;
}

