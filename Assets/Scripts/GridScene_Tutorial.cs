using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScene_Tutorial : GridScene
{
	Text tutorialText;
	private Queue<string> _helpTexts = new Queue<string>();

    public GridScene_Tutorial(Grid grid, Text ContextualText) : base(grid)
    {
		tutorialText = ContextualText;

        // Cells can be swapped
		_helpTexts.Enqueue("Island chunks can be swapped");
		_helpTexts.Enqueue ("Island chunks can be swapped");
        EnqueueMove(GridCon.SwapRandomCells, SPEED_SCALE);
        EnqueueMove(GridCon.SwapRandomCells, SPEED_SCALE);

        // Cells can be dropped
		_helpTexts.Enqueue("Island chunks can be dropped");
		_helpTexts.Enqueue("Island chunks can be dropped");
        EnqueueMove(GridCon.DropRandomCell, SPEED_SCALE);
        EnqueueMove(GridCon.DropRandomCell, SPEED_SCALE);

        // Chunks of cells can be dropped - watch out!
		_helpTexts.Enqueue("Large chunks can be dropped - watch out!");
        EnqueueMove(GridCon.DropCellMultiple, SPEED_SCALE, grid.GetQuadrants()[Quadrant.TopLeft]);

        // They might be brought back - lucky!
		_helpTexts.Enqueue("They might be brought back... if you're lucky!");
        EnqueueMove(GridCon.RestoreEmptyCellMultiple, SPEED_SCALE, grid.GetQuadrants()[Quadrant.TopLeft]);

        // Terrain can be brought in from outside
		_helpTexts.Enqueue("This is a tree. You can't walk through it");
		_helpTexts.Enqueue ("This is a Spiky Bush. Walking into it hurts");
		_helpTexts.Enqueue ("This is a Ballista. The closest player has control of it. Ballistas can rotate and shoot");
		_helpTexts.Enqueue ("This is a piston. It will shove you");
        EnqueueMove(GridCon.ChangeCellTerrain, SPEED_SCALE, grid.MidCell, TerrainType.Tree);
        EnqueueMove(GridCon.ChangeCellTerrain, SPEED_SCALE, grid.MidCell, TerrainType.SpikyBush);
        EnqueueMove(GridCon.ChangeCellTerrain, SPEED_SCALE, grid.MidCell, TerrainType.Ballista);
		EnqueueMove (GridCon.ChangeCellTerrain, SPEED_SCALE, grid.MidCell, TerrainType.Piston);

        // The island won't always stay in one piece.
		_helpTexts.Enqueue("The island won't always stay together in one piece");
        EnqueueMove(GridCon.SplitGridIntoFour, SPEED_SCALE, grid);

        // But eventually it'll go back to normal.
		_helpTexts.Enqueue("...but eventually it will reform");
        EnqueueMove(GridCon.ReformGrid, SPEED_SCALE, grid);
    }


	public override void Start()
	{
		base.Start ();

		if (_isPlaying) 
		{
			tutorialText.enabled = true;
			tutorialText.text = _helpTexts.Dequeue ();
		}
	}

	public override void Play()
	{
		base.Play ();

		if (_moveWasDequeued && _isPlaying) 
		{
			if (_helpTexts.Count > 0)
				tutorialText.text = _helpTexts.Dequeue ();
		}

	}
}
