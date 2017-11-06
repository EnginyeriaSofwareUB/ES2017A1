using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource effectSource;
	public AudioSource musicSource;

	public static SoundManager instance = null;

	public float lowPicthRange = .95f;
	public float highPicthRange = 1.05f;

	// Use this for initialization
	void Awake () {//Singleton Method
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	public void PlayEffect(AudioClip clip, bool loop = false)
	{
		effectSource.clip = clip;
		effectSource.loop = loop;
		effectSource.Play();
	}

	public void PlayEffect(string resourcePath, bool loop = false)
	{
		AudioClip effect = Resources.Load(resourcePath) as AudioClip;
		PlayEffect(effect, loop);
	}

	public void StopEffect()
	{
		if (effectSource.isPlaying)
		{
			effectSource.Stop();
		}
	}

	public void RandomizeSfx (params AudioClip [] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPicthRange, highPicthRange);
		effectSource.pitch = randomPitch;
		effectSource.clip = clips[randomIndex];
		effectSource.Play();
	}

	public void SetMusicVolume(float value)
	{
		Debug.Log(value);
		this.musicSource.volume = value;
	}

	public void SetMasterVolume(float value)
	{
		Debug.Log(value);
		this.musicSource.volume = value;
		this.effectSource.volume = value;
	}

}
