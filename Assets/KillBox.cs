using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {

	public MultiplayerController MultiplayerController;

	protected void OnCollisionEnter (Collision col) {
		MovementControl P = col.gameObject.GetComponent<MovementControl> ();
		//Debug.Log ("Collision with: " + P.name);
		if (P) {
			MultiplayerController.KillPlayerByString (P.Player);
			Destroy (col.gameObject);
			//Debug.Log ("Killed Player");
		}
	}
}
