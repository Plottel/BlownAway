using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScene_Ballista_Arena : GridScene
{
    private List<Cell> _ringCells;


    public GridScene_Ballista_Arena(Grid grid) : base(grid)
    {
        _ringCells = grid.CellsWithLabel("ring");

        if (_ringCells.Count != 8)
            Debug.LogError(_ringCells.Count + " cells instead of 8 for Ring Cells");

        // Cell list comes like this.
        // 0    3   5 
        // 1        6
        // 2    4   7
        
        // Reorder like this
        // 0    1   2
        // 7        3
        // 6    5   4

        // Order ring cells so move just sends them towards next index in list
        var orderedRingCells = new List<Cell>
        {
            _ringCells[0],
            _ringCells[3],
            _ringCells[5],
            _ringCells[6],
            _ringCells[7],
            _ringCells[4],
            _ringCells[2],
            _ringCells[1]
        };

        _ringCells = orderedRingCells;


        EnqueueMove(RotateRing, 5 * SPEED_SCALE);
        EnqueueMove(RotateRing, 5 * SPEED_SCALE);
        EnqueueMove(RotateRing, 5 * SPEED_SCALE);
        EnqueueMove(RotateRing, 5 * SPEED_SCALE);
        EnqueueMove(RotateRing, 5 * SPEED_SCALE);
        EnqueueMove(RotateRing, 5 * SPEED_SCALE);
        EnqueueMove(RotateRing, 5 * SPEED_SCALE);
    }

    private void RotateRing(Grid grid, params object[] args)
    {
        for (int i = 0; i <= 6; ++i) // Don't loop for last element since it needs to go towards index 0
            MovePieceTo(_ringCells[i], _ringCells[i + 1]);
        MovePieceTo(_ringCells[7], _ringCells[0]); // Move last piece

        // Pieces have moves, transfer ownership
        var islandPieces = new List<IslandPiece>();

        foreach (Cell c in _ringCells)
            islandPieces.Add(c.islandPiece);

        for (int i = 0; i <= 6; ++i)
        {
            _ringCells[i + 1].islandPiece = islandPieces[i];

            if (_ringCells[i + 1].islandPiece != null)
                _ringCells[i + 1].islandPiece.transform.SetParent(_ringCells[i + 1].transform);
        }

        _ringCells[0].islandPiece = islandPieces[7];

        if (_ringCells[0].islandPiece != null)
            _ringCells[0].islandPiece.transform.SetParent(_ringCells[0].transform);
    }

    private void MovePieceTo(Cell from, Cell to)
    {
        if (from.islandPiece != null)
        {
            from.islandPiece.SetPath(to.transform.position, GridCon.ISLAND_SPEED, true);
            GridCon.ShakeCell(from, 20, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);
        }
    }
}
