using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : IslandTerrain {
	/// <summary>
	/// Damage per second.
	/// </summary>
	public float Damage = 200f;

	void OnTriggerStay(Collider Col) {
		Player P = Col.GetComponent<Player> ();
		if (P)
		{
			Debug.Log ("I hit someone ;)");
			P.Health += Damage * Time.deltaTime;
		}
	}
}
