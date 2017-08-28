using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public delegate void GridMove(Grid grid);
}

public class GridScene
{
    private Grid _grid;
    private Queue<GridMove> _moves;

    public GridScene(Grid grid)
    {
        _grid = grid;
        _moves = new Queue<GridMove>();
    }

    public void EnqueueMove(GridMove move)
    {
        _moves.Enqueue(move);
    }

    public void Start()
    {
        if (_moves.Count > 0)
            _moves.Peek()(_grid);
    }

    public bool Play()
    {
        if (!MoveIsComplete)
            return false;

        // Move has finished. Are there still moves to process?
        _moves.Dequeue();
        if (_moves.Count == 0)
            return true;

        // Start new move
        _moves.Peek()(_grid);
        return false;

    }

    public bool MoveIsComplete
    {
        get
        {
            for (int col = 0; col < _grid.Cols; ++col)
            {
                for (int row = 0; row < _grid.Rows; ++row)
                {
                    Cell c = _grid[col, row];

                    if (c.islandPiece != null)
                    {
                        if (!c.islandPiece.HasArrived)
                            return false;
                    }
                }
            }

            return true;
        }
    }
}
