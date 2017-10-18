using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : IslandTerrain {

	public int ExplosionForce = 400;
	public int ExplosionRadius = 100;

    //TODO: This may be super whack.
    public static float Force = 10f;
    public static float Damage = 0f;


	//public void Hit(Player target)
	//{
	//	float Multiplier = target.Health;
	//	Multiplier = (Multiplier / 100f) + 1;
	//	//target.Health += Damage;
	//	float DistMulti = Vector3.Distance(target.transform.position, transform.position) * 4;
	//	target.GetComponent<Rigidbody>().AddExplosionForce ((ExplosionForce * Multiplier * Time.deltaTime * 30) / DistMulti, this.gameObject.transform.position, ExplosionRadius);
	//}

	void OnTriggerStay(Collider Col) {
		Player P = Col.GetComponent<Player> ();
		if (P)
		{
            // force * delta * distMulti
            float DistMulti = Vector3.Distance(P.transform.position, transform.position) * 4;

            P.HitMe(Force * Time.deltaTime * DistMulti, transform.position, Damage);
		}
	}

	public override void Operate()
	{
		GetComponent<Collider> ().enabled = !GetComponent<Collider> ().enabled;
		GetComponentInChildren<FanSpinner> ().enabled = !GetComponentInChildren<FanSpinner> ().enabled;
	}
}
