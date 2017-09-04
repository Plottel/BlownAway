using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridFactory
{
    public static Cell MakeBallistaCellAt(Vector3 pos)
    {
        Cell c = Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);

        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Ballista);

        return c;
    }

    public static Cell MakeTreeCellAt(Vector3 pos)
    {
        Cell c = Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);

        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Tree);

        return c;
    }
}
