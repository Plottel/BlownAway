using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasSoundFX : MonoBehaviour {

	public AudioClip soundFX;
	private AudioSource sourceSound;
	private float lowVol = 0.4f;
	private float highVol = 1.0f;

	void Awake()
	{
		sourceSound = gameObject.GetComponent<AudioSource> ();
		var volume = Random.Range (lowVol, highVol);
		sourceSound.PlayOneShot (soundFX, volume);
	}

}
