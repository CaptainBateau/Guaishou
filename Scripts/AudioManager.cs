using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[HideInInspector]
public abstract class AudioManager : MonoBehaviour
{
	// will need to be used if there is a audio setting pannel

	// public AudioMixerGroup _audioMixer;
	// public GameObject _effectsVolumeSlider;

    public void Play(Sound s)
	{
		if (s == null)
		{
			Debug.LogWarning("Sound: " + s + " not found!");
			return;
		}
		if(s.clips.Count == 0)
        {
			Debug.LogWarning("Sound: " + s + " doesn't have any audioClip");
			return;
		}
		if(s.source == null)
        {
			if (s.soundOrigin == null)
			{
				s.soundOrigin = transform;
			}
			s.source = s.soundOrigin.gameObject.AddComponent<AudioSource>();
		}

		s.source.loop = s.loop;


		if (s.randomPitch)
			s.source.pitch = UnityEngine.Random.Range(s.pitchRange.x, s.pitchRange.y);
		else
			s.source.pitch = 1;

		if (s.clips.Count > 1)
			s.source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Count - 1)];
		else
			s.source.clip = s.clips[0];
		s.source.Play();
	}
	public void Stop(Sound s)
    {
		s.source.Stop();
    }
}
