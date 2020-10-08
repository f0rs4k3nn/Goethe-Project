using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public float effectsVolume;
	public float musicVolume;

	private Sound currentlyPlayingMusic;
    private Sound currentlyPlayingAmbient;
    private Sound darkAmbient;

	//private SaveData saveData;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		effectsVolume = musicVolume = 0;

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.isMusic;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}
	
	private void Start()
	{
        effectsVolume = musicVolume = 0.1f;
		//SetMusicVolume();
		//SetEffectsVolume();
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogError("Sound: " + name + " not found!");
			return;
		}

		//s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.volume = s.volume * (s.isMusic ? musicVolume : effectsVolume);

		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		if(s.isMusic)
		{
            currentlyPlayingMusic.source.Stop();
			currentlyPlayingMusic = s;
		}
		
		s.source.Play();
	}



	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

    public void PlayAmbient(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }

        //s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.volume = s.volume * (s.isMusic ? musicVolume : effectsVolume);

        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        if(sound.Equals("Dark Ambient"))
        {
            if (darkAmbient != null)
                darkAmbient = s; 
        }

        currentlyPlayingAmbient.source.Stop();
        currentlyPlayingAmbient = s;

        s.source.Play();
    }

    public void SetPitch(float pitch)
    {
        if(currentlyPlayingAmbient != null)
        {
            currentlyPlayingAmbient.source.pitch = pitch;
        }

        if (currentlyPlayingMusic != null)
        {
            currentlyPlayingMusic.source.pitch = pitch;
        }
    }

    public void SetVolume(float vol)
    {
        if (currentlyPlayingAmbient != null)
        {
            currentlyPlayingAmbient.source.volume = vol;
        }

        if (currentlyPlayingMusic != null)
        {
            currentlyPlayingMusic.source.volume = vol;
        }
    }

    public void 

    /*public void SetMusicVolume()
	{

		if(currentlyPlayingMusic != null)
		{
			currentlyPlayingMusic.source.volume = currentlyPlayingMusic.volume * (currentlyPlayingMusic.isMusic ? musicVolume : effectsVolume);
		} else
		{
			Debug.Log("nothing is playing dfq");
		}

		//saveData.musicVolume = musicVolume;
	}
	public void SetEffectsVolume()
	{

		//saveData.effectsVolume = effectsVolume;

		Play("Dunno");
	}*/

}
