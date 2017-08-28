using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBush : IslandTerrain 
{
    public override void ApplyEffect(Collision c)
    {
        var rb = c.collider.GetComponent<Rigidbody>();

        Vector3 normToCollider = (rb.gameObject.transform.position - transform.position).normalized;
        rb.AddForce(normToCollider * 174, ForceMode.Impulse);
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
