﻿using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
	public List<AudioClip> clips;

	[Range(0f,1f)]
	public float volume = 1;

	public bool randomPitch;
	public Vector2 pitchRange = new Vector2(0.9f, 1.1f);

	public bool loop;

	[Header("Optional")]
	public float minDelayBetweenSounds = 0;
	public bool spatialisation;
	public float spatialisationRange;
	public Transform soundOrigin;
	[HideInInspector] public AudioSource source;
}
