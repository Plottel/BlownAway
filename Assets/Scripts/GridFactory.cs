using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridFactory
{
    public static Cell MakeTerrainCellAt(Vector3 pos, TerrainType type)
    {
        if (type == TerrainType.SpikyBush)
            return MakeSpikyBushCellAt(pos);
        else if (type == TerrainType.Tree)
            return MakeTreeCellAt(pos);
        else if (type == TerrainType.Ballista)
            return MakeBallistaCellAt(pos);
        else if (type == TerrainType.Piston)
            return MakePistonCellAt(pos);
        else if (type == TerrainType.Fan)
            return MakeFanCellAt(pos);
        else if (type == TerrainType.Lava)
            return MakeLavaCellAt(pos);
        else if (type == TerrainType.LavaPipe)
            return MakeLavaPipeCellAt(pos);
        else if (type == TerrainType.Volcano)
            return MakeVolcanoCellAt(pos);
        else if (type == TerrainType.Wall)
            return MakeWallCellAt(pos);
        else
            return MakeIslandPieceCellAt(pos);
    }

    public static Cell MakeSpikyBushCellAt(Vector3 pos)
    {
        Cell c = Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);

        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.SpikyBush);

        return c;
    }

    public static Cell MakeTreeCellAt(Vector3 pos)
    {
        Cell c = Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);

        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Tree);

        return c;
    }

    public static Cell MakeBallistaCellAt(Vector3 pos)
    {
        Cell c = Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);

        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Ballista);

        return c;
    }

    public static Cell BlankCell(Vector3 pos)
    {
        return Object.Instantiate(Prefabs.Cell, pos, Quaternion.identity);
    }
    
    public static Cell MakePistonCellAt(Vector3 pos)
    {
        Cell c = BlankCell(pos);

        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Piston);

        return c;
    }

    public static Cell MakeFanCellAt(Vector3 pos)
    {
        Cell c = BlankCell(pos);
        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Fan);
        return c;
    }

    public static Cell MakeLavaCellAt(Vector3 pos)
    {
        var c = BlankCell(pos);
        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Lava);
        return c;
    }

    public static Cell MakeLavaPipeCellAt(Vector3 pos)
    {
        var c = BlankCell(pos);
        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.LavaPipe);
        return c;
    }

    public static Cell MakeVolcanoCellAt(Vector3 pos)
    {
        var c = BlankCell(pos);
        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Volcano);
        return c;
    }

    public static Cell MakeWallCellAt(Vector3 pos)
    {
        var c = BlankCell(pos);
        c.AddIslandPiece();
        c.islandPiece.AddTerrain(TerrainType.Wall);
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
