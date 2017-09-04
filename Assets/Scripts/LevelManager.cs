using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour 
{
    [SerializeField]
    public Grid grid;
    private GridScene _scene;

    public static System.Random RNG = new System.Random();

    public Text timeToNextGridMove;

    // Use this for initialization
    void Start()
    {
        grid = FindObjectOfType<Grid>();

        _scene = new GridScene(grid);


        _scene.EnqueueMove(GridCon.SwapTwoCells, 5f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SwapTwoCells, 7.65f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SwapTwoCells, 3.9f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SwapTwoCells, 1.23487f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SwapTwoCells, 10.75f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SwapTwoCells, 12f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SwapTwoCells, 3.544f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);
        _scene.EnqueueMove(GridCon.SwapTwoCells, 2.543258f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], grid[RNG.Next(0, 8), RNG.Next(0, 8)]);

        //TestAllMoves();

        // There's an extra shake somewhere.
        // Definitely affects SplitGridIntoFour()
        // Possibly affects ReformGrid()
        // SwapTwoCells() is fine
        UpdateTimeToNextGridMove();

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
        UpdateTimeToNextGridMove();
    }

    void UpdateTimeToNextGridMove()
    {
        timeToNextGridMove.text = "Grid Move In: " + _scene.TimeToNextMove.ToString();  // + Get some value from GridScene here
    }
}
