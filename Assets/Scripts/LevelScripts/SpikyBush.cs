using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBush : IslandTerrain 
{
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.GetComponent<Player>())
        {
            Hit(c.gameObject.GetComponent<Player>());
        }
    }

    public void Hit(Player target)
    {
        float Multiplier = target.Health;
        Multiplier = (Multiplier / 100f) + 1;
        Multiplier = Multiplier * Multiplier; //Square for pistons (maybe too much);
        target.Health += 10;
        target.GetComponent<Rigidbody>().AddExplosionForce(50 * Multiplier, this.gameObject.transform.position, 1);
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
