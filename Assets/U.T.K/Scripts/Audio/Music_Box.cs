using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Music_Box: MonoBehaviour {
	GameObject Player;
	vp_AudioManager AudioManager;
	public List<AudioClip> Music = new List<AudioClip>();
	public List<AudioClip> Ambience = new List<AudioClip>();
	
	public float MusicVolume = 1;
	public float AmbienceVolume = 1;
	
	// Use this for initialization
	void Update () {
		if(Player==null){
			Player=GameObject.Find ("Base_Player");
			if(Player!=null){
				AudioManager=Player.GetComponentInChildren(typeof(vp_AudioManager)) as vp_AudioManager;
				AudioManager.ClearMusic();
				AudioManager.ClearAmb();
				foreach(AudioClip clip in Ambience)
				{
					AudioManager.PlaySound(clip,true);
				}
				
				foreach(AudioClip clip in Music)
				{
					AudioManager.QueueMusic(clip);
				}
				
				AudioManager.setMusicVolume(MusicVolume);
				AudioManager.setAmbienceVolume(AmbienceVolume);
			}
		}
	}
	
	// Update is called once per frame
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (transform.position, "music-icon.png");
	}
	
	public void SetMusic(List<AudioClip> inClips)
	{
		Music=inClips;
	}
	
	public void SetAmbience(List<AudioClip> inClips)
	{
		Ambience=inClips;
	}
	
	public List<AudioClip> GetMusic()
	{
		return Music;
	}
	
	public List<AudioClip> GetAmbience()
	{
		return Ambience;
	}
}
