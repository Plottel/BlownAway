﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class MultiplayerController : MonoBehaviour {

	public GameObject PlayerPrefab;
	public GameObject StockPrefab;
	private bool[] ActivePlayers = new bool[4];
	public int StartingLives = 4;
	private int MaxLives = 4;
	private int[] Lives = new int[4];

	private Image[,] Stocks = new Image[4,4];

	// Use this for initialization
	void Start () {
		if (StartingLives > MaxLives) {
			StartingLives = MaxLives;
		}

		for (int i = 0; i <= 3; i++) {
			Lives [i] = 0;
		}

		int k = 4;
		for (int i = 0; i <= 3; i++) {
			int j = 0;
			Stocks [i,j] = Instantiate (StockPrefab).GetComponent<Image>();
			Stocks [i,j].enabled = false;
			j += 1;
			k += 1;
			Stocks[i,j] = Instantiate (StockPrefab).GetComponent<Image>();
			Stocks [i,j].enabled = false;
			j += 1;
			k += 1;
			Stocks[i,j] = Instantiate (StockPrefab).GetComponent<Image>();
			Stocks [i,j].enabled = false;
			j += 1;
			k += 1;
			Stocks[i,j] = Instantiate (StockPrefab).GetComponent<Image>();
			Stocks [i,j].enabled = false;
			j += 1;
			k += 1;
		}

	}

	// Update is called once per frame
	void Update () {

		for (int p = 0; p <= 3; p++) {
			if (!ActivePlayers [p]) {
				if (ActivePlayers [p] = CrossPlatformInputManager.GetButtonDown ("P" + (p+1) + "_Start")) {

					CreatePlayer (p);
					Debug.Log ("Created in Update");

					GetComponentsInChildren<Image> () [p].enabled = false;

					Lives [p] = StartingLives;
					ActivePlayers [p] = true;

					UpdateStockGraphics ();
					/*
					for (int i = 0; i <= 3; i++) {
						Stocks [p, i].enabled = true;
					}
					*/

				}
			}
		}
	}

	//Show or hide stocks based on the number of lives the player has.
	private void UpdateStockGraphics() {
		for (int p = 0; p <= 3; p++) {
			for (int i = 0; i <= 3; i++) {
				Stocks [p, i].enabled = false;
			}
			for (int i = 0; i < Lives[p]; i++) {
				Stocks [p, i].enabled = true;
			}
		}
	}

	//CHANGE THE SPAWN POSITION OF PLAYERS HERE (for now).
	private void CreatePlayer(int Player) {
		Debug.Log ("Created: " + Player);
		if (Player == 0) {
			GameObject P = Instantiate (PlayerPrefab);
			P.GetComponent<MovementControl> ().Player = "P1";
			P.GetComponent<Transform> ().position = new Vector3 (0, 5, -10);
		} else if (Player == 1) {
			GameObject P = Instantiate (PlayerPrefab);
			P.GetComponent<MovementControl> ().Player = "P2";
			P.GetComponent<Transform> ().position = new Vector3 (10, 5, -10);
		} else if (Player == 2) {
			GameObject P = Instantiate (PlayerPrefab);
			P.GetComponent<MovementControl> ().Player = "P3";
			P.GetComponent<Transform> ().position = new Vector3 (10, 5, 0);
		} else if (Player == 3) {
			GameObject P = Instantiate (PlayerPrefab);
			P.GetComponent<MovementControl> ().Player = "P4";
			P.GetComponent<Transform> ().position = new Vector3 (0, 5, -0);
		}
	}

	public void KillPlayerByString (string PName) {
		int PNumber = 0;
		if (PName == "P1") {
			PNumber = 0;
		} else if (PName == "P2") {
			PNumber = 1;
		} else if (PName == "P3") {
			PNumber = 2;
		} else if (PName == "P4") {
			PNumber = 3;
		}
		Lives [PNumber] -= 1;
		if (Lives [PNumber] != 0) {
			CreatePlayer (PNumber);
			Debug.Log ("Created in String");
		}
		UpdateStockGraphics ();
		Debug.Log ("Name: " + PName + ", Number: " + PNumber + ", Lives: " + Lives [PNumber]);
	}

	public void KillPlayerByInt (int PNumber) {
		Lives [PNumber] -= 1;
		if (Lives [PNumber] != 0) {
			CreatePlayer (PNumber);
			Debug.Log ("Created in Int");
		}
		UpdateStockGraphics ();
	}

}
