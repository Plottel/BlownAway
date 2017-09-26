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

	public Text ContextualText;

    // Use this for initialization
    void Start()
	{
		grid = FindObjectOfType<Grid>();
		GetComponentInChildren<MultiplayerController> ().grid = grid;

		GetComponentInChildren<MultiplayerController> ().StartManual ();

		ContextualText = GetComponentInChildren<MultiplayerController>().TutorialText[4];


		_scene = GridCon.CreateGridScene (grid, MainMenu.Area, ContextualText);

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
        timeToNextGridMove.text = "Grid Move In: " + _scene.TimeToNextMove.ToString();  // + Get some value from GridScene here
    }
}
