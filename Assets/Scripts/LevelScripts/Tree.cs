using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : IslandTerrain 
{
    // Use this for initialization
    void Start () 
	{
        // Randomly adjust scale and rotation
        float scaleAdjustment = Random.Range(0.4f, 1.2f);
        float rotationAdjustment = Random.Range(0, 360);
        var s = transform.localScale;
        transform.localScale += new Vector3(scaleAdjustment, scaleAdjustment, scaleAdjustment);
        //transform.localScale.Scale(new Vector3(scaleAdjustment, scaleAdjustment, scaleAdjustment));
        transform.rotation = transform.rotation * Quaternion.Euler(0, rotationAdjustment, 0);

    }
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
