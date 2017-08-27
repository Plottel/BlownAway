using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandPiece : MonoBehaviour 
{
    public IslandTerrain terrainPrefab;
    public IslandTerrain terrain;

    private bool _followDirect = false;
    public Vector3 directTarget;
    private float _speed;


    private bool _followPath = false;

    private List<Cell> _path;

    public void AddTerrain()
    {
        if (terrain != null)
            DestroyImmediate(terrain.gameObject);

        terrain = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
        terrain.transform.localScale = new Vector3(0.75f, 1.2f, 0.75f);
        terrain.transform.Translate(0, terrain.transform.localScale.y, 0);
        terrain.transform.SetParent(this.transform);
    }

    public void RemoveTerrain()
    {
        DestroyImmediate(terrain.gameObject);
        terrain = null;
    }

    public void SetPathDirect(Vector3 target, float speed)
    {
        _followDirect = true;
        directTarget = target;
        _speed = speed;
    }

    public void SetPathToCell(Cell target)
    {
        // Should only be true if valid path fetched.
        _followPath = true;

        var src = this.transform.parent.GetComponent<Cell>();
        var open = new List<Cell>();
        var closed = new List<Cell>();
        var parents = new Dictionary<Cell, Cell>();
        var scores = new Dictionary<Cell, float>();
        var gScores = new Dictionary<Cell, float>();

        var current = src;

        if (target == src)
            return;

        open.Add(current);
        parents.Add(current, null);
        scores.Add(current, 0);
        gScores.Add(current, 1);

        while (open.Count > 0)
        {
            if (current == target)
                break;

            foreach (Cell cell in current.neighbours)
            {
                if (!cell.IslandIsConnected && 
                    !closed.Contains(cell) && 
                    !open.Contains(cell))
                {
                    open.Add(cell);
                    parents.Add(cell, current);

                    // Calc g-score
                    float gScore = 1;
                    Cell c = current;

                    while (parents[c] != null)
                    {
                        gScore += gScores[parents[c]];
                        c = parents[c];
                    }

                    float hScore = Vector2.Distance(current.Pos2D, cell.Pos2D);

                    // Add scores
                    scores.Add(cell, hScore + gScore);
                    gScores.Add(cell, gScore);

                }
            }

            open.Remove(current);
            closed.Add(current);

            open = open.OrderBy(openCell => scores[openCell]).ToList();
            current = open[0];
        }

        // Search complete - fetch patch.
        _path = new List<Cell>();

        // Add target
        _path.Add(target);
        _path.Add(current);

        // Retrace parents back to start
        while (parents[current] != null)
        {
            _path.Add(parents[current]);
            current = parents[current];
        }

        _path.Add(src);

        _path.Reverse();    
    }

    // Use this for initialization
    void Start () 
	{
        _path = new List<Cell>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_followPath)
        {
            // Path following stuff here.
        }
        else if (_followDirect)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, directTarget, _speed * Time.deltaTime);
        }
	}
}
