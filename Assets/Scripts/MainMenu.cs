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
	public static string Area = "Tutorial";
	private int AreaN = 0;
	public Sprite BirdSprite;
	public Sprite EggSprite;
	public GameObject LevelHolder;
	public float GridSpacing = 35;
	private Vector3 DesiredPos;
	private Vector3 ZeroPos;
	public Vector3 StagePos = new Vector3(0, 2, 1.2f);
	private GameObject[] Players = new GameObject[4];

	private int[] KillsByPlayer = new int[4]; //Used for stats, deathmatch etc later (not currently).

	private Image[] PlayerJoinedIcons = new Image[4];

	private GameObject MenuOwnerImage;
	private Vector3 RelativePosition = new Vector3();

	public static int PlayerHoldingMenu = 0;
	public static int Lives = 2;

	public int MaxLives = 10;

	public GameObject B_Area;
	public GameObject B_Lives;

	public GameObject Messages;

	public EventSystem ES;

	public GameObject PlayerPrefab;


	// Use this for initialization
	void Start () {
		B_Lives.GetComponentInChildren<Text> ().text = "" + Lives;
		ES = FindObjectOfType<EventSystem> ();
		ES.firstSelectedGameObject = B_Area;
        B_Area.GetComponentInChildren<Text>().text = Area;
        ToggleLives();
        ToggleLives();
        ToggleLives();
        ToggleLives();
        
		DesiredPos = LevelHolder.transform.position;
		ZeroPos = LevelHolder.transform.position;

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
					Messages.GetComponent<Text> ().enabled = false; // Not enough players error.
					ChooseController (p);

					//Spawn players on the stage.
					Players[p] = Instantiate (PlayerPrefab, StagePos, new Quaternion());
					Players [p].GetComponent<MovementControl> ().PlayerName = "P" + (p + 1);
					Players [p].GetComponent<MovementControl> ().InMenu = true;
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

					//Destroy players on the stage.
					Destroy(Players[p]);
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
			DesiredPos = ZeroPos;
			LevelHolder.transform.position = ZeroPos;
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

        if (Lives == 0)
            B_Lives.GetComponentInChildren<Text>().text = "Infinite";
        else
            B_Lives.GetComponentInChildren<Text>().text = "" + Lives;

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
		ChangeTextColour ();
		MoveStar ();
	}

	private void PassControl(int p) {
		StandaloneInputModule SIM = ES.GetComponent<StandaloneInputModule> ();
		SIM.submitButton = ("P" + (p + 1) + "_AttackDirect");
		SIM.horizontalAxis = ("P" + (p + 1) + "_Horizontal");
		SIM.verticalAxis = ("P" + (p + 1) + "_Vertical");
		PlayerHoldingMenu = p;
	}

	private void MoveStar() {
		MenuOwnerImage.transform.SetParent (PlayerJoinedIcons [PlayerHoldingMenu].transform);
		MenuOwnerImage.transform.localPosition = RelativePosition;
	}

	private void ChangeTextColour() {
		foreach (Text T in GetComponentsInChildren<Text>()) {
			T.color = Player.ChooseColor (PlayerHoldingMenu);
		}
		//MenuOwnerImage.transform.SetParent (PlayerJoinedIcons [PlayerHoldingMenu].transform);
		//MenuOwnerImage.transform.localPosition = RelativePosition;
	}
}
