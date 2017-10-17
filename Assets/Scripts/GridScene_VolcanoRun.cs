using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScene_VolcanoRun: GridScene
{
    private List<Cell> _patrolFanCells;
    private List<Cell> _rightPlatform1;
    private List<Cell> _rightPlatform2;
    private List<Cell> _botPlatform1;
    private List<Cell> _botPlatform2;
    private List<Cell> _allPlatforms;

    public GridScene_VolcanoRun(Grid grid) : base(grid)
    {
        _patrolFanCells = grid.CellsWithLabel("patrolFan");
        _rightPlatform1 = grid.CellsWithLabel("rightPlatform1");
        _rightPlatform2 = grid.CellsWithLabel("rightPlatform2");
        _botPlatform1 = grid.CellsWithLabel("botPlatform1");
        _botPlatform2 = grid.CellsWithLabel("botPlatform2");

        _allPlatforms = _rightPlatform1;
        _allPlatforms.AddRange(_rightPlatform2);
        _allPlatforms.AddRange(_botPlatform1);
        _allPlatforms.AddRange(_botPlatform2);

        // Order properly
        _patrolFanCells = new List<Cell>
        {
            _patrolFanCells[0],
            _patrolFanCells[1],
            _patrolFanCells[3],
            _patrolFanCells[2]
        };

        EnqueueMove(MoveLifts1, 3);
        EnqueueMove(MoveLifts2, 3);
        EnqueueMove(MoveLifts1, 3);
        EnqueueMove(MoveLifts2, 3);
        EnqueueMove(MoveLifts1, 3);
        EnqueueMove(MoveLifts2, 3);
    }

    public override void Start()
    {
        base.Start();
        MovePatrolFan(_grid);
    }

    public override void Play()
    {
        if (!_isPlaying)
            return;

        foreach (Cell c in _patrolFanCells)
        {
            if (c.IslandIsConnected)
            {
                RotatePatrolFan(_grid);
                MovePatrolFan(_grid);
            }
        }

        base.Play();
    }

    private void MovePatrolFan(Grid grid, params object[] args)
    {
        for (int i = 0; i <= 2; ++i) // Don't loop for last element since it needs to go towards index 0
            MovePieceTo(_patrolFanCells[i], _patrolFanCells[i + 1], false);
        MovePieceTo(_patrolFanCells[3], _patrolFanCells[0], false); // Move last piece

        // Pieces have moved, transfer ownership
        var islandPieces = new List<IslandPiece>();

        foreach (Cell c in _patrolFanCells)
            islandPieces.Add(c.islandPiece);

        for (int i = 0; i <= 2; ++i)
        {
            _patrolFanCells[i + 1].islandPiece = islandPieces[i];

            if (_patrolFanCells[i + 1].islandPiece != null)
                _patrolFanCells[i + 1].islandPiece.transform.SetParent(_patrolFanCells[i + 1].transform);
        }

        _patrolFanCells[0].islandPiece = islandPieces[3];

        if (_patrolFanCells[0].islandPiece != null)
            _patrolFanCells[0].islandPiece.transform.SetParent(_patrolFanCells[0].transform);
    }

    private void RotatePatrolFan(Grid grid, params object[] args)
    {
        foreach (Cell c in _patrolFanCells)
            c.transform.eulerAngles = c.transform.eulerAngles + new Vector3(0, 90, 0);
    }

    private void MoveLifts1(Grid grid, params object[] args)
    {
        for (int i = 0; i < 4; ++i)
        {
            MovePieceTo(_rightPlatform1[i], _rightPlatform2[i], true);
            MovePieceTo(_botPlatform1[i], _botPlatform2[i], true);

            // Transfer island ownership.
            _rightPlatform2[i].islandPiece = _rightPlatform1[i].islandPiece;
            _rightPlatform2[i].islandPiece.transform.SetParent(_rightPlatform2[i].transform);
            _rightPlatform1[i].islandPiece = null;

            _botPlatform2[i].islandPiece = _botPlatform1[i].islandPiece;
            _botPlatform2[i].islandPiece.transform.SetParent(_botPlatform2[i].transform);
            _botPlatform1[i].islandPiece = null;
        }
    }

    private void MoveLifts2(Grid grid, params object[] args)
    {
        for (int i = 0; i < 4; ++i)
        {
            MovePieceTo(_rightPlatform2[i], _rightPlatform1[i], true);
            MovePieceTo(_botPlatform2[i], _botPlatform1[i], true);

            // Transfer island ownership.
            _rightPlatform1[i].islandPiece = _rightPlatform2[i].islandPiece;
            _rightPlatform1[i].islandPiece.transform.SetParent(_rightPlatform1[i].transform);
            _rightPlatform2[i].islandPiece = null;

            _botPlatform1[i].islandPiece = _botPlatform2[i].islandPiece;
            _botPlatform1[i].islandPiece.transform.SetParent(_botPlatform1[i].transform);
            _botPlatform2[i].islandPiece = null;
        }
    }

    private void MovePieceTo(Cell from, Cell to, bool shake)
    {
        if (from.islandPiece != null)
        {
            from.islandPiece.SetPath(to.transform.position, GridCon.ISLAND_SPEED, true);

            if (shake)
                GridCon.ShakeCell(from, 20, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);
        }
    }

    public override bool MoveIsComplete
    {
        get
        {
            foreach (Cell c in _allPlatforms)
            {
                IslandPiece ip = c.islandPiece;

                if (ip != null)
                {
                    if (!ip.HasArrived)
                        return false;
                }
            }

            return true;
        }
    }
}
