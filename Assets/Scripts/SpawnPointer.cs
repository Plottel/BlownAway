using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointer : MonoBehaviour {

	public bool OverTile = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit Hit;

		Vector3 RayStart = new Vector3 (transform.position.x, 5, transform.position.z);

		if (Physics.Raycast (RayStart, -transform.up, out Hit)) {
			transform.position = Hit.point;
			OverTile = true;
		} else {
			OverTile = false;
		}
	}
}
