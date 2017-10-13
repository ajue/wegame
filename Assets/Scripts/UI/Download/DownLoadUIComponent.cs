using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class DownloadUIComponent : MonoBehaviour {

	string url = "http://219.135.224.80:8300/test3/";
	string updateInfo = "update.json";
	string packageName = "wegame.apk";

	UILabel contentLab;
	UISlider progress;

	void Start(){
		contentLab = transform.FindChild ("content").GetComponent<UILabel> ();
		progress = transform.FindChild ("ProgressBar").GetComponent<UISlider> ();
		progress.enabled = false;

		Invoke ("delayInit", 1);
	}
	public void delayInit(){
		//延迟初始化，防止画面过快关闭，误造bug效果
		string file = url + updateInfo;
		StartCoroutine (UpdateVersion (file));
	}
	IEnumerator UpdateVersion(string url){
		contentLab.text = "正在检测版本...";
		WWW www = new WWW(url);
		yield return www;

		string net = www.text;
		Debug.Log (net);
		JsonData netJson = JsonMapper.ToObject (net);
		string netVersion = netJson["version"].ToString ();
		//比较版本号
		if (Application.version.Equals (netVersion)) {
			contentLab.text = "当前为最新版本 v"+Application.version;
			DownloadUIHandler.instance.UnShow ();
			DownloadUIHandler.instance.Release ();
			LoginUIHandler.instance.Show ();
		}
		else {

			#if UNITY_ANDROID
				//www.Dispose();
				//this.packageName = netJson ["packageName"].ToString ();
				//this.executeDownload ();
				Application.OpenURL ("http://fir.im/6y8p");
				Debug.Log("安卓>_<");
			#endif
			#if UNITY_IPHONE
				Application.OpenURL ("http://fir.im/fpuk");
				Debug.Log("这里是苹果设备>_<");
			#endif
		}
	}
	public void executeDownload(){
		//下载更新包
		string file = url + packageName;
		StartCoroutine(DownloadFile (file));
		progress.enabled = true;
	}
	//下载并写入文件
	IEnumerator DownloadFile(string url)
	{
		WWW www = new WWW(url);
		contentLab.text = "正在下载...";
		while (!www.isDone)
		{
			if (progress != null) {
				progress.value = www.progress;
			}
			yield return null;
		}
		if (!string.IsNullOrEmpty(www.error))
		{
			if (contentLab != null) {
				contentLab.color = Color.red;
				contentLab.text = www.error;
			}
			Debug.LogError("WWW DownloadFile:" + www.error);
			yield break;
		}
		contentLab.text = "下载完成，等待安装...";
		//先检测删除同名文件
		DeleteFile (Application.persistentDataPath, packageName);
		//下载完成即写入本地
		CreateModelFile(Application.persistentDataPath,packageName, www.bytes,www.bytes.Length);
		www.Dispose();
		
		InstallAPK (Application.persistentDataPath+"//"+packageName,false);
	}

	void CreateModelFile(string path, string name, byte[] info, int length)
	{
		Stream sw;
		FileInfo t = new FileInfo(path + "//" + name);
		if (!t.Exists) {
			sw = t.Create ();
		} else {
			return;
		}
		sw.Write(info, 0, length);
		sw.Close();
		sw.Dispose();
	}

	/**
   * path：文件创建目录
   * name：文件的名称
   *  info：写入的内容
   */
	void CreateFile(string path,string name,string info)
	{
		//文件流信息
		StreamWriter sw;
		FileInfo t = new FileInfo(path+"//"+ name);
		if(!t.Exists)
		{
			sw = t.CreateText();
		}
		else
		{
			sw = t.AppendText();
		}
		//以行的形式写入信息
		sw.WriteLine(info);
		//关闭流
		sw.Close();
		//销毁流
		sw.Dispose();
	}
		
	/**
   * 读取文本文件
   * path：读取文件的路径
   * name：读取文件的名称
   */
	string LoadFile(string path,string name)
	{
		StreamReader sr = null;
		try{
			sr = File.OpenText(path+"//"+ name);
		}
		catch(System.Exception e)
		{
			return "";
		}

		string buf = "";
		string line;
		while ((line = sr.ReadLine()) != null)
		{
			buf += line;
		}
		sr.Close();
		sr.Dispose();

		return buf;
	}  
	/**
   * path：删除文件的路径
   * name：删除文件的名称
   */
	void DeleteFile(string path,string name)
	{
		File.Delete(path+"//"+ name);
	}

	//bReTry表示第一次安装不成功的时候，再试一次下面的方式
	public bool InstallAPK(string path, bool bReTry)
	{
		try
		{
			var Intent = new AndroidJavaClass("android.content.Intent");
			var ACTION_VIEW = Intent.GetStatic<string>("ACTION_VIEW");
			var FLAG_ACTIVITY_NEW_TASK = Intent.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
			var intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

//			var file = new AndroidJavaObject("java.io.File", path);
//			var Uri = new AndroidJavaClass("android.net.Uri");

			if (!bReTry)
			{
				intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
				intent.Call<AndroidJavaObject>("setClassName", "com.android.packageinstaller", "com.android.packageinstaller.PackageInstallerActivity");
			}

			var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			currentActivity.Call("startActivity", intent);

			return true;
		}
		catch (System.Exception e)
		{
			Debug.Log (e);
			if (!bReTry) {
				InstallAPK (path, true);
			} else {
				contentLab.text = "安装失败。请尝试在该路径下手动安装："+path;
			}
			return false;
		}
	}

}
