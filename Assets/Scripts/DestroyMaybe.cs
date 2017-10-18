using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMaybe : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float rotation = Random.Range(0, 360);

        gameObject.transform.rotation = transform.rotation * Quaternion.Euler(0, rotation, 0);

        int chance = Random.Range (0, 2);
		if (chance == 0)
			Destroy (this.gameObject);
	}
}
