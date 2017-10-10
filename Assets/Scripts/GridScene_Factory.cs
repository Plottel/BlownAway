using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScene_Factory : GridScene
{
    private List<Cell> _patrolFanCells;
    private List<Cell> _nwLift;
    private List<Cell> _swLift;
    private List<Cell> _neLift;
    private List<Cell> _seLift;
    private List<Cell> _allLifts;
    private GridScene _liftScene;

    public GridScene_Factory(Grid grid) : base(grid)
    {
        _liftScene = new GridScene(grid);
        _nwLift = grid.CellsWithLabel("nwlift");
        _swLift = grid.CellsWithLabel("swlift");
        _neLift = grid.CellsWithLabel("nelift");
        _seLift = grid.CellsWithLabel("selift");
        _allLifts = new List<Cell>();
        _allLifts.AddRange(_nwLift);
        _allLifts.AddRange(_swLift);
        _allLifts.AddRange(_neLift);
        _allLifts.AddRange(_seLift);

        Debug.Log("NW: " + _nwLift.Count);
        Debug.Log("NE: " + _neLift.Count);
        Debug.Log("SW: " + _swLift.Count);
        Debug.Log("SE: " + _seLift.Count);

        grid.CellWithLabel("sepressureplate").GetComponentInChildren<PressurePlate>().Linked = grid.CellWithLabel("sepiston");
        grid.CellWithLabel("swpressureplate").GetComponentInChildren<PressurePlate>().Linked = grid.CellWithLabel("swfan");
        grid.CellWithLabel("nepressureplate").GetComponentInChildren<PressurePlate>().Linked = grid.CellWithLabel("nefan");
        grid.CellWithLabel("nwpressureplate").GetComponentInChildren<PressurePlate>().Linked = grid.CellWithLabel("nepiston");
        grid.CellWithLabel("nwinnerpressureplate").GetComponentInChildren<PressurePlate>().Linked = grid.CellWithLabel("neinnerpiston");
        grid.CellWithLabel("neinnerpressureplate").GetComponentInChildren<PressurePlate>().Linked = grid.CellWithLabel("neinnerpiston");
        grid.CellWithLabel("swinnerpressureplate").GetComponentInChildren<PressurePlate>().Linked = grid.CellWithLabel("swinnerpiston");
        grid.CellWithLabel("seinnerpressureplate").GetComponentInChildren<PressurePlate>().Linked = grid.CellWithLabel("seinnerpiston");

        _patrolFanCells = grid.CellsWithLabel("patrolfan");

        _patrolFanCells = new List<Cell>
        {
            _patrolFanCells[0],
            _patrolFanCells[1],
            _patrolFanCells[3],
            _patrolFanCells[2]
        };

        EnqueueMove(MoveLifts1, 1, grid);
        EnqueueMove(MoveLifts2, 1, grid);
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

    private void MoveLifts1(Grid grid, params object[] args)
    {
        for (int i = 0; i < 4; ++i)
        {
            MovePieceTo(_nwLift[i], _swLift[i], true);
            MovePieceTo(_seLift[i], _neLift[i], true);

            // Transfer island ownership.
            _swLift[i].islandPiece = _nwLift[i].islandPiece;
            _swLift[i].islandPiece.transform.SetParent(_swLift[i].transform);
            _nwLift[i].islandPiece = null;

            _neLift[i].islandPiece = _seLift[i].islandPiece;
            _neLift[i].islandPiece.transform.SetParent(_neLift[i].transform);
            _seLift[i].islandPiece = null;
        }
    }

    private void MoveLifts2(Grid grid, params object[] args)
    {
        for (int i = 0; i < 4; ++i)
        {
            MovePieceTo(_swLift[i], _nwLift[i], true);
            MovePieceTo(_neLift[i], _seLift[i], true);

            // Transfer island ownership.
            _nwLift[i].islandPiece = _swLift[i].islandPiece;
            _nwLift[i].islandPiece.transform.SetParent(_nwLift[i].transform);
            _swLift[i].islandPiece = null;

            _seLift[i].islandPiece = _neLift[i].islandPiece;
            _seLift[i].islandPiece.transform.SetParent(_seLift[i].transform);
            _neLift[i].islandPiece = null;
        }
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

    private void MovePieceTo(Cell from, Cell to, bool shake)
    {
        if (from.islandPiece != null)
        {
            from.islandPiece.SetPath(to.transform.position, GridCon.ISLAND_SPEED, true);

            if (shake)
                GridCon.ShakeCell(from, 20, Grid.SHAKE_SPEED, Grid.SHAKE_DISTANCE);
        }
    }

    private void SetTerrainRotation(Grid grid, params object[] args)
    {
        if (args.Length != 2)
            Debug.LogError(args.Length + " args passed instead of 2 to SetTerrainRotation");

        var terrain = (IslandTerrain)args[0];
        var rotationKey = (string)args[1];
        var rotation = GetRotation(rotationKey);

        terrain.transform.eulerAngles = rotation;
    }

    private Vector3 GetRotation(string rot)
    {
        if (rot == "east")
            return new Vector3(0, 0, 0);
        else if (rot == "west")
            return new Vector3(0, 180, 0);
        else if (rot == "north")
            return new Vector3(0, 270, 0);
        else if (rot == "south")
            return new Vector3(0, 90, 0);
        return Vector3.zero;
    }

    public override bool MoveIsComplete
    {
        get
        {
            foreach (Cell c in _allLifts)
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
