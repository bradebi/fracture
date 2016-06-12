using UnityEngine;
using System.Collections;
using System;
using System.IO;

public static class SaveUtility {
	static public string DirectoryName = "/SaveData/";
	static public string fileType = ".dat";
	static private string profileFolder;
	
	public static void SaveToImage(string fileName,Texture2D image)
	{
		Byte [] bytes=image.EncodeToPNG();
    	File.WriteAllBytes(Application.dataPath + "/"+ fileName + ".png",bytes);
		Debug.Log("Saving image to: " + Application.dataPath+fileName+".png");
	}

	public static void SaveToImageAtDirectory(string fileName,Texture2D image)
	{
		Byte [] bytes=image.EncodeToPNG();
		File.WriteAllBytes( fileName + ".png",bytes);
		Debug.Log("Saving image to: "+fileName+".png");
	}
	static string IncrementFilename(string fileName)
	{
		char Delimiter = '_';
		string [] TextLines;
		string Identifier;
		int IntIdentifier;
		
		string [] fileNames=Directory.GetFiles(Application.dataPath + DirectoryName + profileFolder,fileName+"*"+fileType);

		if(fileNames.Length>1)
			fileName=fileNames[1];
		else
			fileName=fileNames[0];
		
		fileName=fileName.Remove(0,fileName.LastIndexOf("/")+1);
		fileName=fileName.Remove(fileName.LastIndexOf("."));
		Debug.Log (fileName);
		
		//If no underscore exists, then it is the original profile name
		if(fileName.IndexOf(Delimiter)<0){
			return fileName+"_1";
		}
		
		TextLines=fileName.Split(Delimiter);
		
		foreach(string lines in TextLines)
			Debug.Log (lines);
		
		//At least one underscore must exist,continue with the value after the underscore
		Identifier=TextLines[TextLines.Length-1];
		
		if(!Int32.TryParse(Identifier,out IntIdentifier))
			return fileName+"_1";
		else{
			string fileNameOut;
			IntIdentifier=1;
			do {
				fileNameOut="";
				for(int i=0;i<TextLines.Length-1;i++)
				{
					fileNameOut+=TextLines[i];
					fileNameOut+=Delimiter;
				}
				
				fileNameOut+=(IntIdentifier).ToString();

				IntIdentifier++;
			}while(File.Exists(Application.dataPath + DirectoryName + profileFolder + fileNameOut + fileType));
			
			return fileNameOut;
		}
	}
}