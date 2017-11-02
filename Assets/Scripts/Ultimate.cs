using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Destroy(this.gameObject, 0.5f);
        gameObject.GetComponent<Rigidbody>().velocity += new Vector3(0, -1, 0);
	}
	
	void OnTriggerEnter (Collider col)
	{
        var island = col.gameObject.GetComponent<IslandPiece>();

        if (island)
        {
            var islandGO = island.gameObject;

            var grid = LevelManager.Instance.grid;

            if (islandGO != null)
            {
                GridCon.DropCell(grid, islandGO.transform.parent.GetComponent<Cell>());
            }
        }
	}
}
