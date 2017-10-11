using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour 
{
    [SerializeField]
    public Grid grid;
    private GridScene _scene;

    public GameObject ballistaGrid;
    public GameObject factoryGrid;

    public static System.Random RNG = new System.Random();

    private Vector3 gridSpawnPoint = new Vector3(-5.5f, -0.7f, -12.8f);

    public Text timeToNextGridMove;

	public Text ContextualText;

    // Use this for initialization
    void Start()
	{
        grid = FindObjectOfType<Grid>();
        //ContextualText = GetComponentInChildren<MultiplayerController>().TutorialText[4];

        //if (MainMenu.Area == "Insane")
        //    grid = Instantiate(factoryGrid).GetComponent<Grid>();
        //else if (MainMenu.Area == "Ballista")
        //    grid = Instantiate(ballistaGrid).GetComponent<Grid>();

        //grid.transform.position = gridSpawnPoint;

		GetComponentInChildren<MultiplayerController> ().grid = grid;

		GetComponentInChildren<MultiplayerController> ().StartManual ();

        _scene = new GridScene_Factory(grid);

		//_scene = GridCon.CreateGridScene (grid, MainMenu.Area, ContextualText);


		if (_scene == null)
			Debug.Log ("Scene null right after init");

        UpdateTimeToNextGridMove();
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.F1))
            _scene.Start();

		if (_scene == null)
			Debug.Log ("Scene is null");

        _scene.Play();
        UpdateTimeToNextGridMove();
    }

    void UpdateTimeToNextGridMove()
    {
		Debug.Log ("Scene? " + _scene);
        timeToNextGridMove.text = "Grid Move In: " + _scene.TimeToNextMove.ToString();  // + Get some value from GridScene here
    }
}
 