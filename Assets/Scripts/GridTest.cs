using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridTest : MonoBehaviour
{
    [SerializeField]
    public Grid grid;
    private GridScene _scene;

    // Use this for initialization
    void Start()
    {
        Debug.Log("START WAS CALLED");
        grid = FindObjectOfType<Grid>();
        _scene = new GridScene_Ballista_Arena(grid);
        //_scene = GridCon.CreateGridScene(grid, "Ballista", ContextualText);

        if (_scene == null)
            Debug.Log("Scene null right after init");
        else
            Debug.Log("Scene NOT null right after init");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            _scene.Start();

        if (_scene == null)
            Debug.Log("Scene is null");

        _scene.Play();
    }
}
