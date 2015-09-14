﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
	public float mastervolume;
	public bool mute;

	private List<AudioSourceWrapper> _audioSourceList;

	// Use this for initialization
	void Awake ()
	{
		this.enabled = false;
		_audioSourceList = new List<AudioSourceWrapper>();
	}
	
	public void AddAudioSource(string inClipName, AudioID inAID, GameObject inGameObj)
	{
		if (inGameObj == null)
			inGameObj = GameObject.Find ("MCP");
		
		AudioSource audioSrc = inGameObj.AddComponent<AudioSource> ();
		audioSrc.clip = Resources.Load ("Audio/" + inClipName) as AudioClip;
		audioSrc.playOnAwake = false;
		
		this._audioSourceList.Add (new AudioSourceWrapper(inGameObj, audioSrc, inAID));
	}
	
	public void AttachAudioSource( AudioSource inAudioSrc, GameObject inGameObj, string inName)
	{
		AudioID  aID = this.getIDByName(inName);
		this._audioSourceList.Add (new AudioSourceWrapper(inGameObj, inAudioSrc, aID));
	}
	
	//I need to test this one!!
	public AudioID getIDByName(string inName)
	{
		return ((AudioID)System.Enum.Parse(typeof(AudioID), inName));
	}
	
	public void Initialize()
	{
		
	}
	
	public void Update()
	{
		
	}
	
	// Maybe implement a different Data Structure in the future for largers sets of data Hashtable or Dictionary. 
	public void PlayAudio(AudioID inAID)
	{
		for (int i = 0; i< this._audioSourceList.Count; i++)
		{
			if (this._audioSourceList[i].getAID().Equals(inAID))
			{
				this._audioSourceList[i].playClip();
				break;
			}
		}
	}
}
public enum AudioID
{
	Fire,
	BGM,
	Dialogue_1,
	Dialogue_2
}

public class AudioSourceWrapper
{
	private GameObject _gameobj;
	private AudioSource _audiosrc;
	private AudioID _aID;
	
	private AudioSourceWrapper(){}
	
	public AudioSourceWrapper(GameObject inGameObj, AudioSource inAudioSrc, AudioID inAID)
	{	
		this._gameobj = inGameObj;
		this._audiosrc = inAudioSrc;
		this._aID = inAID;
	}
	
	public AudioID getAID()
	{
		return this._aID;
	}
	
	public void playClip()
	{
		//not sure if this affects the 3Dness of audio
		//second call seems better to me.
		//this._audiosrc.Play();
		this._gameobj.GetComponent<AudioSource>().Play();
	}
	
	
}