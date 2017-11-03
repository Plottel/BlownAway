using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggShell : HasSoundFX {

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 2f);
	}
}
