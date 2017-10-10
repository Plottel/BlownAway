using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : IslandTerrain {

	public int ExplosionForce = 400;
	public int ExplosionRadius = 100;

	public void Hit(Player target)
	{
		float Multiplier = target.Health;
		Multiplier = (Multiplier / 100f) + 1;
		//target.Health += Damage;
		float DistMulti = Vector3.Distance(target.transform.position, transform.position) * 4;
		target.GetComponent<Rigidbody>().AddExplosionForce ((ExplosionForce * Multiplier * Time.deltaTime * 30) / DistMulti, this.gameObject.transform.position, ExplosionRadius);
	}

	void OnTriggerStay(Collider Col) {
		Player P = Col.GetComponent<Player> ();
		if (P)
		{
			Debug.Log ("I hit someone ;)");
			Hit (P);
		}
	}

	public override void Operate()
	{
		GetComponent<Collider> ().enabled = !GetComponent<Collider> ().enabled;
		GetComponentInChildren<FanSpinner> ().enabled = !GetComponentInChildren<FanSpinner> ().enabled;
	}
}
