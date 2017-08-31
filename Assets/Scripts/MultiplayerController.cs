using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class MultiplayerController : MonoBehaviour {

	public GameObject PlayerPrefab;
	private bool Player1 = false;
	private bool Player2 = false;
	private bool Player3 = false;
	private bool Player4 = false;
	/*
	public GameObject Player1Join;
	public GameObject Player2Join;
	public GameObject Player3Join;
	public GameObject Player4Join;
	*/

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (!Player1) {
			if (Player1 = CrossPlatformInputManager.GetButtonDown ("P1_Start")) {
				GameObject P = Instantiate (PlayerPrefab);
				P.GetComponent<MovementControl> ().Player = "P1";
				P.GetComponent<Transform> ().position = new Vector3 (0, 5, -10);

				GetComponentsInChildren<Image> () [0].enabled = false;
			}
		}
		if (!Player2) {
			if (Player2 = CrossPlatformInputManager.GetButtonDown ("P2_Start")) {
				GameObject P = Instantiate (PlayerPrefab);
				P.GetComponent<MovementControl> ().Player = "P2";
				P.GetComponent<Transform> ().position = new Vector3 (10, 5, -10);

				GetComponentsInChildren<Image> () [1].enabled = false;
			}
		}
		if (!Player3) {
			if (Player3 = CrossPlatformInputManager.GetButtonDown ("P3_Start")) {
				GameObject P = Instantiate (PlayerPrefab);
				P.GetComponent<MovementControl> ().Player = "P3";
				P.GetComponent<Transform> ().position = new Vector3 (10, 5, 0);

				GetComponentsInChildren<Image> () [2].enabled = false;
			}
		}
		if (!Player4) {
			if (Player4 = CrossPlatformInputManager.GetButtonDown ("P4_Start")) {
				GameObject P = Instantiate (PlayerPrefab);
				P.GetComponent<MovementControl> ().Player = "P4";
				P.GetComponent<Transform> ().position = new Vector3 (0, 5, -0);

				GetComponentsInChildren<Image> () [3].enabled = false;
			}
		}
	}
}
