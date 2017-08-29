using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell : MonoBehaviour 
{
    [SerializeField]
    public SpikyBush spikyBushPrefab;

    [SerializeField]
    public Tree treePrefab;

    public IslandPiece islandPrefab;

    [SerializeField]
    public IslandPiece islandPiece;

    public List<Cell> neighbours = new List<Cell>();

    public Vector2 Pos2D
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.z);
        }
    }

    public bool IslandIsConnected
    {
        get
        {
            if (islandPiece == null)
                return false;

            Vector2 cellPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 islandPos = new Vector2(islandPiece.transform.position.x, islandPiece.transform.position.z);

            return cellPos == islandPos;
        }
    }

    public void AddIslandPiece()
    {
        if (islandPiece != null)
            DestroyImmediate(islandPiece.gameObject);

        islandPiece = Instantiate(Prefabs.IslandPiece, transform.position, Quaternion.identity);
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
