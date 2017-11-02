using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {

	public MultiplayerController MultiplayerController;

	protected void OnCollisionEnter (Collision col) {
		MovementControl P = col.gameObject.GetComponent<MovementControl> ();
		if (P) {
            var PE = Instantiate(Prefabs.DeathPE, P.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            PE.GetComponent<ParticleSystem>().startColor = Player.ChooseColor(P.PlayerName);
            Destroy(PE, 1f);

			MultiplayerController.KillPlayerByString (P.PlayerName);
			Destroy (col.gameObject);
		}

		Egg E = col.gameObject.GetComponent<Egg> ();
		if (E) {
			var PE = Instantiate(Prefabs.DeathPE, E.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
			PE.GetComponent<ParticleSystem>().startColor = Player.ChooseColor(E.PlayerNum);
			Destroy(PE, 1f);

			MultiplayerController.KillPlayerByString (E.PlayerNum);
			Destroy (col.gameObject);
		}

		BallistaBolt B = col.gameObject.GetComponent<BallistaBolt> ();
		if (B) {
			Destroy (B.gameObject);
		}

		Ultimate U = col.gameObject.GetComponent<Ultimate> ();
		if (U) {
			Destroy (U.gameObject);
		}
	}
}
