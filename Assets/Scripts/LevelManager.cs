using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
    [SerializeField]
    public Grid grid;
    
    private GridCon _gridCon;
    private GridScene _scene;
    private bool _sequenceIsFinished = false;

    // Use this for initialization
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        _gridCon = new GridCon();

        _scene = new GridScene(grid);

        _scene.EnqueueMove(GridCon.Instance.SplitGridIntoFour);
        _scene.EnqueueMove(GridCon.Instance.ReformGrid);
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.F1))
            _scene.Start();

        if (!_sequenceIsFinished)
            _sequenceIsFinished = _scene.Play();
    }
}
