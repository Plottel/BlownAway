using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GridTest : MonoBehaviour
{
    [SerializeField]
    public IslandGrid grid;
    private GridScene _scene;

    private List<Player> _players;

    private Vector3 V3(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    // Use this for initialization
    void Start()
    {
        _players = FindObjectsOfType<Player>().ToList();

        Debug.Log("PLAYER COUNT" + _players.Count);

        Debug.Log("START WAS CALLED");
        grid = FindObjectOfType<IslandGrid>();

        _scene = new GridScene_Tutorial(grid);
        //_scene = new GridScene_Ballista_Arena(grid);
        //_scene = GridCon.CreateGridScene(grid, "Ballista", ContextualText);

        if (grid == null)
            Debug.Log("Grid null in init");

        if (_scene == null)
            Debug.Log("Scene null right after init");
        else
            Debug.Log("Scene NOT null right after init");

        //GenerateAndApplyGridHeightMap(grid);   
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateCameraZoomAndPosition();

        if (Input.GetKeyDown(KeyCode.F1))
            _scene.Start();

        if (_scene == null)
            Debug.Log("Scene is null");

        _scene.Play();
    }

    static double NthRoot(double A, int N)
    {
        return System.Math.Pow(A, 1.0 / N);
    }

    private void ApplyGridHeightMap(IslandGrid grid, List<Cell>[] heightMap, float heightChangePower)
    {
        foreach (List<Cell> square in heightMap)
        {
            float chunkNormalizer = Random.Range(-heightChangePower, heightChangePower);
            foreach (Cell c in square)
            {
                float cellRandom = Random.Range(-heightChangePower, heightChangePower);
                float normalizedHeightChange = (chunkNormalizer * cellRandom) / 2;
                c.transform.Translate(V3(0, normalizedHeightChange, 0));
            }
                               
        }
    }

    private void GenerateAndApplyGridHeightMap(IslandGrid grid)
    {
        // We only have 3 partition cycles because our grid is small....
        // Just run the height map a bunch of times to split those levels up into other levels
        int numTimesToApplyHeightMap = 1;
        
        for (int appCount = 0; appCount < numTimesToApplyHeightMap; ++appCount)
        {
            float heightMapPower = 0.45f;

            int partitionSize = grid.Cols;
            int numPartitions = 1;

            int numCellsInGrid = grid.Cols * grid.Rows;

            // Start a new partition.
            while (numPartitions <= numCellsInGrid)
            {
                int curIdx = 0;

                var newPartition = new List<Cell>[numPartitions];

                // Loop for each partition part which needs to be constructed
                for (int partCount = 0; partCount < numPartitions; ++partCount)
                {
                    var newPartitionPart = new List<Cell>();

                    // For each cell part of the newPartitionPart
                    for (int c = curIdx; c < curIdx + partitionSize; ++c)
                    {
                        for (int r = curIdx; r < curIdx + partitionSize; ++r)
                            newPartitionPart.Add(grid[c, r]);
                    }

                    // Partition part has been constructed, add it to the newPartition
                    newPartition[partCount] = newPartitionPart;
                }

                // Partition has been constructed, apply height map changes
                // and update subdivision details
                // Next time we do more partitions, but they're smaller and weaker
                numPartitions *= 2;
                partitionSize /= 2;

                // Apply height map changes
                ApplyGridHeightMap(grid, newPartition, heightMapPower);

                // Weaker height power next partition.
                heightMapPower /= 2;
            }
        }       
    }

    private void UpdateCameraZoomAndPosition()
    {
        var cam = Camera.main;
        var newCamPos = new Vector3();

        var playerCenter = GetPlayerCenterPoint(_players);
        playerCenter.y = cam.transform.position.y; // Dont move camera Y from player center

        cam.transform.position = playerCenter;      
    }

    private Vector3 GetPlayerCenterPoint(List<Player> players)
    {
        var positions = new List<Vector3>();
        foreach (var p in players)
            positions.Add(p.transform.position);

        Vector3 result = Vector3.zero;

        foreach (var pos in positions)
        {
            result += pos;
        }

        result /= positions.Count;
        return result;
    }
}