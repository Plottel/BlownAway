using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public delegate void GridMove(Grid grid, params object[] args);
}   

public class GridScene
{
    private Grid _grid;
    private Queue<GridMove> _moves;
    private Queue<float> _moveDelays;
    private Queue<object[]> _args;

	protected bool _moveWasDequeued = false;

    protected bool _isPlaying = false;
    protected bool _waitingForNextMove = false;
    private float _moveFinishedAt = 0f;

    public float TimeToNextMove
    {
        get
        {
            if (!_isPlaying)
                return 0;
            if (!_waitingForNextMove)
                return 0;

            float result = _moveDelays.Peek() - (Time.time - _moveFinishedAt);
            return result > 0 ? result : 0f; 
        }
    }

    public GridScene(Grid grid)
    {
        _grid = grid;
        _moves = new Queue<GridMove>();
        _moveDelays = new Queue<float>();
        _args = new Queue<object[]>();
    }

    public void EnqueueMove(GridMove move, float delay, params object[] args)
    {
        _moves.Enqueue(move);
        _moveDelays.Enqueue(delay);
        _args.Enqueue(args);
    }

    public virtual void Start()
    {
        if (_moves.Count > 0)
        {
            _args.Enqueue(_args.Peek()); // Put first args to the back of the queue for looping. 
            _moves.Peek()(_grid, _args.Dequeue());
            _isPlaying = true;
        }
    }

    public virtual void Play()
    {
		_moveWasDequeued = false;

        if (!_isPlaying)
            return;
        if (_moves.Count == 0)
            return;

        if (MoveIsComplete)
        {
            // Move has finished. Are there still moves to process?
            if (!_waitingForNextMove)
            {
                GridCon.CleanUpOffScreenPieces();
                _waitingForNextMove = true;
                _moveFinishedAt = Time.time;
            }
            // Has enough time passed to start the next move?
            else if (Time.time - _moveFinishedAt > _moveDelays.Peek())
            {
				_moveWasDequeued = true;

                // Push move and move delay to back of queue for looping
                _moves.Enqueue(_moves.Peek());
                _moveDelays.Enqueue(_moveDelays.Peek());

                _moves.Dequeue();
                _moveDelays.Dequeue();


                _waitingForNextMove = false;

                if (_moves.Count > 0)
                {
                    // Push args to back of queue for looping
                    _args.Enqueue(_args.Peek());
                    _moves.Peek()(_grid, _args.Dequeue());
                }
            }
        }
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