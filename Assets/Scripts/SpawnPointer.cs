using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SpawnPointer : MonoBehaviour {

	public bool OverTile = false;
	public string PlayerNum = "P3";
    public float MovementSpeed = 0.5f;

	public bool ManualUpdate () {

		float h = CrossPlatformInputManager.GetAxis(PlayerNum + "_Horizontal");
		float v = CrossPlatformInputManager.GetAxis(PlayerNum + "_Vertical");

        transform.position += new Vector3((h * 0.3f * MovementSpeed), 0, (v * 0.3f * MovementSpeed));

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
