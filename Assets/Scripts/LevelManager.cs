using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    [SerializeField]
    public Grid grid;
    private GridScene _scene;

    public static System.Random RNG = new System.Random();

    // Use this for initialization
    void Start()
    {
        grid = FindObjectOfType<Grid>();

        _scene = new GridScene(grid);

        TestAllMoves();

    }

    public void TestAllMoves()
    {
        _scene.EnqueueMove(GridCon.SwapTwoCells, 1f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SwapTwoCells, 1f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.MoveGridBy, 0f, new Vector3(0, 0, 4f));
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.F1))
            _scene.Start();

        _scene.Play();
    }
}
