using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public static bool[] ActivePlayers = new bool[4];
	public static string Area = "Normal";
	public static string Mode = "Normal";
	public static int Lives = 2;

	public int MaxLives = 4;

	public GameObject B_Mode;
	public GameObject B_Area;
	public GameObject B_Lives;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//For each player, check if their start button has been pressed, and if so add them to the 'active players'.
		for (int p = 0; p <= 3; p++) {
			if (!ActivePlayers [p]) {
				if (CrossPlatformInputManager.GetButtonDown ("P" + (p + 1) + "_Start")) {

					Debug.Log ("Added " + p + " in Update");

					GetComponentsInChildren<Image> () [p].enabled = false;

					ActivePlayers [p] = true;
				}
			}
			if (ActivePlayers [p]) {
				if (CrossPlatformInputManager.GetButtonDown ("P" + (p + 1) + "_Back")) {
					Debug.Log ("Removed " + p + " in Update");

					GetComponentsInChildren<Image> () [p].enabled = true;

					ActivePlayers [p] = false;
				}
			}
		}
	}

	public void ToggleMode() {
		if (Mode == "Normal") {
			Mode = "Tutorial";
			Lives = -1;
			B_Lives.GetComponentInChildren<Text> ().text = "infinite";
		} else if (Mode == "Tutorial") {
			Mode = "Freeplay";
			Lives = -1;
			B_Lives.GetComponentInChildren<Text> ().text = "infinite";
		} else {
			Mode = "Normal";
			Lives = 1;
			B_Lives.GetComponentInChildren<Text> ().text = "1";
		}

		B_Mode.GetComponentInChildren<Text> ().text = Mode;
	}

	public void ToggleArea() {
		if (Area == "Normal") {
			Area = "Insane";
		} else if (Area == "Insane") {
			Area = "Ballista";
		} else if (Area == "Ballista") {
			Area = "Random";
		} else {
			Area = "Normal";
		}

		B_Area.GetComponentInChildren<Text> ().text = Area;
	}

	public void ToggleLives() {
		if (Lives == MaxLives) {
			Lives = 1;
		} else  {
			Lives += 1;
		}

		string Temp = "" + Lives;

		B_Lives.GetComponentInChildren<Text> ().text = Temp;
	}

	public void StartGame() {
		//Go to the scene from here.
		//Choose scene based on area? Ask Matt.
		Debug.Log("Game Started. Right?");
		SceneManager.LoadScene ("Game");
	}
}
