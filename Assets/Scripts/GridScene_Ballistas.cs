using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScene_Ballistas : GridScene
{
    public int MOVE_DELAY_SCALE = 1;

    public GridScene_Ballistas(Grid grid) : base(grid)
    {
        EnqueueMove(GridCon.ChangeCellTerrain, 20 * MOVE_DELAY_SCALE, grid.MidCell, TerrainType.Ballista);  // Bring ballista to middle.
        EnqueueMove(GridCon.ChangeCellTerrainMultiple, 0, grid.Corners, TerrainType.Ballista);              // Bring 4 ballistas to corners.
        EnqueueMove(GridCon.RestoreEmptyCell, 10 * MOVE_DELAY_SCALE, grid.MidCell);                         // Immediately remove middle ballista

    }

}
