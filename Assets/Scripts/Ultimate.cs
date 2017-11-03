using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour {

    private SpawnPointer _sp;

	// Use this for initialization
	void Start () {
		//Destroy(this.gameObject, 0.5f);
        gameObject.GetComponent<Rigidbody>().velocity += new Vector3(0, -1, 0);

        _sp = Instantiate(Prefabs.SpawnPointer, new Vector3(0, 4, 0), Quaternion.Euler(new Vector3(90, 0, 0))).GetComponent<SpawnPointer>();
        //_sp = Instantiate(Prefabs.SpawnPointer, new Vector3(0, 4, 0), Quaternion.identity);
        _sp.Target = this.gameObject.transform;
        _sp.isPartOfUltimate = true;
        _sp.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
	}

    void OnDestroy()
    {
        DestroyImmediate(_sp.gameObject);
    }

    void FixedUpdate()
    {
        _sp.ManualUpdate();
        _sp.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
    }
	
	void OnTriggerEnter (Collider col)
	{
        var p = col.GetComponent<Player>();
        if (p)
            p.HitMe(700f, p.transform.position, 50);

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

        var terrain = col.gameObject.GetComponent<Terrain>();

        if (terrain)
        {
            var terrainsIsland = terrain.gameObject.GetComponentInParent<IslandPiece>();

            if (terrainsIsland)
            {
                var islandGO = terrainsIsland.gameObject;
                var grid = LevelManager.Instance.grid;

                if (islandGO != null)
                {
                    GridCon.DropCell(grid, islandGO.transform.parent.GetComponent<Cell>());
                }
            }
        }
    }
}
