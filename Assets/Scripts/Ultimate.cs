using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (this, 0.2f);
	}
	
	void OnTriggerEnter (Collider col)
	{
		var island = col.gameObject.GetComponent<IslandPiece>();

		if (island) 
		{
			var grid = FindObjectOfType<Grid> ();
			var cell = island.transform.parent;
			GridCon.DropCell (grid, cell);
		}
	}
}
