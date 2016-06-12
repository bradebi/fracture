using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public static class LoadUtility {
	static public string DirectoryName = "/SaveData/";
	static public string fileType = ".dat";
	
	//Get list of save games 
	public static string [] GetLoadFileListDir(string profileName)
	{
		if(!Directory.Exists(Application.dataPath + DirectoryName + profileName + "/"))
		{
			Directory.CreateDirectory(Application.dataPath + DirectoryName + profileName + "/");
		}

		string [] fileNames=Directory.GetFiles(Application.dataPath + DirectoryName + profileName + "/",profileName+"*"+fileType);
		
		return fileNames;
	}
	
	public static string [] GetLoadFileList(string profileName)
	{
		string [] fileNames=GetLoadFileListDir(profileName);
		List<int> fileInts = new List<int>();
		for(int i=0;i<fileNames.Length;i++)
		{
			fileNames[i]=fileNames[i].Remove(0,fileNames[i].LastIndexOf('/')+1);
			fileNames[i]=fileNames[i].Remove(fileNames[i].LastIndexOf('.'));
			fileNames[i]=fileNames[i].Remove(0,fileNames[i].LastIndexOf('_')+1);
			fileInts.Add(Int32.Parse(fileNames[i]));
		}
		fileInts.Sort();
		
		for(int i=0;i<fileNames.Length;i++)
		{
			fileNames[i]=profileName+"_"+fileInts[i];
		}
		
		return fileNames;
	}
	
	public static string [] GetProfileNames()
	{
		string [] Profiles=Directory.GetDirectories(Application.dataPath + DirectoryName);
		
		for(int i=0; i<Profiles.Length; i++){Profiles[i]=Profiles[i].Remove(0,Profiles[i].LastIndexOf("/")+1);}
		
		return Profiles;
	}
	
	public static void setLoadDirectory(string directory)
	{
		DirectoryName="/"+directory+"/";
	}
	
	public static Texture LoadImage(string fileName)
	{
    	return LoadImageFrom(Application.dataPath + DirectoryName + fileName + ".png");
	}
	
	public static Texture LoadImageFrom(string Directory)
	{
		WWW load = new WWW("file://" + Directory);
		while(!load.isDone){}

        if (!string.IsNullOrEmpty(load.error)) {
        	Debug.Log(load.error);
			return default(Texture);
        }
		else{
            return load.texture;
        }
	}

	public static int GetFileCount(string DirectoryName,string search)
	{
		if(!Directory.Exists(Application.dataPath + DirectoryName))
		{
			Directory.CreateDirectory(Application.dataPath + DirectoryName);

			return 0;
		}
		string [] fileNames=Directory.GetFiles(Application.dataPath + DirectoryName,search+"*");
			return fileNames.Length;
	}
	//LoadFrom
}
