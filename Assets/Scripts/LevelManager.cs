using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour 
{
    [SerializeField]
    public Grid grid;
    private GridScene _scene;
    public bool gameStarted = false;

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

        // Normalize the terrain height
        for (int col = 0; col < grid.Cols; ++col)
        {
            for (int row = 0; row < grid.Rows; ++row)
            {
                Cell c = grid[col, row];

                c.transform.position = new Vector3(c.transform.position.x, grid.transform.position.y, c.transform.position.z);
            }
        }

        // Remove all terrain pieces
        // Reinstantiate terrain pieces with appropriate prefabs
        for (int col = 0; col < grid.Cols; ++col)
        {
            for (int row = 0; row < grid.Rows; ++row)
            {
                Cell c = grid[col, row];

                if (c.islandPiece != null)
                {
                    var ip = c.islandPiece;
                    // Stuff here about reinstantiating island piece prefab.
                    // TODO: Needed for winter reskinning.

                    if (ip.terrain != null)
                    {
                        var terrain = ip.terrain;
                        // Stuff here about reinstantiating island terrain prefab.
                        var pos = terrain.transform.position;
                        var rotation = terrain.transform.rotation;

                        TerrainType terrainType = TerrainType.None;

                        if (terrain.GetComponent<Ballista>())
                            terrainType = TerrainType.Ballista;
                        else if (terrain.GetComponent<SpikyBush>())
                            terrainType = TerrainType.SpikyBush;
                        else if (terrain.GetComponent<Lava>())
                            terrainType = TerrainType.Lava;
                        else if (terrain.GetComponent<Tree>())
                            terrainType = TerrainType.Tree;
                        else if (terrain.GetComponent<Volcano>())
                            terrainType = TerrainType.Volcano;
                        else if (terrain.GetComponent<Fan>())
                            terrainType = TerrainType.Fan;
                        else if (terrain.GetComponent<LavaPipe>())
                            terrainType = TerrainType.LavaPipe;
                        else if (terrain.GetComponent<Piston>())
                            terrainType = TerrainType.Piston;
                        else if (terrain.GetComponent<PressurePlate>())
                            terrainType = TerrainType.PressurePlate;
                        
                        // Reinstantiate new prefab
                        ip.RemoveTerrain();
                        ip.AddTerrain(terrainType);
                        ip.terrain.transform.position = pos;
                        ip.terrain.transform.rotation = rotation;


                        // Figure out which type was removed and reinstantiate 
                        // with appropriate prefab.
                    }

                }
            }
        }





        //ContextualText = GetComponentInChildren<MultiplayerController>().TutorialText[4];

        //if (MainMenu.Area == "Insane")
        //    grid = Instantiate(factoryGrid).GetComponent<Grid>();
        //else if (MainMenu.Area == "Ballista")
        //    grid = Instantiate(ballistaGrid).GetComponent<Grid>();

        //grid.transform.position = gridSpawnPoint;

		GetComponentInChildren<MultiplayerController> ().grid = grid;

		GetComponentInChildren<MultiplayerController> ().StartManual ();

        _scene = new GridScene_VolcanoRun(grid);

		//_scene = GridCon.CreateGridScene (grid, MainMenu.Area, ContextualText);

		if (_scene == null)
			Debug.Log ("Scene null right after init");

        
        _scene.Start();

        //UpdateTimeToNextGridMove();
    }
	
	// Update is called once per frame
	void Update () 
	{

		if (_scene == null)
			Debug.Log ("Scene is null");

        _scene.Play();
       // UpdateTimeToNextGridMove();
    }

    void UpdateTimeToNextGridMove()
    {
		Debug.Log ("Scene? " + _scene);
        timeToNextGridMove.text = "Grid Move In: " + _scene.TimeToNextMove.ToString();  // + Get some value from GridScene here
    }
}
 