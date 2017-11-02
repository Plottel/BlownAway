using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

	public static bool[] ActivePlayers = new bool[4];
	public string[] Areas = new string[2];
	public static string Area = "Ballista";
	private int AreaN = 0;
	public Sprite BirdSprite;
	public Sprite EggSprite;
	public GameObject LevelHolder;
	public float GridSpacing = 35;
	private Vector3 DesiredPos;

	private int[] KillsByPlayer = new int[4];

	private Image[] PlayerJoinedIcons = new Image[4];

	private GameObject MenuOwnerImage;
	private Vector3 RelativePosition = new Vector3();

	public int PlayerHoldingMenu = 0;
	public static int Lives = 2;

	public int MaxLives = 4;

	public GameObject B_Area;
	public GameObject B_Lives;

	public GameObject Messages;

	public EventSystem ES;


	// Use this for initialization
	void Start () {
		B_Lives.GetComponentInChildren<Text> ().text = "" + Lives;
		ES = FindObjectOfType<EventSystem> ();
		ES.firstSelectedGameObject = B_Area;
		ToggleArea ();
		ToggleLives ();

		//Get the eggs which show if a player has joined.
		for (int i = 0; i < 4; ++i) {
			PlayerJoinedIcons[i] = GetComponentsInChildren<Image> () [i];
		}

		//Change the eggs colour to the player colours.
		for (int i = 0; i < 4; ++i) {
			PlayerJoinedIcons[i].color = Player.ChooseColor (i);
		}

		//If a player is already active make their egg into a bird (for returning to the menu from a game).
		for (int i = 0; i < 4; ++i) {
			if (ActivePlayers[i])
				PlayerJoinedIcons[i].sprite = BirdSprite;
		}

		MenuOwnerImage = GetComponentsInChildren<Image> () [4].gameObject;
		RelativePosition = MenuOwnerImage.transform.localPosition;
		MoveStar ();
	}

	// Update is called once per frame
	void Update () {

		//For each player, check if their start button has been pressed, and if so add them to the 'active players'.
		for (int p = 0; p <= 3; p++) {
			if (!ActivePlayers [p]) {
				if (CrossPlatformInputManager.GetButtonDown ("P" + (p + 1) + "_Start")) {

					Debug.Log ("Added " + p + " in Update");

					PlayerJoinedIcons[p].sprite = BirdSprite;

					ActivePlayers [p] = true;

					Messages.GetComponent<Text> ().enabled = false;

					ChooseController (p);
				}
			}
			if (ActivePlayers [p]) {
				if (CrossPlatformInputManager.GetButtonDown ("P" + (p + 1) + "_Back")) {
					Debug.Log ("Removed " + p + " in Update");

					PlayerJoinedIcons[p].sprite = EggSprite;

					ActivePlayers [p] = false;

					if (PlayerHoldingMenu == p) {
						ChooseController (p);
					}
				}
			}
		}

	}

	void FixedUpdate() {
		LevelHolder.transform.position = Vector3.MoveTowards (LevelHolder.transform.position, DesiredPos, 0.5f); //+ ((DesiredPos.x-LevelHolder.transform.position.x) / 100));
	}
	/*
	public void ToggleMode() {

		if (Mode == "Normal") {
			Mode = "Tutorial";
			B_Lives.GetComponentInChildren<Text> ().text = "--";
		} else if (Mode == "Tutorial") {
			Mode = "Freeplay";
			B_Lives.GetComponentInChildren<Text> ().text = "--";
		} else {
			Mode = "Normal";
			B_Lives.GetComponentInChildren<Text> ().text = "" + Lives;
		}

		B_Mode.GetComponentInChildren<Text> ().text = Mode;
	}
	*/

	public void ToggleArea() {
		
		if (AreaN >= Areas.Length - 1) {
			AreaN = 0;
			DesiredPos = new Vector3(0, 0, 0);
            LevelHolder.transform.position = new Vector3(0, 0, 0);
        } else {
			AreaN += 1;
			DesiredPos += new Vector3(GridSpacing, 0, 0);
		}

		Area = Areas [AreaN];

		B_Area.GetComponentInChildren<Text> ().text = Area;


	}

	public void ToggleLives() {

		//if (Mode == "Normal") {
			
			if (Lives == MaxLives) {
				Lives = 0;
			} else  {
				Lives += 1;
			}

			B_Lives.GetComponentInChildren<Text> ().text = "" + Lives;

		//}
	}

	public void StartGame() {
		//Go to the scene from here.
		//Choose scene based on area? Ask Matt.
		int n = 0;
		for (int i = 0; i < 4; i++) {
			if (ActivePlayers [i])
				n += 1;
		}

		if (n >= 2) {
			SceneManager.LoadScene ("Game");
		} else {
			Messages.GetComponent<Text> ().enabled = true;
		}
	}

	void ChooseController(int Default) {
		if (!ActivePlayers [PlayerHoldingMenu]) {
			int n = 0;
			for (int i = 0; i < 4; i++) {
				if (ActivePlayers [i])
					n += 1;
			}

			if (n > 0) {
				if (ActivePlayers [Default]) {
					PassControl (Default);
				} else {
					n = -1;
					for (int i = 3; i >= 0; i--) {
						if (ActivePlayers [i])
							n = i;
					}

					PassControl (n);
				}
			}
		}
			
		MoveStar ();
	}

	private void PassControl(int p) {
		StandaloneInputModule SIM = ES.GetComponent<StandaloneInputModule> ();
		SIM.submitButton = ("P" + (p + 1) + "_Jump");
		SIM.horizontalAxis = ("P" + (p + 1) + "_Horizontal");
		SIM.verticalAxis = ("P" + (p + 1) + "_Vertical");
		PlayerHoldingMenu = p;
	}

	private void MoveStar() {
		MenuOwnerImage.transform.SetParent (PlayerJoinedIcons [PlayerHoldingMenu].transform);
		MenuOwnerImage.transform.localPosition = RelativePosition;
	}
}
