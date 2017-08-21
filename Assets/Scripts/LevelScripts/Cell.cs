using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour 
{
    [SerializeField]
    public SpikyBush spikyBushPrefab;

    [SerializeField]
    public Tree treePrefab;


    public IslandPiece islandPrefab;
    public IslandPiece islandPiece;


    public void AddIslandPiece()
    {
        if (islandPiece != null)
            DestroyImmediate(islandPiece.gameObject);

        islandPiece = Instantiate(islandPrefab, transform.position, Quaternion.identity);
        islandPiece.transform.localScale = new Vector3(1, 1, 1);
        islandPiece.transform.Translate(0, -(islandPiece.transform.localScale.y / 2), 0);
        islandPiece.transform.SetParent(this.transform);       

    }

    public void RemoveIslandPiece()
    {
        DestroyImmediate(islandPiece.gameObject);
        islandPiece = null;
    }

	// Use this for initialization
	void Start() 
	{
        GetComponent<MeshRenderer>().enabled = false;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
