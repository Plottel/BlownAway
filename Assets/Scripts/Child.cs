using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour {
	void OnTriggerEnter(Collider Col) {
		GetComponentInParent<IslandTerrain> ().Triggered (Col);
	}
}
