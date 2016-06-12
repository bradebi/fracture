using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Music_Box))]
public class MusicBoxEditor : Editor {
    // There is a variable called 'target' that comes from the Editor, its the script we are extending but to
    // make it easy to use we will decalre a new variable called '_target' that will cast this 'target' to our script type
    // otherwise you will need to cast it everytime you use it like this: int i = (ourType)target;
	//int numSongs=0;
	//int numAmb	=0;

	List<AudioClip> MusicList = new List<AudioClip>();

	List<AudioClip> AmbienceList = new List<AudioClip>();
    Music_Box _target;
	[MenuItem("U.T.K./Audio/Music Box")]
	
    void OnEnable()
    {
       _target = (Music_Box)target;
    }
	 
    public override void OnInspectorGUI()
    {
		MusicList=_target.GetMusic();
		AmbienceList=_target.GetAmbience();
		
        GUILayout.BeginVertical();
        GUILayout.Label ("Music Information", EditorStyles.boldLabel);
       
		_target.MusicVolume = EditorGUILayout.Slider("Music Volume", _target.MusicVolume,0f,1f);
			
		if(GUILayout.Button("Add Slot"))
		{
			MusicList.Add(new AudioClip());
		}
		
		for(int i = MusicList.Count - 1; i >= 0; i--) {
    		DisplayMusicClip(i);
		}
		
		GUILayout.Space(10);
		
		GUILayout.Label ("Ambience Information", EditorStyles.boldLabel);
		_target.AmbienceVolume = EditorGUILayout.Slider("Ambience Volume", _target.AmbienceVolume,0f,1f);
			
		if(GUILayout.Button("Add Slot"))
		{
			AmbienceList.Add(new AudioClip());
		}

		for(int i = AmbienceList.Count - 1; i >= 0; i--) {
    		DisplayAmbienceClip(i);
		}
		
       	GUILayout.EndVertical();

      	if(GUI.changed)
       	{
       		EditorUtility.SetDirty(_target);        
       	}
		
		_target.SetMusic(MusicList);
		_target.SetAmbience(AmbienceList);
    }
	
	void DisplayMusicClip(int i)
	{
		GUILayout.BeginHorizontal();
		MusicList[i]=EditorGUILayout.ObjectField(MusicList[i],typeof(AudioClip)) as AudioClip;
		GUI.color=Color.red;
		if(GUILayout.Button("X"))
		{
			MusicList.RemoveAt(i);
		}
		GUI.color=Color.white;
		GUILayout.EndHorizontal();	
	}
	
	void DisplayAmbienceClip(int i)
	{
		GUILayout.BeginHorizontal();
		AmbienceList[i]=EditorGUILayout.ObjectField(AmbienceList[i],typeof(AudioClip)) as AudioClip;
		GUI.color=Color.red;
		if(GUILayout.Button("remove"))
		{
			AmbienceList.RemoveAt(i);
		}
		GUI.color=Color.white;
		GUILayout.EndHorizontal();	
	}
}