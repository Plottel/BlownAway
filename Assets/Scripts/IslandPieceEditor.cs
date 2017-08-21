using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IslandPiece))]
[CanEditMultipleObjects]
public class IslandPieceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var islandPieces = targets;

        if (GUILayout.Button("Add Terrain"))
        {
            foreach (IslandPiece IP in targets)
                IP.AddTerrain();

        }

        if (GUILayout.Button("Remove Terrain"))
        {
            foreach (IslandPiece IP in targets)
                IP.RemoveTerrain();
        }
    }
}
