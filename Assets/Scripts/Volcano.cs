using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PSA - DONT MOVE VOLCANOES. LET THEM STAY WHERE THEY WERE SPAWNED
/// </summary>
public class Volcano : IslandTerrain {
    private Vector2 cellIndex;

    [SerializeField]
    public static float TicksPerSpread = 60;

    [SerializeField]
    private float _currentTick = 0;

    private Cell _cell;

    private List<List<Cell>> _spreadTargets = new List<List<Cell>>();

	// Use this for initialization
	void Start () {
        Grid grid = transform.parent.parent.parent.gameObject.GetComponent<Grid>();
        Cell cell = transform.parent.parent.gameObject.GetComponent<Cell>();
        _cell = cell;

        cellIndex = grid.IndexOf(cell);

        int borderThickness = 3;

        // Populate spread targets
        while (true)
        {
            var nextLayerOfTargets = grid.GetOutlineCellSquare(cellIndex, borderThickness);
            borderThickness += 2;

            if (nextLayerOfTargets.Count == 0)
                break;

            _spreadTargets.Add(nextLayerOfTargets);
        }
	}

    // Lava can be spread to any cell with an active island piece.
    private void SpreadLava()
    {
        Cell current = _cell;
        var open = new List<Cell>();
        var closed = new List<Cell>();

        open.Add(current);

        while (open.Count > 0)
        {
            current = open[0];

            foreach (Cell cell in current.neighbours)
            {
                if (open.Contains(cell) || closed.Contains(cell))
                    continue;

                // Try to spread lava.
                if (cell.IslandIsConnected)
                {
                    IslandPiece ip = cell.islandPiece;
                    IslandTerrain it = ip.terrain;

                    if (it == null || it.GetComponent<Lava>() == null)
                    {
                        ip.AddTerrain(TerrainType.Lava);
                        return;
                    }

                    // If can't spread lava, add to open
                    open.Add(cell);
                }
            }

            open.Remove(current);
            closed.Add(current);
        }
    }       

    private void FixedUpdate()
    {
        var cell = transform.parent.parent.GetComponent<Cell>();

        var ip = transform.parent.GetComponent<IslandPiece>();

        if (!ip.HasArrived)
            return;

        ++_currentTick;

        if (_currentTick == TicksPerSpread)
        {
            _currentTick = 0;
            SpreadLava();
        }
    }
}
