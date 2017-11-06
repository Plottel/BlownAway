using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainKillBox : MonoBehaviour 
{
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("DESTROYEDOIUEJDAOISJDIOAJSDIOJ");
        if (col.gameObject.GetComponent<IslandPiece>())
        {
            Destroy(col.gameObject);
            Debug.Log("ACTUALLY destroying");
        }

    }
}
