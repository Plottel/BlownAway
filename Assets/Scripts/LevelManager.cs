using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    [SerializeField]
    public Grid grid;
    private GridScene _scene;

    // Use this for initialization
    void Start()
    {
        grid = FindObjectOfType<Grid>();

        _scene = new GridScene(grid);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.SplitGridIntoFour, 0);
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
