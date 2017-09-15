﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	public void MainMenu() {
		//Go to Main Menu Scene.
		Debug.Log("Main Menu was called.");
	}

	public void Restart() {
		//Restart this scene with the same settings.
		Debug.Log("Restart was called.");
	}
}
