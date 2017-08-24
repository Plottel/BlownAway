using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridMovementMap = Dictionary<Grid, Vector2>;



public class GridManager : MonoBehaviour
{



    public List<Grid> grids = new List<Grid>();

    public GridMovementMap movingGrids = new GridMovementMap();



    public void MoveGridTo(Grid grid, Vector2 pos)
    {
        if (grids.Contains(grid))
        {

        }
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
