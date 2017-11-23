using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roost : MonoBehaviour {

	private bool[] PlayersInside = new bool[4];

	public bool IsPlayerOnRoost (int PlayerNumber) {
		if (PlayerNumber < 4 && PlayerNumber >= 0)
			return PlayersInside [PlayerNumber];
		return false;
	}

	void OnTriggerStay(Collider col)
	{
		MovementControl MC = col.GetComponent<MovementControl> ();
		if (MC) {
			PlayersInside [MultiplayerController.PNameToNumber (MC.PlayerName)] = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		MovementControl MC = col.GetComponent<MovementControl> ();
		if (MC) {
			PlayersInside [MultiplayerController.PNameToNumber (MC.PlayerName)] = false;
		}
	}
}
