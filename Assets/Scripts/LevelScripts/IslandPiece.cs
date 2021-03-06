﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class IslandPiece : MonoBehaviour 
{
    public GameObject winterTerrainPrefab;
    public IslandTerrain terrainPrefab;
    public IslandTerrain terrain;

    private float _speed;

    private bool _followPath = false;
    private Queue<Vector3> _path = new Queue<Vector3>();

    public bool HasArrived
    {
        get { return _followPath == false && _path.Count == 0; }
    }

    public void AddTerrain(TerrainType type, bool isWinterSkin=false)
    {
        if (terrain != null)
            DestroyImmediate(terrain.gameObject);

        // Instantiate a winter skin.
        if (isWinterSkin)
        {
            if (type == TerrainType.Tree)
                winterTerrainPrefab = Prefabs.WinterTree;
            else if (type == TerrainType.Fan)
                winterTerrainPrefab = Prefabs.WinterFan;
            else if (type == TerrainType.Piston)
                winterTerrainPrefab = Prefabs.WinterPiston;
            else if (type == TerrainType.SpikyBush)
                winterTerrainPrefab = Prefabs.WinterSpikyBush;
            else if (type == TerrainType.PressurePlate)
                winterTerrainPrefab = Prefabs.WinterPressurePlate;
            else if (type == TerrainType.LavaPipe)
                winterTerrainPrefab = Prefabs.WinterLavaPipe;
            else if (type == TerrainType.Wall)
                winterTerrainPrefab = Prefabs.WinterWall;
            else
            {
                // Don't have a winter skin for it, get the non-winter version.
                if (type == TerrainType.SpikyBush)
                    terrainPrefab = Prefabs.SpikyBush;
                else if (type == TerrainType.Tree)
                    terrainPrefab = Prefabs.WinterTree.GetComponent<Tree>();
                else if (type == TerrainType.Ballista)
                    terrainPrefab = Prefabs.Ballista;
                else if (type == TerrainType.Piston)
                    terrainPrefab = Prefabs.Piston;
                else if (type == TerrainType.Fan)
                    terrainPrefab = Prefabs.Fan;
                else if (type == TerrainType.PressurePlate)
                    terrainPrefab = Prefabs.PressurePlate;
                else if (type == TerrainType.Lava)
                    terrainPrefab = Prefabs.Lava;
                else if (type == TerrainType.Volcano)
                    terrainPrefab = Prefabs.Volcano;

                terrain = Instantiate(terrainPrefab, transform.position, Quaternion.identity);
                //terrain = Instantiate(winterTerrainPrefab, transform.position, Quaternion.identity).GetComponent<IslandTerrain>(); // terrainPrefab.transform.rotation);
                terrain.transform.parent = this.transform;
                return;

            }
            
            // Instantiate valid winter prefab
            terrain = Instantiate(winterTerrainPrefab, transform.position, Quaternion.identity).GetComponent<IslandTerrain>();
            //terrain = Instantiate(winterTerrainPrefab, transform.position, Quaternion.identity).GetComponent<IslandTerrain>(); // terrainPrefab.transform.rotation);
            terrain.transform.parent = this.transform;
        }
        else // Not winter skin, just fetch normal
        {
            // Instantiate a not winter skin.
            if (type == TerrainType.SpikyBush)
                terrainPrefab = Prefabs.SpikyBush;
            else if (type == TerrainType.Tree)
                terrainPrefab = Prefabs.Tree;
            else if (type == TerrainType.Ballista)
                terrainPrefab = Prefabs.Ballista;
            else if (type == TerrainType.Piston)
                terrainPrefab = Prefabs.Piston;
            else if (type == TerrainType.Fan)
                terrainPrefab = Prefabs.Fan;
            else if (type == TerrainType.PressurePlate)
                terrainPrefab = Prefabs.PressurePlate;
            else if (type == TerrainType.Lava)
                terrainPrefab = Prefabs.Lava;
            else if (type == TerrainType.LavaPipe)
                terrainPrefab = Prefabs.LavaPipe;
            else if (type == TerrainType.Volcano)
                terrainPrefab = Prefabs.Volcano;
            else if (type == TerrainType.Wall)
                terrainPrefab = Prefabs.Wall;

            // Instantiate normal
            terrain = Instantiate(terrainPrefab, transform.position, Quaternion.identity); // terrainPrefab.transform.rotation);
            terrain.transform.parent = this.transform;
        }
        

        // Instantiate, maintaining Prefab rotation
		
//        terrain.transform.Translate(0, transform.lossyScale.y, 0);
//        terrain.transform.rotation = terrainPrefab.transform.rotation;

        // Move terrain so it sits on top of island - parent it.
        //terrain.transform.Translate(0, terrainPrefab.transform.localScale.y, 0);
    }

    public void RemoveTerrain()
    {
        if (terrain == null)
            return;
        DestroyImmediate(terrain.gameObject);
        terrain = null;
    }

    public void SetPath(Vector3 target, float speed, bool clearPath)
    {
        SetPath(new List<Vector3> { target }, speed, clearPath); 
    }

    public void SetPath(List<Vector3> path, float speed, bool clearPath)
    {
        _followPath = true;

        for (int i = 0; i < path.Count; ++i)
            path[i] = new Vector3(path[i].x, path[i].y + 0.5f, path[i].z);

        if (clearPath)
            _path = new Queue<Vector3>(path);
        else
            _path = new Queue<Vector3>(path.Concat(_path));
        _speed = speed;
    }

    /*
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
    */


    // Use this for initialization
    void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_followPath)
        {
            // Move towards next node in path.
            this.transform.position = Vector3.MoveTowards(this.transform.position, _path.Peek(), _speed * Time.deltaTime);

            // Have we reached target node?
            if (this.transform.position == _path.Peek())
            {
                // Do we have more nodes in the path?
                _path.Dequeue();
                if (_path.Count == 0)
                    _followPath = false;
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
            collision.gameObject.transform.parent = this.transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        // TODO: Maybe need to set parent back to "Map"
        if (collision.gameObject.GetComponent<Player>())
            collision.gameObject.transform.parent = null;
    }
}
