using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridScene_Ballistas : GridScene
{
    private Cell _midLeft1In;
    private Cell _topMid1In;
    private Cell _botMid1In;
    private Cell _botLeft1In;
    private Cell _botRight1In;
    private Cell _botRightOfLeftHalf1In;
    private Cell _midOfRightHalf;

    private List<Cell> _midLeftTreeSquare = new List<Cell>();

    private List<Cell> _leftDropChunk = new List<Cell>();
    private List<Cell> _rightDropChunk = new List<Cell>();
    private List<Cell> _rightWholeChunk = new List<Cell>();
    private List<Cell> _leftWholeChunk = new List<Cell>();
    private List<Cell> _topLeftHalfBorder = new List<Cell>();
    private List<Cell> _protectiveBallistaTreeWall = new List<Cell>();
    private List<Cell> _finalStageHoles = new List<Cell>();
    private List<Cell> _finalStageSpikyBushCells = new List<Cell>();

    public GridScene_Ballistas(IslandGrid grid) : base(grid)
    {
        int midCols = (int)Mathf.Floor(grid.Cols / 2);
        int midRows = (int)Mathf.Floor(grid.Rows / 2);

        _midLeft1In = grid[1, midRows];
        _topMid1In = grid[grid.Cols / 2, 1];
        _botMid1In = grid[grid.Cols / 2, grid.Rows - 2];
        _botLeft1In = grid[1, grid.Rows - 2];
        _botRight1In = grid[grid.Cols - 2, grid.Rows - 2];
        _botRightOfLeftHalf1In = grid[grid.Cols - 2 - midCols - 1, grid.Rows - 2];
        _midOfRightHalf = grid[Mathf.FloorToInt(grid.Cols * 0.75f), midRows];

        _midLeftTreeSquare.Add(grid[0, midRows - 2]);
        _midLeftTreeSquare.Add(grid[1, midRows - 2]);
        _midLeftTreeSquare.Add(grid[2, midRows - 2]);
        _midLeftTreeSquare.Add(grid[2, midRows - 1]);
        _midLeftTreeSquare.Add(grid[2, midRows]);
        _midLeftTreeSquare.Add(grid[2, midRows + 1]);
        _midLeftTreeSquare.Add(grid[2, midRows + 2]);
        _midLeftTreeSquare.Add(grid[1, midRows + 2]);
        _midLeftTreeSquare.Add(grid[0, midRows + 2]);

        for (int col = 0; col < midCols - 1; ++col)
        {
            for (int row = 0; row < grid.Rows; ++row)
                _leftDropChunk.Add(grid[col, row]);
        }

        for (int col = midCols + 2; col < grid.Cols; ++col)
        {
            for (int row = 0; row < grid.Rows; ++row)
                _rightDropChunk.Add(grid[col, row]);
        }

        _rightWholeChunk.AddRange(grid.GetQuadrant(Quadrant.TopRight));
        _rightWholeChunk.AddRange(grid.GetQuadrant(Quadrant.BotRight));

        _leftWholeChunk.AddRange(grid.GetQuadrant(Quadrant.TopLeft));
        _leftWholeChunk.AddRange(grid.GetQuadrant(Quadrant.BotLeft));
        

        _topLeftHalfBorder = grid.TopRow.Concat(grid.LeftCol).ToList();
        _topLeftHalfBorder.Remove(grid[0, 0]);

        for (int col = midCols; col < grid.Cols; ++col)
            _topLeftHalfBorder.Remove(grid[col, 0]);

        _protectiveBallistaTreeWall.Add(grid[grid.Cols - 4 - midCols - 1, grid.Rows - 1]);
        _protectiveBallistaTreeWall.Add(grid[grid.Cols - 4 - midCols - 1, grid.Rows - 2]);
        _protectiveBallistaTreeWall.Add(grid[grid.Cols - 1 - midCols - 1, grid.Rows - 4]);
        _protectiveBallistaTreeWall.Add(grid[grid.Cols - 2 - midCols - 1, grid.Rows - 4]);

        int midRightHalfCol = Mathf.FloorToInt(grid.Cols * 0.75f);

        // NE hole
        _finalStageHoles.Add(grid[midRightHalfCol + 2, midRows - 2]);
        _finalStageHoles.Add(grid[midRightHalfCol + 2, midRows - 1]);
        _finalStageHoles.Add(grid[midRightHalfCol + 3, midRows - 2]);
        _finalStageHoles.Add(grid[midRightHalfCol + 3, midRows - 1]);

        // SE hole
        _finalStageHoles.Add(grid[midRightHalfCol + 2, midRows + 2]);
        _finalStageHoles.Add(grid[midRightHalfCol + 2, midRows + 1]);
        _finalStageHoles.Add(grid[midRightHalfCol + 3, midRows + 2]);
        _finalStageHoles.Add(grid[midRightHalfCol + 3, midRows + 1]);

        // NW hole
        _finalStageHoles.Add(grid[midRightHalfCol - 2, midRows - 2]);
        _finalStageHoles.Add(grid[midRightHalfCol - 2, midRows - 1]);
        _finalStageHoles.Add(grid[midRightHalfCol - 3, midRows - 2]);
        _finalStageHoles.Add(grid[midRightHalfCol - 3, midRows - 1]);

        // SW hole
        _finalStageHoles.Add(grid[midRightHalfCol - 2, midRows + 2]);
        _finalStageHoles.Add(grid[midRightHalfCol - 2, midRows + 1]);
        _finalStageHoles.Add(grid[midRightHalfCol - 3, midRows + 2]);
        _finalStageHoles.Add(grid[midRightHalfCol - 3, midRows + 1]);


        _finalStageSpikyBushCells.Add(grid[midRightHalfCol + 4, midRows + 4]);
        _finalStageSpikyBushCells.Add(grid[midRightHalfCol + 4, midRows - 4]);
        _finalStageSpikyBushCells.Add(grid[midRightHalfCol - 4, midRows + 4]);
        _finalStageSpikyBushCells.Add(grid[midRightHalfCol - 4, midRows - 4]);
        _finalStageSpikyBushCells.Add(grid[midRightHalfCol + 4, midRows]);
        _finalStageSpikyBushCells.Add(grid[midRightHalfCol - 4, midRows]);
        _finalStageSpikyBushCells.Add(grid[midRightHalfCol, midRows + 4]);
        _finalStageSpikyBushCells.Add(grid[midRightHalfCol, midRows - 4]);





        // Bring a ballista to mid left cell
        EnqueueMove(GridCon.ChangeCellTerrain, 1 * SPEED_SCALE, _midLeft1In, TerrainType.Ballista);
        // Surround ballista in a square of trees, trapping whoever is there
        EnqueueMove(GridCon.ChangeCellTerrainMultiple, 1 * SPEED_SCALE, _midLeftTreeSquare, TerrainType.Tree);
        // Bring ballistas to top mid and bot mid cells
        EnqueueMove(GridCon.ChangeCellTerrainMultiple, 1 * SPEED_SCALE, new List<Cell> { _topMid1In, _botMid1In }, TerrainType.Ballista);
        // Replace first ballista and tree square with normal cells
        EnqueueMove(GridCon.ChangeCellTerrainMultiple, 1 * SPEED_SCALE, _midLeftTreeSquare.Concat(new List<Cell> { _midLeft1In }).ToList(), TerrainType.None);
        // Bring a spiky bush into the middle of the bridge
        EnqueueMove(GridCon.ChangeCellTerrain, 1 * SPEED_SCALE, grid.MidCell, TerrainType.SpikyBush);
        // Drop left and right sides, leaving a top-bottom bridge with a ballista at each end
        EnqueueMove(GridCon.DropCellMultiple, 1 * SPEED_SCALE, (List<Cell>)_leftDropChunk.Concat(_rightDropChunk).ToList());
        // Bring back dropped sides
        EnqueueMove(GridCon.RestoreEmptyCellMultiple, 1 * SPEED_SCALE, (List<Cell>)_leftDropChunk.Concat(_rightDropChunk).ToList());
        // Remove ballistas and spiky bush, back to normal
        EnqueueMove(GridCon.ChangeCellTerrainMultiple, 1 * SPEED_SCALE, new List<Cell> { _topMid1In, _botMid1In, grid.MidCell }, TerrainType.None);
        // Drop right side
        EnqueueMove(GridCon.DropCellMultiple, 1 * SPEED_SCALE, _rightWholeChunk);
        // Bring in spiky bush border along top and right
        EnqueueMove(GridCon.ChangeCellTerrainMultiple, 1 * SPEED_SCALE, _topLeftHalfBorder, TerrainType.SpikyBush);
        // Bring in a ballista bot right
        EnqueueMove(GridCon.ChangeCellTerrain, 1 * SPEED_SCALE, _botRightOfLeftHalf1In, TerrainType.Ballista);
        //Bring in protective tree wall for ballista
        EnqueueMove(GridCon.ChangeCellTerrainMultiple, 1 * SPEED_SCALE, _protectiveBallistaTreeWall, TerrainType.Tree);
        // Bring back right side
        EnqueueMove(GridCon.RestoreEmptyCellMultiple, 1 * SPEED_SCALE, _rightWholeChunk);
        // Populate right side
        EnqueueMove(FinalStagePopulateRightHalf, 1 * SPEED_SCALE);
        // Drop left side
        EnqueueMove(GridCon.DropCellMultiple, 1 * SPEED_SCALE, _leftWholeChunk);
        // Clear right side
        EnqueueMove(FinalStageClearRightHalf, 1 * SPEED_SCALE);
        // Bring back left side - back to normal
        EnqueueMove(GridCon.RestoreEmptyCellMultiple, 1 * SPEED_SCALE, _leftWholeChunk);
    }

    private void FinalStagePopulateRightHalf(IslandGrid grid, params object[] args)
    {
        if (args.Length != 0)
            Debug.LogError(args.Length + " args passed instead of 0");

        GridCon.ChangeCellTerrain(grid, _midOfRightHalf, TerrainType.Ballista);
        GridCon.DropCellMultiple(grid, _finalStageHoles);
        GridCon.ChangeCellTerrainMultiple(grid, _finalStageSpikyBushCells, TerrainType.SpikyBush);
    }

    private void FinalStageClearRightHalf(IslandGrid grid, params object[] args)
    {
        GridCon.ChangeCellTerrain(grid, _midOfRightHalf, TerrainType.None);
        GridCon.RestoreEmptyCellMultiple(grid, _finalStageHoles);
        GridCon.ChangeCellTerrainMultiple(grid, _finalStageSpikyBushCells, TerrainType.None);
    }

}
