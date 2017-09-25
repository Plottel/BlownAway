using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScene_Tutorial : GridScene
{
    public float MOVE_DELAY = 1f;

    public GridScene_Tutorial(Grid grid) : base(grid)
    {
        // Cells can be swapped
        EnqueueMove(GridCon.SwapRandomCells, MOVE_DELAY);
        EnqueueMove(GridCon.SwapRandomCells, MOVE_DELAY);

        // Cells can be dropped
        EnqueueMove(GridCon.DropRandomCell, MOVE_DELAY);
        EnqueueMove(GridCon.DropRandomCell, MOVE_DELAY);

        // Chunks of cells can be dropped - watch out!
        EnqueueMove(GridCon.DropCellMultiple, MOVE_DELAY, grid.GetQuadrants()[Quadrant.TopLeft]);

        // They might be brought back - lucky!
        EnqueueMove(GridCon.RestoreEmptyCellMultiple, MOVE_DELAY, grid.GetQuadrants()[Quadrant.TopLeft]);

        // Terrain can be brought in from outside
        EnqueueMove(GridCon.ChangeCellTerrain, MOVE_DELAY, grid.MidCell, TerrainType.Tree);
        EnqueueMove(GridCon.ChangeCellTerrain, MOVE_DELAY, grid.MidCell, TerrainType.SpikyBush);
        EnqueueMove(GridCon.ChangeCellTerrain, MOVE_DELAY, grid.MidCell, TerrainType.Ballista);

        // The island won't always stay in one piece.
        EnqueueMove(GridCon.SplitGridIntoFour, MOVE_DELAY, grid);

        // But eventually it'll go back to normal.
        EnqueueMove(GridCon.ReformGrid, MOVE_DELAY, grid);
    }
}
