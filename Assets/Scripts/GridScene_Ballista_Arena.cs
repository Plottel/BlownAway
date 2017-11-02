using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScene_Ballista_Arena : GridScene
{
    private List<Cell> _swWall, _seWall, _nwWall, _neWall;

    public GridScene_Ballista_Arena(Grid grid) : base(grid)
    {
        _swWall = grid.CellsWithLabel("wallPlatformSW");
        _seWall = grid.CellsWithLabel("wallPlatformSE");
        _nwWall = grid.CellsWithLabel("wallPlatformNW");
        _neWall = grid.CellsWithLabel("wallPlatformNE");

        EnqueueMove(MoveWallPlatform, 6 * SPEED_SCALE);
        EnqueueMove(MoveWallPlatform, 6 * SPEED_SCALE);
        EnqueueMove(MoveWallPlatform, 6 * SPEED_SCALE);
        EnqueueMove(MoveWallPlatform, 6 * SPEED_SCALE);
        EnqueueMove(MoveWallPlatform, 6 * SPEED_SCALE);
        EnqueueMove(MoveWallPlatform, 6 * SPEED_SCALE);
        EnqueueMove(MoveWallPlatform, 6 * SPEED_SCALE);
    }

    private void MoveWallPlatform(Grid grid, params object[] args)
    {
        // SW -> NW
        for (int i = 0; i < 3; ++i)
            MovePieceTo(_swWall[i], _nwWall[i]);

        // NW -> NE
        for (int i = 0; i < 3; ++i)
            MovePieceTo(_nwWall[i], _neWall[i]);

        // NE -> SE
        for (int i = 0; i < 3; ++i)
            MovePieceTo(_neWall[i], _seWall[i]);

        // SE -> SW
        for (int i = 0; i < 3; ++i)
            MovePieceTo(_seWall[i], _swWall[i]);


        // Pieces have moved. Transfer ownership
        // Make function
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
