using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioController : MonoBehaviour
{
	public static AudioController Instance = null;
	private Dictionary<string, int> AudioDictionary = new Dictionary<string , int>();

	private const int MaxAudioCount = 10;
	private const string ResourcePath = "Audio/";
	private const string StreamingAssetsPath = "";
	private AudioSource BGMAudioSource;
	private AudioSource LastAudioSource;

	private float bgmVolume = 1;
	private float clipVolume = 1;

	#region Mono Function
	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
			int sound = PlayerPrefs.GetInt ("Sound1");
			int music = PlayerPrefs.GetInt ("Music");
			if (sound == -1) {
				clipVolume = 0;
			} else if(sound == 1){
				clipVolume = 1;
			}
			if (music == -1) {
				bgmVolume = 0;
			} else if(music == 1){
				bgmVolume = 1;
			}
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			if(Instance != this)
			{
				Destroy(transform.gameObject);
			}
		}
	}

	#endregion

	/// <summary>
	/// 播放
	/// </summary>
	/// <param name="audioname"></param>
	public void SoundZJHPlay(string audioname,int sexID)
	{
		string sounSexFilName = "zjh2/male/";
		if (sexID == 1) {
			sounSexFilName = "zjh2/male/";
		} else if (sexID == 2) {
			sounSexFilName = "zjh2/female/";
		} else {
			sounSexFilName = "zjh2/";
		}
		SoundPlay (sounSexFilName+audioname);
	}

	/// <summary>
	/// 播放
	/// </summary>
	/// <param name="audioname"></param>
	public void SoundDDZPlay(string audioname,int sexID)
	{
		string sounSexFilName = "ddz/Man/Man_";
		if (sexID == 1) {
			sounSexFilName = "ddz/Man/Man_";
		} else if (sexID == 2) {
			sounSexFilName = "ddz/Woman/Woman_";
		} else {
			sounSexFilName = "ddz/";
		}
		SoundPlay (sounSexFilName+audioname);
	}


	/// <summary>
	/// 播放
	/// </summary>
	/// <param name="audioname"></param>
	public void SoundPlay(string audioname)
	{
		if (AudioDictionary.ContainsKey(audioname))
		{
			if (AudioDictionary[audioname] <= MaxAudioCount)
			{
				AudioClip sound = this.GetAudioClip(audioname);
				if (sound != null)
				{
					StartCoroutine(this.PlayClipEnd(sound, audioname));
					this.PlayClip(sound);
					AudioDictionary[audioname]++;
				}
			}
		}
		else
		{
			AudioDictionary.Add(audioname, 1);
			AudioClip sound = this.GetAudioClip(audioname);
			if (sound != null)
			{
				StartCoroutine(this.PlayClipEnd(sound, audioname));
				this.PlayClip(sound);
				AudioDictionary[audioname]++;
			}
		}
	}

	/// <summary>
	/// 暂停
	/// </summary>
	/// <param name="audioname"></param>
	public void SoundPause(string audioname) 
	{
		if (this.LastAudioSource!=null)
		{
			this.LastAudioSource.Pause();
		}
	}

	/// <summary>
	/// 暂停所有音效音乐
	/// </summary>
	public void SoundAllPause() 
	{
		AudioSource[] allsource = FindObjectsOfType<AudioSource>();
		if (allsource!=null&&allsource.Length>0)
		{
			for (int i = 0; i < allsource.Length; i++)
			{
				allsource[i].Pause();
			}
		}
	}

	/// <summary>
	/// 停止特定的音效
	/// </summary>
	/// <param name="audioname"></param>
	public void SoundStop(string audioname) 
	{
		GameObject obj=  this.transform.FindChild("audioname").gameObject;
		if (obj!=null)
		{
			Destroy(obj);
		}
	}

	/// <summary>
	/// 设置音量
	/// </summary>
	public void BGMSetVolume(float volume) 
	{
		this.bgmVolume = volume;
		if (this.BGMAudioSource!=null)
		{
			this.BGMAudioSource.volume = volume;
		}
	}
	public void ClipSetVolume(float volume){
		this.clipVolume = volume;
	}

	/// <summary>
	/// 播放背景音乐
	/// </summary>
	/// <param name="bgmname"></param>
	/// <param name="volume"></param>
	public void BGMPlay(string bgmname)
	{
		BGMStop();

		if (bgmname!=null)
		{
			AudioClip bgmsound = this.GetAudioClip(bgmname);
			if (bgmsound!=null)
			{
				this.PlayLoopBGMAudioClip(bgmsound);
			}
		}
	}

	/// <summary>
	/// 暂停背景音乐
	/// </summary>
	public void BGMPause() 
	{
		if (this.BGMAudioSource!=null)
		{
			this.BGMAudioSource.Pause();
		}
	}

	/// <summary>
	/// 停止背景音乐
	/// </summary>
	public void BGMStop()
	{
		if (this.BGMAudioSource != null && this.BGMAudioSource.gameObject)
		{
			Destroy(this.BGMAudioSource.gameObject);
			this.BGMAudioSource = null;
		}  
	}

	/// <summary>
	/// 重新播放
	/// </summary>
	public void BGMReplay() 
	{
		if (this.BGMAudioSource!=null)
		{
			this.BGMAudioSource.Play();
		}
	}

	#region 音效资源路径

	enum eResType {
		AB=0,
		CLIP=1
	}

	/// <summary>
	/// 下载音效
	/// </summary>
	/// <param name="aduioname"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	private AudioClip GetAudioClip(string aduioname, eResType type = eResType.CLIP) 
	{
		AudioClip audioclip = null;
		switch (type)
		{
		case eResType.AB:
			break;
		case eResType.CLIP:
			audioclip = GetResAudioClip(aduioname);
			break;
		default:
			break;
		}        
		return audioclip;
	}

	private IEnumerator GetAbAudioClip(string aduioname) 
	{
		yield  return null;
	}

	private AudioClip GetResAudioClip(string aduioname) 
	{
		return Resources.Load(ResourcePath + aduioname) as AudioClip;
	}
	#endregion

	#region 背景音乐
	/// <summary>
	/// 背景音乐控制器
	/// </summary>
	/// <param name="audioClip"></param>
	/// <param name="volume"></param>
	/// <param name="isloop"></param>
	/// <param name="name"></param>
	private void PlayBGMAudioClip(AudioClip audioClip,bool isloop=false,string name=null) 
	{
		if (audioClip==null)
		{
			return;
		}
		else
		{
			GameObject obj = new GameObject(name);
			obj.transform.parent = this.transform;
			AudioSource LoopClip = obj.AddComponent<AudioSource>();
			LoopClip.clip = audioClip;
			LoopClip.volume = bgmVolume;
			LoopClip.loop = true;
			LoopClip.pitch = 1f;
			LoopClip.Play();
			this.BGMAudioSource = LoopClip;
		}
	}

	/// <summary>
	/// 播放一次的背景音乐
	/// </summary>
	/// <param name="audioClip"></param>
	/// <param name="volume"></param>
	/// <param name="name"></param>
	private void PlayOnceBGMAudioClip(AudioClip audioClip,string name = null) 
	{
		PlayBGMAudioClip(audioClip, false, name == null ? "BGMSound" : name);
	}

	/// <summary>
	/// 循环播放的背景音乐
	/// </summary>
	/// <param name="audioClip"></param>
	/// <param name="volume"></param>
	/// <param name="name"></param>
	private void PlayLoopBGMAudioClip(AudioClip audioClip, string name = null) 
	{
		PlayBGMAudioClip(audioClip, true, name==null?"LoopSound":name);
	}

	#endregion

	#region  音效

	/// <summary>
	/// 播放音效
	/// </summary>
	/// <param name="audioClip"></param>
	/// <param name="volume"></param>
	/// <param name="name"></param>
	private void PlayClip(AudioClip audioClip,string name=null)
	{
		if (audioClip==null)
		{
			return;
		}
		else
		{
			GameObject obj = new GameObject(name == null ? "SoundClip" : name);
			obj.transform.parent = this.transform;
			AudioSource source = obj.AddComponent<AudioSource>();
			StartCoroutine(this.PlayClipEndDestroy(audioClip,obj));
			source.pitch = 1f;
			source.volume = clipVolume;
			source.clip = audioClip;
			source.Play();
			this.LastAudioSource = source;
		}
	}

	/// <summary>
	/// 播放玩音效删除物体
	/// </summary>
	/// <param name="audioclip"></param>
	/// <param name="soundobj"></param>
	/// <returns></returns>
	private IEnumerator PlayClipEndDestroy(AudioClip audioclip, GameObject soundobj)
	{
		if (soundobj == null||audioclip==null)
		{
			yield break;
		}
		else
		{
			yield return new WaitForSeconds (audioclip.length * Time.timeScale);
			Destroy(soundobj);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	private IEnumerator PlayClipEnd(AudioClip audioclip,string audioname) 
	{
		if (audioclip!=null)
		{
			yield return new WaitForSeconds(audioclip.length * Time.timeScale);
			AudioDictionary[audioname]--;
			if (AudioDictionary[audioname]<=0)
			{
				AudioDictionary.Remove(audioname);
			}
		}
		yield break;
	}
	#endregion
}
