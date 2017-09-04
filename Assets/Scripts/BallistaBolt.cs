using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaBolt : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        // Destroy self after 5 seconds.
        Destroy(gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
