using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Grid grid = (Grid)target;

        if (GUILayout.Button("Add Column"))
            grid.AddColumn();

        if (GUILayout.Button("Add Row"))
            grid.AddRow();
    }
}
