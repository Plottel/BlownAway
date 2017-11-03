using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScene_Tutorial : GridScene
{
    //Text tutorialText;
    //private Queue<string> _helpTexts = new Queue<string>();
    private List<Cell> _keyCells;

    private GameObject[] _invisWalls;

    public GridScene_Tutorial(Grid grid) : base(grid)
    {

        GridCon.ISLAND_SPEED = 6f;

        _invisWalls = GameObject.FindGameObjectsWithTag("WallTutorial");

        _keyCells = grid.CellsWithLabel("keyCell");

        EnqueueMove(KillWalls, 1, "Barriers are down! FIGHT! (or choose a proper level from the menu)");
        EnqueueMove(Blank, 3, "Left Joystick moves and aims");
        EnqueueMove(Blank, 4, "If you want more control, you can aim with the Right Joystick");
        EnqueueMove(Blank, 5, "Left Bumper jumps! You can jump three times, which will be refreshed when you hit the ground");
        EnqueueMove(Blank, 5, "Right Bumper attacks in front of you. This also increases the charge of your ultimate ability");
        EnqueueMove(Blank, 2, "Look at the Player Details along the bottom of the screen");
        EnqueueMove(Blank, 3, "Below the heart is how many lives you have left");
        EnqueueMove(Blank, 5, "Above the cross is your damage percent. The higher this is, the further you will be pushed");
        EnqueueMove(Blank, 3, "Above the star is your ultimate charge");
        EnqueueMove(Blank, 4, "At 100 percent, use your ultimate by holding both bumpers for a full second.");
        EnqueueMove(Blank, 5, "In a real game, choose where to spawn with Left Joystick and spawn with Right Bumper");

        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.SpikyBush, "Don't run in to Spiky Bushes - they hurt!");
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Ballista, "Right Bumper to shoot a Cannon Ball. Cannons face away from the nearest player");
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Fan, "Fans will push you around. They don't hit you hard, but make navigating terrain difficult");
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Piston, "Pistons will push in front of them every so often. They hit hard!");
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Tree, "Trees are nice and harmless. Use them for cover!");
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Wall, "Walls are tall and can be walked on top of!");
        EnqueueMove(ReplaceKeyCellsWithTerrain, 6, TerrainType.Volcano, "Volcanos will spread lava until the whole level is on fire");
        EnqueueMove(KillWalls, 6, "Barriers are down! FIGHT! (or choose a proper level from the menu)");
        EnqueueMove(Blank, int.MaxValue, "");
    }

    private void Blank(Grid grid, params object[] args)
    {
        var newTutText = (string)args[0];
        tutText.text = newTutText;
    }

    private void KillWalls(Grid grid, params object[] args)
    {
        var newTutText = (string)args[0];
        tutText.text = newTutText;

        foreach (var wall in _invisWalls)
            Object.DestroyImmediate(wall);
    }

    private void ReplaceKeyCellsWithTerrain(Grid grid, params object[] args)
    {
        var terrainType = (TerrainType)args[0];

        var newTutText = (string)args[1];
        tutText.text = newTutText;

        foreach (Cell c in _keyCells)
        {
            GridCon.ChangeCellTerrain(grid, c, terrainType);
        }
    }
}
