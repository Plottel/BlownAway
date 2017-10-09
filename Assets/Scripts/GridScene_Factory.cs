using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScene_Factory : GridScene
{
    public GridScene_Factory(Grid grid) : base(grid)
    {
        EnqueueMove(SetTerrainRotation, 1, grid[0, 0].islandPiece.terrain, "east");
        EnqueueMove(SetTerrainRotation, 1, grid[0, 0].islandPiece.terrain, "west");
        EnqueueMove(SetTerrainRotation, 1, grid[0, 0].islandPiece.terrain, "north");
        EnqueueMove(SetTerrainRotation, 1, grid[0, 0].islandPiece.terrain, "south");
    }    

    private void SetTerrainRotation(Grid grid, params object[] args)
    {
        if (args.Length != 2)
            Debug.LogError(args.Length + " args passed instead of 2 to SetTerrainRotation");

        var terrain = (IslandTerrain)args[0];
        var rotationKey = (string)args[1];
        var rotation = GetRotation(rotationKey);

        Debug.Log("Terrain Null? " + (terrain == null).ToString());
        Debug.Log("Transform Null? " + (terrain.transform == null).ToString());
        Debug.Log("Euler ANgles null?" + (terrain.transform.eulerAngles == null).ToString());

        Debug.Log(rotation.ToString());

        terrain.transform.eulerAngles = rotation;
    }

    private Vector3 GetRotation(string rot)
    {
        if (rot == "east")
            return new Vector3(0, 0, 0);
        else if (rot == "west")
            return new Vector3(0, 180, 0);
        else if (rot == "north")
            return new Vector3(0, 270, 0);
        else if (rot == "south")
            return new Vector3(0, 90, 0);
        return Vector3.zero;
    }
}
