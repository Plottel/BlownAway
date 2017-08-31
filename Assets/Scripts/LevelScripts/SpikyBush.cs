using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBush : IslandTerrain 
{
    public override void ApplyEffect(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            Debug.Log("IM EXPLODIN HERE");
            Debug.Log(c.gameObject);
            var rb = c.collider.GetComponent<Rigidbody>();
            rb.AddExplosionForce(400, this.gameObject.transform.position, 100);
        }
    }

    // Use this for initialization
    void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
