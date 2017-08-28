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
    private Queue<float> _moveDelays;
    private bool _waitingForNextMove = false;
    private float _moveFinishedAt = 0f;

    public GridScene(Grid grid)
    {
        _grid = grid;
        _moves = new Queue<GridMove>();
        _moveDelays = new Queue<float>();
    }

    public void EnqueueMove(GridMove move, float delay)
    {
        _moves.Enqueue(move);
        _moveDelays.Enqueue(delay);
    }

    public void Start()
    {
        if (_moves.Count > 0)
            _moves.Peek()(_grid);
    }

    public void Play()
    {
        if (_moves.Count == 0)
            return;

        if (MoveIsComplete)
        {
            // Move has finished. Are there still moves to process?
            if (!_waitingForNextMove)
            {
                _moves.Dequeue();
                _waitingForNextMove = true;
                _moveFinishedAt = Time.time;
            }
            // Has enough time passed to start the next move?
            else if (Time.time - _moveFinishedAt > _moveDelays.Peek())
            {
                _moveDelays.Dequeue();
                _moves.Peek()(_grid);
                _waitingForNextMove = false;
            }
        }        
    }

    private bool MoveIsComplete
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
