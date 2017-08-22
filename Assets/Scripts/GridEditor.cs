using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    public int Cols = 0;
    public int Rows = 0;

    public override void OnInspectorGUI()
    {
        // grid.cols = 6
        // cols = 2
        DrawDefaultInspector();

        Cols = EditorGUILayout.IntSlider("Cols", Cols, 0, 30);
        Rows = EditorGUILayout.IntSlider("Rows", Rows, 0, 30);

        Grid grid = (Grid)target;      

        if (GUILayout.Button("Populate Islands"))
            grid.PopulateIslands();

        if (GUILayout.Button("Update Grid Size"))
            UpdateGridSize(grid);

    }

    public void UpdateGridSize(Grid grid)
    {      
        int colsToAdd = Cols - grid.cols;

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

        int rowsToAdd = Rows - grid.rows;

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
