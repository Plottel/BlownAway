using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaBolt : MonoBehaviour
{
    public static float Force = 560f;
    public static float Damage = 50f;

	// Use this for initialization
	void Start ()
    {
        // Destroy self after 3 seconds.
        Destroy(gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision c)
    {
        var player = c.gameObject.GetComponent<Player>();

        if (player != null)
            player.HitMe(Force, transform.position, Damage);
    }
}
