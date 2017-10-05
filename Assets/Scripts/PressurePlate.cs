using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : IslandTerrain {

	public Cell Linked;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{	
		if (col.gameObject.GetComponent<Player> ())
			SteppedOn ();
				
	}


	void SteppedOn () {
		if (Linked != null) {
			Linked.Operate ();
		}
	}
}
