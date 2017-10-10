using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TerrainType
{
    SpikyBush,
    Tree,
    Ballista,
    Piston,
    Fan,
    PressurePlate,
    Lava,
    LavaPipe,
    Volcano,
    None
}

[CustomEditor(typeof(Cell))]
[CanEditMultipleObjects]
public class CellEditor : Editor
{
    public TerrainType terrainType;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var cells = targets;

        if (GUILayout.Button("Add Island Piece"))
        {
            foreach (Cell c in cells)
                c.AddIslandPiece();
        }

        if (GUILayout.Button("Remove Island Piece"))
        {
            foreach (Cell c in cells)
                c.RemoveIslandPiece();
        }

        if (GUILayout.Button("Add Terrain"))
        {
            foreach (Cell c in targets)
            {
                var ip = c.islandPiece;

                if (ip != null)
                    ip.AddTerrain(terrainType);            
            }
        }

        if (GUILayout.Button("Remove Terrain"))
        {
            foreach (Cell c in targets)
            {
                IslandPiece ip = c.islandPiece;

                if (ip != null)
                    ip.RemoveTerrain();
            }
        }

        terrainType = (TerrainType)EditorGUILayout.EnumPopup("Terrain Type", terrainType);
    }
}
