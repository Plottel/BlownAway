using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScene_Tutorial : GridScene
{
    //Text tutorialText;
    //private Queue<string> _helpTexts = new Queue<string>();
    private List<Cell> _keyCells;

    public GridScene_Tutorial(Grid grid) : base(grid)
    {
        _keyCells = grid.CellsWithLabel("keyCell");

        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Ballista);
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.SpikyBush);
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Fan);
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Piston);
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Tree);
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Wall);
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Volcano);
    }

    private void ReplaceKeyCellsWithTerrain(Grid grid, params object[] args)
    {
        var terrainType = (TerrainType)args[0];

        foreach (Cell c in _keyCells)
        {
            GridCon.ChangeCellTerrain(grid, c, terrainType);
        }
    }
}
