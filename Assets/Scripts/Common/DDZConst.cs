using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DDZConst {

	///服务器下发房间时间
	public static int ACTION_ROOM_TIME     = 1;  

	public static int ACTION_ROOM_READY    = 10;//房间准备完成

	public static int ACTION_ROOM_PLAYERS  = 11;//下发玩家信息

	public static int ACTION_ROOM_DISPATCH = 20;//发牌

	public static int ACTION_ROOM_JIAOPAI_NEXT = 29;

	public static int ACTION_ROOM_JIAOPAI = 30;//叫牌


	public static int ACTION_ROOM_STARTGAME = 31;//确定地主，开始游戏

	public static int ACTION_ROOM_CHUPAI = 32;//出牌

	public static int ACTION_ROOM_NEXT = 40;//轮到下一位

	public static int ACTION_ROOM_COMPUTE = 50;//胜利计算

	public static int ACTION_ROOM_KEEPON = 60;//继续游戏

	public static int ACTION_ROOM_UPDATE = 70;//更新房间信息

	public static int ACTION_ROOM_JIABEI = 71;//玩家加倍消息

	public static int ACTION_ROOM_TUOGUAN = 80;//托管

	public static int ACTION_ROOM_RELOGIN = 81; //重连

	public static Dictionary<int,string> DEBUG_ACTION_STRING = new Dictionary<int,string> () {
		{ ACTION_ROOM_TIME,"ACTION_ROOM_TIME" },
		{ ACTION_ROOM_READY,"ACTION_ROOM_READY" },
		{ ACTION_ROOM_PLAYERS,"ACTION_ROOM_PLAYERS" },
		{ ACTION_ROOM_DISPATCH,"ACTION_ROOM_DISPATCH" },
		{ ACTION_ROOM_JIAOPAI_NEXT,"ACTION_ROOM_JIAOPAI_NEXT" },
		{ ACTION_ROOM_JIAOPAI,"ACTION_ROOM_JIAOPAI" },
		{ ACTION_ROOM_STARTGAME,"ACTION_ROOM_STARTGAME" },
		{ ACTION_ROOM_CHUPAI,"ACTION_ROOM_CHUPAI" },
		{ ACTION_ROOM_NEXT,"ACTION_ROOM_NEXT" },
		{ ACTION_ROOM_COMPUTE,"ACTION_ROOM_COMPUTE" },
		{ ACTION_ROOM_KEEPON,"ACTION_ROOM_KEEPON" },
		{ ACTION_ROOM_UPDATE,"ACTION_ROOM_UPDATE" },
		{ ACTION_ROOM_JIABEI,"ACTION_ROOM_JIABEI" },
		{ ACTION_ROOM_TUOGUAN,"ACTION_ROOM_TUOGUAN" },
		{ ACTION_ROOM_RELOGIN,"ACTION_ROOM_RELOGIN" }
	};
	public static bool checkShunzi(int[] data){
		if (data.Length < 5 || data.Length > 12 || (int)data[0]>48) {
			return false;
		} else {
			for(int i =0;i<data.Length-1;i++)
			{
				if (((int)data [i]-1)/4 - 1 != ((int)data [i + 1]-1)/4) {
					return  false;
				} else {

				}
			}
			return true;
		}
	}

	public static string getNameByChairID(int chairID){
		if (chairID == Users.instance.cid) {
			return "myHeadInfo";
		} else if (chairID == Users.instance.cid % 3 + 1) {
			return "nextHeadInfo";
		} else {
			return "backHeadInfo";
		}
	}
}
