using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	static MusicPlayer instance = null;
	public string Music;

	void Awake()
	{
		//Debug.Log ("Music player Awake " + GetInstanceID ());
		if (instance != null) 
		{
			if (instance.Music == this.Music) 
			{
				Destroy (gameObject);
				//Debug.Log ("Duplicate MusicPlayer destroyed.");
			} 
			else 
			{
				Destroy (instance.gameObject);
				instance = this;
				GameObject.DontDestroyOnLoad (gameObject);
			}
		} 
		else 
		{
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}

	// Use this for initialization
	void Start () 
	{
		//Debug.Log ("Music Player Start " + GetInstanceID ());
	}

	// Update is called once per frame
	void Update () {

	}
}
