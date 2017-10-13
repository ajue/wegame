using UnityEngine;
using System.Collections.Generic;

public class ZJHConst{
	///服务器下发房间时间
	public static int ACTION_ROOM_TIME     = 1;  
	///准备好了
	public static int ACTION_ROOM_READY    = 10;   
	///开始游戏
	public static int ACTION_ROOM_START    = 11;   
	///发牌
	public static int ACTION_ROOM_DISPATCH = 12;

	public static int ACTION_ROOM_GENZHU   = 13;

	public static int ACTION_ROOM_JIAZHU   = 14;

	public static int ACTION_ROOM_KANPAI   = 15;

	public static int ACTION_ROOM_QIPAI    = 16;

	public static int ACTION_ROOM_BIPAI_START  = 171;
	public static int ACTION_ROOM_BIPAI_END	   = 17;

	public static int ACTION_ROOM_NEXT     = 18;

	public static int ACTION_ROOM_SETTLE   = 19;

	public static int ACTION_ROOM_PUBLICH  = 20;

	public static int ACTION_ROOM_CLEARGAME 		= 21;
	public static int ACTION_ROOM_KAIPAI_BEGIN      = 22;
	public static int ACTION_ROOM_KAIPAI_END        = 23;

	public static int ACTION_INFO_UPDATE = 100; 

	//玩家状态
	public static int PLAYER_STATE_GARK  = 0;
	public static int PLAYER_STATE_READY = 1;
	public static int PLAYER_STATE_QIPAI = 2;
	public static int PLAYER_STATE_LOSER = 3;

	public static int PLAYER_STATE_LINE = 10;

	public static int PLAYER_STATE_START  = 11;
	public static int PLAYER_STATE_KANPAI = 12;
	public static int PLAYER_STATE_WIN	  = 13;

	public static Dictionary<int,string> ACTION_STRING = new Dictionary<int,string> () {
		{ ACTION_ROOM_TIME,"ACTION_ROOM_TIME" },
		{ ACTION_ROOM_READY,"ACTION_ROOM_READY" },
		{ ACTION_ROOM_START,"ACTION_ROOM_START" },
		{ ACTION_ROOM_DISPATCH,"ACTION_ROOM_DISPATCH" },
		{ ACTION_ROOM_GENZHU,"ACTION_ROOM_GENZHU" },
		{ ACTION_ROOM_JIAZHU,"ACTION_ROOM_JIAZHU" },
		{ ACTION_ROOM_KANPAI,"ACTION_ROOM_KANPAI" },
		{ ACTION_ROOM_QIPAI,"ACTION_ROOM_QIPAI" },
		{ ACTION_ROOM_BIPAI_START,"ACTION_ROOM_BIPAI_START" },
		{ ACTION_ROOM_BIPAI_END,"ACTION_ROOM_BIPAI" },
		{ ACTION_ROOM_NEXT,"ACTION_ROOM_NEXT" },
		{ ACTION_ROOM_SETTLE,"ACTION_ROOM_SETTLE" },
		{ ACTION_ROOM_PUBLICH,"ACTION_ROOM_PUBLICH" },
		{ ACTION_ROOM_CLEARGAME,"ACTION_ROOM_CLEARGAME" },
		{ ACTION_INFO_UPDATE,"ACTION_INFO_UPDATE" },
		{ ACTION_ROOM_KAIPAI_BEGIN,"ACTION_ROOM_KAIPAI_BEGIN" },
		{ ACTION_ROOM_KAIPAI_END,"ACTION_ROOM_KAIPAI_END" }
	};
}
