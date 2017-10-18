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
        //Time.timeScale = 0.3f;
        grid = FindObjectOfType<Grid>();
        grid.isWinterSkin = false;

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
                    // Remember terrain
                    // Rember island
                    // Remove terrain
                    // Remove island
                    // Add island
                    // Add terrain
                    
                    var ip = c.islandPiece;
                    var terrain = ip.terrain;

                    var islandPos = ip.transform.position;
                    var islandRotation = ip.transform.rotation;
                    var terrainPos = Vector3.zero;
                    var terrainRotation = Quaternion.identity;
                    var terrainType = TerrainType.None;

                    if (terrain != null)
                    {
                        terrainPos = terrain.transform.position;
                        terrainRotation = terrain.transform.rotation;
                    }

                    // TODO: Needed for winter reskinning.

                    if (terrain != null)
                    {
                        // Stuff here about reinstantiating island terrain prefab.            

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
                        else if (terrain.GetComponent<Wall>())
                            terrainType = TerrainType.Wall;

                        // Figure out which type was removed and reinstantiate 
                        // with appropriate prefab.
                    }

                    // Scope after terrain ns island piece details have been recorded
                    ip.RemoveTerrain();
                    c.RemoveIslandPiece();
                    c.AddIslandPiece(grid.isWinterSkin);

                    ip = c.islandPiece;

                    if (terrainType != TerrainType.None)
                    {
                        ip.AddTerrain(terrainType, grid.isWinterSkin);
                        ip.terrain.transform.position = terrainPos;
                        ip.terrain.transform.rotation = terrainRotation;
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

        //_scene = new GridScene_VolcanoRun(grid);

		//_scene = GridCon.CreateGridScene (grid, MainMenu.Area, ContextualText);

		//if (_scene == null)
		//	Debug.Log ("Scene null right after init");

        
        //_scene.Start();

        //UpdateTimeToNextGridMove();
    }
	
	// Update is called once per frame
	void Update () 
	{

		//if (_scene == null)
			//Debug.Log ("Scene is null");

        //_scene.Play();
       // UpdateTimeToNextGridMove();
    }

    void UpdateTimeToNextGridMove()
    {
		//Debug.Log ("Scene? " + _scene);
        //timeToNextGridMove.text = "Grid Move In: " + _scene.TimeToNextMove.ToString();  // + Get some value from GridScene here
    }
}
 