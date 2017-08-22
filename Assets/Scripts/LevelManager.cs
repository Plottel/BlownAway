using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    public Grid grid;

	// Use this for initialization
	void Start () 
	{
        PathManager.grid = grid;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
