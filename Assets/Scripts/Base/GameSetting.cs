using UnityEngine;
using System.Collections;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

public class GameSetting : SingletonNew< GameSetting > 
{
	
	public static string	IconPath = "Textures/Icon/";
	public static string	TexturesPath = "Textures/";
	public static string	UIAtlasPath = "UIAtlas/";
	public static string	UIPath = "Prefabs/";
	public static string	UIItemPath = "Prefabs/Item/";
	public static string	UIItemPrefabsPath = "Prefabs/Item/Prefab/";
	public static string	MaterialsPath = "Prefabs/Character/Object/Materials/";
	public static string	CreaterPath = "Prefabs/Character/";
	public static string	PetPath = "Prefabs/Character/Prefab/";
	public static string	MonsterPath = "Prefabs/Monster/";
	public static string	EffectsPath = "Prefabs/Effects/";
	public static string    UIEffectsPath = "Prefabs/UIEffects/";
	public static string	ItemPath = "Prefabs/Effects/";
	public static string	AudioPath = "Audio/";
	
	public static string	StreamingAssetsPath;
	
	public static float		GameSpeed = 1.0f;
	public static float		GameScale = 1.0f;	
	
}

