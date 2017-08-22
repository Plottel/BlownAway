using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandPiece : MonoBehaviour 
{
    public IslandTerrain terrainPrefab;
    public IslandTerrain terrain;

    public void AddTerrain()
    {
        if (terrain != null)
            DestroyImmediate(terrain.gameObject);

        terrain = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
        terrain.transform.localScale = new Vector3(0.75f, 1.2f, 0.75f);
        terrain.transform.Translate(0, terrain.transform.localScale.y, 0);
        terrain.transform.SetParent(this.transform);
    }

    public void RemoveTerrain()
    {
        DestroyImmediate(terrain.gameObject);
        terrain = null;
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
