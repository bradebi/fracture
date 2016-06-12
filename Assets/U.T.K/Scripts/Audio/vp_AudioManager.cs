using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vp_AudioManager : MonoBehaviour {
	
	public List<AudioSource> AudioSlot=new List<AudioSource>();
	public List<AudioClip> Music=new List<AudioClip>();
	private AudioSource MusicSource;
	private int PlaylistIndex=0;
	public int Voices=100;
	public float musicVolume=0.5f;
	public float ambVolume=0.5f;
	public float fadeTime=2f;
	// Update is called once per frame
	void Update () {
		if(AudioSlot.Count>0){
			PruneAudioSource();
			fadeDestroy();
		}
		
		if(Music.Count>0)
		{
			ManageMusic();	
		}
	}
	
	void ManageMusic()
	{
		if(Music.Count>0)
		{
			if(MusicSource==null)
			{
				PlaySound (Music[PlaylistIndex]);
				MusicSource=AudioSlot[AudioSlot.Count-1];
				PlaylistIndex++;
			}
		}
		
		if(PlaylistIndex>=Music.Count)
			PlaylistIndex=0;
		
		MusicSource.volume=musicVolume;
	}
	
	void PruneAudioSource()
	{
		foreach(AudioSource slot in AudioSlot)
		{
			if(!slot.isPlaying)
			{
				AudioSlot.Remove(slot);
				deadSource.Add(slot);
				timeLeft.Add (fadeTime);
				
			}
			if(slot!=MusicSource)
				slot.volume=ambVolume;
		}
	}
	
	public void PlaySound(AudioClip sound, bool loop)
	{
		if(AudioSlot.Count<Voices){
			AudioSlot.Add (gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
			AudioSlot[AudioSlot.Count-1].clip=sound;
			AudioSlot[AudioSlot.Count-1].Play();
			AudioSlot[AudioSlot.Count-1].loop=loop;

		}
	}
	
	public void PlaySound(AudioClip sound)
	{
		if(AudioSlot.Count<Voices){
			AudioSlot.Add (gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
			AudioSlot[AudioSlot.Count-1].clip=sound;
			AudioSlot[AudioSlot.Count-1].Play();
		}
	}
	
	public void QueueMusic(AudioClip music)
	{
		Music.Add (music);
	}
	
	public void StopMusic()
	{
		if(MusicSource!=null)
		MusicSource.Stop();
	}
	
	public void StopClip(AudioClip clip)
	{
		
	}
	
	public void ClearMusic()
	{
		Music.Clear ();
		if(MusicSource!=null)
			Destroy(MusicSource);
	}

	public void ClearAmb()
	{
		for(int i=0;i<AudioSlot.Count;i++)
		{
			Destroy(AudioSlot[i]);
		}
		AudioSlot.Clear();
	}
	
	public bool isPlaying(AudioClip clip)
	{
		
		foreach(AudioSource slot in AudioSlot)
		{
			if(slot.clip==clip)
				return true;
			
		}
		
		return false;
	}
	
	public void setAmbienceVolume(float inVol)
	{
		this.ambVolume=inVol;
	}
	
	public void setMusicVolume(float inVol)
	{
		this.ambVolume=inVol;
	}
	
	List<float> timeLeft = new List<float>();
	List<AudioSource> deadSource = new List<AudioSource>();
	public void fadeDestroy()
	{
		for(int i=0;i<timeLeft.Count;i++)
			timeLeft[i]-=Time.deltaTime;
		
		AudioSource tempSource;
		for(int i=0;i<deadSource.Count;i++)
		{	
			tempSource=deadSource[i];
			if(timeLeft[i]<=0)
			{
				deadSource.RemoveAt(i);
				timeLeft.RemoveAt(i);
				Destroy(tempSource);
			}
		}
	}
}
