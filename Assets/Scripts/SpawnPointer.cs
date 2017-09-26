using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SpawnPointer : MonoBehaviour {

	public bool OverTile = false;
	public string Player = "P3";
	
	// Update is called once per frame

	public bool ManualUpdate () {

		float h = CrossPlatformInputManager.GetAxis(Player + "_Horizontal");
		float v = CrossPlatformInputManager.GetAxis(Player + "_Vertical");

		transform.position = new Vector3 (transform.position.x + (h*0.3f), transform.position.y, transform.position.z + (v*0.3f));

		RaycastHit Hit;

		Vector3 RayStart = new Vector3 (transform.position.x, 5, transform.position.z);

		if (Physics.Raycast (RayStart, Vector3.down * 6, out Hit, 6)) {
			//Debug.DrawRay (RayStart, Vector3.down * 6, Color.red);
			transform.position = Hit.point;
			return true;
		} else {
			return false;
		}
	}
}
