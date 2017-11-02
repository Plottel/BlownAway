using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Corner : MonoBehaviour {

	public Text Lives;
	public Text Health;
	public Text UltimateCharge;

	public Player TargetPlayer;

	// Use this for initialization
	public void ManualStart () {
		Lives = GetComponentsInChildren<Text> () [0];
		Health = GetComponentsInChildren<Text> () [1];
		UltimateCharge = GetComponentsInChildren<Text> () [2];

		Lives.text = "";
		Health.text = "";
		UltimateCharge.text = "";

		if (TargetPlayer != null) {
			SetColours (MultiplayerController.PNameToNumber(TargetPlayer.GetComponent<MovementControl> ().PlayerName));
		}
	}

	void Update () {
		if (TargetPlayer != null) {
			SetHealth (TargetPlayer.Health);
			//SetUltiCharge(TargetPlayer.UltimateCharge);
		}
	}

	public void SetColours(int PlayerNumber) {
		Lives.color = Player.ChooseColor (PlayerNumber);
		Health.color = Player.ChooseColor (PlayerNumber);
		UltimateCharge.color = Player.ChooseColor (PlayerNumber);
	}

	public void SetLives(int Value) {
		if (Value == 0) {
			Lives.text = "DEAD";
		} else if (Value > 0) {
			Lives.text = "x" + Value;
		} else {
			Lives.text = "" + Value;
		}
	}

	public void SetHealth(float Value) {
		Health.text = Mathf.Ceil (Value) + "%";
	}

	public void SetUltiCharge(float Value) {
		UltimateCharge.text = Mathf.Ceil (Value) + "%";
	}
}
