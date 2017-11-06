using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    [SerializeField]
    public int Cols = 0;
    [SerializeField]
    public int Rows = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Update slider values for desired Cols and Rows.
        Cols = EditorGUILayout.IntSlider("Cols", Cols, 0, 100);
        Rows = EditorGUILayout.IntSlider("Rows", Rows, 0, 100);

        // Store target object to work with.
        Grid grid = (Grid)target;

        // IMPORTANT:
        // Ensures editor changes persist in play mode
        EditorUtility.SetDirty(grid);

        // Fill all cells with default island.
        if (GUILayout.Button("Populate Islands"))
            grid.PopulateIslands();

        // Apply changes to grid size based on Cols and Rows sliders.
        if (GUILayout.Button("Update Grid Size"))
            UpdateGridSize(grid);

        // Clear all islands and turn it back into a blank grid.
        if (GUILayout.Button("Reset"))
            grid.Clear();

        if (GUILayout.Button("Make Populated 10x10 Island"))
            grid.MakePopulated1010();

        if (GUILayout.Button("Kill Cube in Cell Prefabs"))
        {
            for (int col = 0; col < grid.Cols; ++col)
            {
                for (int row = 0; row < grid.Rows; ++row)
                {
                    Cell c = grid[col, row];
                    var cube = c.transform.Find("Cube").gameObject;
                    DestroyImmediate(cube);
                }
            }
        }

        if (GUILayout.Button("Normalize Grid Height"))
        {
            var gridHeight = grid.gameObject.transform.position.y;

            for (int col = 0; col < grid.Cols; ++col)
            {
                for (int row = 0; row < grid.Rows; ++row)
                {
                    var c = grid[col, row];
                    c.gameObject.transform.position = new Vector3(c.gameObject.transform.position.x, gridHeight, c.gameObject.transform.position.z);
                }
                    
            }
        }

        if (GUILayout.Button("Make Winter"))
        {
            // Normalize the terrain height
            for (int col = 0; col < grid.Cols; ++col)
            {
                for (int row = 0; row < grid.Rows; ++row)
                {
                    Cell c = grid[col, row];

                    c.transform.position = new Vector3(c.transform.position.x, grid.transform.position.y, c.transform.position.z);
                }
            }

            // Remove all terrain pieces
            // Reinstantiate terrain pieces with appropriate prefabs
            for (int col = 0; col < grid.Cols; ++col)
            {
                for (int row = 0; row < grid.Rows; ++row)
                {
                    Cell c = grid[col, row];

                    if (c.islandPiece != null)
                    {
                        // Remember terrain
                        // Rember island
                        // Remove terrain
                        // Remove island
                        // Add island
                        // Add terrain

                        var ip = c.islandPiece;
                        var terrain = ip.terrain;

                        var islandPos = ip.transform.position;
                        var islandRotation = ip.transform.rotation;
                        var terrainPos = Vector3.zero;
                        var terrainRotation = Quaternion.identity;
                        var terrainType = TerrainType.None;

                        if (terrain != null)
                        {
                            terrainPos = terrain.transform.position;
                            terrainRotation = terrain.transform.rotation;
                        }

                        // TODO: Needed for winter reskinning.

                        if (terrain != null)
                        {
                            // Stuff here about reinstantiating island terrain prefab.            

                            if (terrain.GetComponent<Ballista>())
                                terrainType = TerrainType.Ballista;
                            else if (terrain.GetComponent<SpikyBush>())
                                terrainType = TerrainType.SpikyBush;
                            else if (terrain.GetComponent<Lava>())
                                terrainType = TerrainType.Lava;
                            else if (terrain.GetComponent<Tree>())
                                terrainType = TerrainType.Tree;
                            else if (terrain.GetComponent<Volcano>())
                                terrainType = TerrainType.Volcano;
                            else if (terrain.GetComponent<Fan>())
                                terrainType = TerrainType.Fan;
                            else if (terrain.GetComponent<LavaPipe>())
                                terrainType = TerrainType.LavaPipe;
                            else if (terrain.GetComponent<Piston>())
                                terrainType = TerrainType.Piston;
                            else if (terrain.GetComponent<PressurePlate>())
                                terrainType = TerrainType.PressurePlate;
                            else if (terrain.GetComponent<Wall>())
                                terrainType = TerrainType.Wall;

                            // Figure out which type was removed and reinstantiate 
                            // with appropriate prefab.
                        }

                        // Scope after terrain ns island piece details have been recorded
                        ip.RemoveTerrain();
                        c.RemoveIslandPiece();
                        c.AddIslandPiece(grid.isWinterSkin);

                        ip = c.islandPiece;

                        if (terrainType != TerrainType.None)
                        {
                            ip.AddTerrain(terrainType, grid.isWinterSkin);
                            ip.terrain.transform.position = terrainPos;
                            ip.terrain.transform.rotation = terrainRotation;
                        }

                    }
                }
            }
        }

    }

    public void UpdateGridSize(Grid grid)
    {      
        int colsToAdd = Cols - grid.Cols;

        if (colsToAdd != 0)
        {
            // Do we add or remove cols? Which one?
            Action gridColOperator = null;

            if (colsToAdd > 0)
                gridColOperator = grid.AddColumn;
            else if (colsToAdd < 0)
                gridColOperator = grid.RemoveColumn;

            // Modify cols.
            for (int i = 0; i < Math.Abs(colsToAdd); ++i)
                gridColOperator();
        }

        int rowsToAdd = Rows - grid.Rows;

        if (rowsToAdd != 0)
        {
            // Do we add or remove rows? Which one?
            Action gridRowOperator = null;

            if (rowsToAdd > 0)
                gridRowOperator = grid.AddRow;
            else if (rowsToAdd < 0)
                gridRowOperator = grid.RemoveRow;

            // Modify rows.
            for (int i = 0; i < Math.Abs(rowsToAdd); ++i)
                gridRowOperator();
        }
    }
}
