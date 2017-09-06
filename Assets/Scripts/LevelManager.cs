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

        //_scene.EnqueueMove(GridCon.ReplaceBorderWithTrees, 1.5f);

        _scene.EnqueueMove(GridCon.ReplaceWithOffScreenPiece, 0, grid[RNG.Next(0, 8), RNG.Next(0, 8)], GridFactory.MakeTreeCellAt(grid.transform.position + new Vector3(10, 0, 20)));
        _scene.EnqueueMove(GridCon.ReplaceWithOffScreenPiece, 0, grid[RNG.Next(0, 8), RNG.Next(0, 8)], GridFactory.MakeTreeCellAt(grid.transform.position + new Vector3(10, 0, 20)));
        _scene.EnqueueMove(GridCon.ReplaceWithOffScreenPiece, 0, grid[RNG.Next(0, 8), RNG.Next(0, 8)], GridFactory.MakeTreeCellAt(grid.transform.position + new Vector3(10, 0, 20)));
        _scene.EnqueueMove(GridCon.ReplaceWithOffScreenPiece, 0, grid[RNG.Next(0, 8), RNG.Next(0, 8)], GridFactory.MakeTreeCellAt(grid.transform.position + new Vector3(10, 0, 20)));
        _scene.EnqueueMove(GridCon.ReplaceWithOffScreenPiece, 0, grid[RNG.Next(0, 8), RNG.Next(0, 8)], GridFactory.MakeTreeCellAt(grid.transform.position + new Vector3(10, 0, 20))); 

        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);
        _scene.EnqueueMove(GridCon.SwapTwoRandomCells, 0);

        _scene.EnqueueMove(GridCon.SwapTwoChunks, 3, grid.GetQuadrants()[Quadrant.BotRight], grid.GetQuadrants()[Quadrant.TopLeft]);
        _scene.EnqueueMove(GridCon.SwapTwoChunks, 3, grid.GetQuadrants()[Quadrant.BotLeft], grid.GetQuadrants()[Quadrant.TopRight]);




        // TODO: Make a higher level function so the calls are less obnoxious.
        //cene.EnqueueMove(GridCon.ReplaceWithOffScreenPiece, 1.5f, grid[RNG.Next(0, 8), RNG.Next(0, 8)], GridFactory.MakeBallistaCellAt(grid.transform.position + new Vector3(10, 0, 20)));

        //TestAllMoves();

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
        GridCon.CleanUpOffScreenPieces();
    }

    void UpdateTimeToNextGridMove()
    {
        timeToNextGridMove.text = "Grid Move In: " + _scene.TimeToNextMove.ToString("n2");  // + Get some value from GridScene here
    }    
}
