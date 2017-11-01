using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBush : IslandTerrain 
{
    public static float Force = 440f;
    public static float Damage = 10f;


    void OnCollisionEnter(Collision c)
    {
        var player = c.gameObject.GetComponent<Player>();

        if (player != null)
            player.HitMe(Force, transform.position, Damage);
    }

    //public void Hit(Player target)
    //{
    //    float Multiplier = target.Health;
    //    Multiplier = (Multiplier / 100f) + 1;
    //    Multiplier = Multiplier * Multiplier; //Square for pistons (maybe too much);
    //    target.Health += 10;
    //    target.GetComponent<Rigidbody>().AddExplosionForce(330 * Multiplier, this.gameObject.transform.position, 1);
}
