using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaBolt : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        // Destroy self after 3 seconds.
        Destroy(gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.GetComponent<Player>())
        {
            col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(300f, gameObject.transform.position, 5);
        }
    }
}
