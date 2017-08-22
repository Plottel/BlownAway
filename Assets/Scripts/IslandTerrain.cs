using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IslandTerrain : MonoBehaviour 
{
    public abstract void ApplyEffect(Collision c);

    void OnCollisionEnter(Collision c)
    {
        ApplyEffect(c);
    }


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
