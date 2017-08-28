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
        Cols = EditorGUILayout.IntSlider("Cols", Cols, 0, 30);
        Rows = EditorGUILayout.IntSlider("Rows", Rows, 0, 30);

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
