using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {

	public MultiplayerController MultiplayerController;

	protected void OnCollisionEnter (Collision col) {
		MovementControl P = col.gameObject.GetComponent<MovementControl> ();
		//Debug.Log ("Collision with: " + P.name);
		if (P) {
            var PE = Instantiate(Prefabs.DeathPE, P.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));

            Destroy(PE, 1f);

			MultiplayerController.KillPlayerByString (P.PlayerName);
			Destroy (col.gameObject);
			//Debug.Log ("Killed Player");
		}
	}
}
