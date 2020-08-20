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

    List<Sound> bannedSounds;

    public void Play(Sound s)
    {
        if (!bannedSounds.Contains(s))
        {
            bannedSounds.Add(s);
            StartCoroutine(RemoveSound(s));
            if (s == null)
            {
                Debug.LogWarning("Sound: " + s.ToString() + " not found!");
                return;
            }
            if (s.clips.Count == 0)
            {
                Debug.LogWarning("Sound: " + s.ToString() + " doesn't have any audioClip");
                return;
            }
            if (s.source == null)
            {
                if (s.soundOrigin == null)
                {
                    s.soundOrigin = transform;
                }
                s.source = s.soundOrigin.gameObject.AddComponent<AudioSource>();
            }
            if (s.source.isPlaying)
            {
                AudioSource[] sources = s.soundOrigin.GetComponents<AudioSource>();
                bool availableSource = false;
                foreach (AudioSource source in sources)
                {
                    if (!source.isPlaying)
                    {
                        availableSource = true;
                        s.source = source;
                        break;
                    }
                }
                if (!availableSource)
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
    }
    IEnumerator RemoveSound(Sound s)
    {
        yield return new WaitForSeconds(s.minDelayBetweenSounds);
        bannedSounds.Remove(s);
    }
    public void Stop(Sound s)
    {
        // will need to do something if there is multiple source
        s.source.Stop();
    }
}
