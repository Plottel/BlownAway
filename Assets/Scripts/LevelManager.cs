using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    [SerializeField]
    public Grid grid;
    
    private GridCon _gridCon;

	// Use this for initialization
	void Start ()
    {
        grid = FindObjectOfType<Grid>();
        _gridCon = new GridCon();
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.S))
            GridCon.Instance.SplitGridIntoFour(grid);
        if (Input.GetKeyDown(KeyCode.R))
            GridCon.Instance.ReformGrid(grid);
    }
}
