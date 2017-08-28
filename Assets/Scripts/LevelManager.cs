using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    [SerializeField]
    public Grid grid;
    
    private GridCon _gridCon;
    private GridScene _scene;

    // Use this for initialization
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        _gridCon = new GridCon();

        _scene = new GridScene(grid);

        _scene.EnqueueMove(GridCon.Instance.SplitGridIntoFour, 0);
        _scene.EnqueueMove(GridCon.Instance.ReformGrid, 0.5f);
        _scene.EnqueueMove(GridCon.Instance.SplitGridIntoFour, 0.5f);
        _scene.EnqueueMove(GridCon.Instance.ReformGrid, 1f);
        _scene.EnqueueMove(GridCon.Instance.SplitGridIntoFour, 1f);
        _scene.EnqueueMove(GridCon.Instance.ReformGrid, 1.5f);
        _scene.EnqueueMove(GridCon.Instance.SplitGridIntoFour, 2);
        _scene.EnqueueMove(GridCon.Instance.ReformGrid, 1);
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.F1))
            _scene.Start();

        _scene.Play();
    }
}
