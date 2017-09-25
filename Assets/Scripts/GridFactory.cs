using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridFactory
{
    public static Cell MakeTerrainCellAt(Vector3 pos, TerrainType type)
    {
        if (type == TerrainType.Ballista)
            return MakeBallistaCellAt(pos);
        else if (type == TerrainType.Tree)
            return MakeTreeCellAt(pos);
        else
            return MakeSpikyBushCellAt(pos);
    }

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

    public static Cell MakeSpikyBushCellAt(Vector3 pos)
    {
        Cell c = Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);

        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.SpikyBush);

        return c;
    }

    public static Cell MakeIslandPieceCellAt(Vector3 pos)
    {
        Cell c = Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);
        c.AddIslandPiece();

        return c;
    }

    public static Cell MakeEmptyCellAt(Vector3 pos)
    {
        return Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);
    }
}
