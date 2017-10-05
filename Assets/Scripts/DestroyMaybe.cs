using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMaybe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int chance = Random.Range (0, 2);
		if (chance == 0)
			Destroy (this.gameObject);
	}
}
