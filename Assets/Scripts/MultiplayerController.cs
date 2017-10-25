using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MultiplayerController : MonoBehaviour {
	public Grid grid;
	public GameObject PlayerPrefab;
	public GameObject EggPrefab;
	public GameObject PlayerIconPrefab;
	private bool[] ActivePlayers = new bool[4];
	public int StartingLives = 4;
	private int MaxLives = 4;
	private int[] Lives = new int[4];
	private int Paused = -1;
	public GameObject SpawnPointer;
	private Egg[] Eggs = new Egg[4];
	private SpawnPointer[] SP = new SpawnPointer[4];
    private bool[] InGame = new bool[4]; //true from when the egg is falling until the player is dead.
	private int[] SpawnTimer = new int[4];
	public GameObject PauseMenuPrefab;
	private GameObject PauseMenu;
	public EventSystem ES;
	private PlayerIcon[] PlayerIcons = new PlayerIcon[4];

	public Vector3[] PlayerSpawnPos = new Vector3[4];
	public Text[] TutorialText = new Text[5];

	private Image[,] Stocks = new Image[4,4];

	// Use this for initialization
	public void StartManual()
	{
		ES = FindObjectOfType<EventSystem> ();

		TutorialText = new Text[5];

		for (int i = 0; i < 5; ++i)
			TutorialText [i] = GetComponentsInChildren<Text>()[i];

		//If spawn positions are not set, set them to these defaults.
		if (PlayerSpawnPos == new Vector3[4]) {
			PlayerSpawnPos[0] = new Vector3 (0, 5, -10);
			PlayerSpawnPos[0] = new Vector3 (10, 5, -10);
			PlayerSpawnPos[0] = new Vector3 (10, 5, 0);
			PlayerSpawnPos[0] = new Vector3 (0, 5, -0);
		}

		//Get the settings chosen in the main menu.
		ActivePlayers = MainMenu.ActivePlayers;
		StartingLives = MainMenu.Lives;

		bool n = true;
		for( int i = 0; i < 4; i++) {
			if (ActivePlayers [i] == true) {
				n = false;
				break;
			}
		}
		if (n)
        {
            ActivePlayers[0] = true;
            ActivePlayers[1] = true;
            ActivePlayers [3] = true;
			ActivePlayers [2] = true;
		}
		

		//Disable the tutorial text if not in tutorial mode.
		if (MainMenu.Mode != "Tutorial") {			
			TutorialText[0].enabled = false;
			TutorialText[1].enabled = false;
			TutorialText[2].enabled = false;
			TutorialText[3].enabled = false;
			TutorialText[4].enabled = false;
		}

		if (MainMenu.Mode != "Normal") {
			StartingLives = -1;
		}

		if (StartingLives > MaxLives) {
			StartingLives = MaxLives;
		}

		//Put all the stocks in a list and change their visibility depending on which players are present.
		int k = 4;
		for (int i = 0; i <= 3; i++) {
			int j = 0;
			Stocks [i, j] = GetComponentsInChildren<Image> () [i * 4 + j];
			Stocks [i, j].enabled = ActivePlayers [i];
			j += 1;
			k += 1;
			Stocks [i, j] = GetComponentsInChildren<Image> () [i * 4 + j];
			Stocks [i, j].enabled = ActivePlayers [i];
			j += 1;
			k += 1;
			Stocks [i, j] = GetComponentsInChildren<Image> () [i * 4 + j];
			Stocks [i, j].enabled = ActivePlayers [i];
			j += 1;
			k += 1;
			Stocks [i, j] = GetComponentsInChildren<Image> () [i * 4 + j];
			Stocks [i, j].enabled = ActivePlayers [i];
			j += 1;
			k += 1;
		}



		//Create and gives lives to the players in this match, and create and setup their icon.
		for (int i = 0; i <= 3; i++) {
			if (ActivePlayers [i] == true) {
				Lives [i] = StartingLives;
				StartSpawn(i);
			}
		}

		UpdateStockGraphics ();
	}

	//Should probably change to Update and use Delta.Time
	void FixedUpdate() {
		ContinueSpawn ();
	}

	// Update is called once per frame
	void Update () {

		//Check if any of the players in the game pressed start, and pause (or unpause) if they did.
		for (int p = 0; p <= 3; p++) {
			if (ActivePlayers [p]) {
				if (CrossPlatformInputManager.GetButtonDown ("P" + (p + 1) + "_Start")) {
					if (Paused == -1) {
						Paused = p;
						Time.timeScale = 0;
						PauseMenu = Instantiate (PauseMenuPrefab, transform.parent);

						PauseMenu.GetComponentInChildren<Button> ().Select();

						StandaloneInputModule SIM = ES.GetComponent<StandaloneInputModule> ();
						SIM.submitButton = ("P" + (p + 1) + "_Jump");
						SIM.horizontalAxis = ("P" + (p + 1) + "_Horizontal");
					} else if (Paused == p) {
						Paused = -1;
						Time.timeScale = 1;
						Destroy (PauseMenu);
					}
				}
			}

			if (MainMenu.Mode == "Tutorial") {
				if (CrossPlatformInputManager.GetAxis("P" + (p + 1) + "_Horizontal") != 0)
					TutorialText[0].enabled = false;
				if (CrossPlatformInputManager.GetAxis("P" + (p + 1) + "_RotationX") != 0)
					TutorialText[1].enabled = false;
				if (CrossPlatformInputManager.GetButtonDown ("P" + (p + 1) + "_Jump"))
					TutorialText[2].enabled = false;
				if (CrossPlatformInputManager.GetButtonDown ("P" + (p + 1) + "_AttackDirect"))
					TutorialText[3].enabled = false;
			}
		}
	}

	//Show or hide stocks based on the number of lives the player has.
	private void UpdateStockGraphics() {
		for (int p = 0; p <= 3; p++) {
			if (ActivePlayers[p]) {
				for (int i = 0; i <= 3; i++) {
					Stocks [p, i].enabled = false;
				}
				for (int i = 0; i < Lives[p]; i++) {
					Stocks [p, i].enabled = true;
				}
			}
		}
	}

	//Starts the egg falling, (starts checking it hit the ground?).
	public void DropEgg(int PlayerNum) {
		Eggs [PlayerNum].GetComponent<Rigidbody> ().useGravity = true;
		//Eggs [PlayerNum].gameObject.GetComponent<Collider> ().isTrigger = false;
		Eggs [PlayerNum].FallingMode = true;
		InGame [PlayerNum] = true;
	}

	//The egg tells the MC that it has broken, so the pointers etc should be attatched to the player now not it.
	public void EggBroke(string PlayerName) {
		if (Eggs [PNameToNumber (PlayerName)] != null) {
			CreatePlayer (PNameToNumber (PlayerName));
			Eggs [PNameToNumber (PlayerName)] = null;
		}
	}

	//The actual creation of the bird-man, ie from the egg.
	//Called (triggered) by the egg, and creates a player at (just above) the eggs position.
	private void CreatePlayer(int PlayerNum) {

		GameObject P = Instantiate (PlayerPrefab);
		P.GetComponent<MovementControl> ().PlayerName = "P" + (PlayerNum + 1);
		P.GetComponent<Transform> ().position = SP [PlayerNum].transform.position + new Vector3(0, 0, 0);

		PlayerIcons [PlayerNum] = Instantiate (PlayerIconPrefab, transform).GetComponent<PlayerIcon> ();
		PlayerIcons [PlayerNum].PlayerNumber = PlayerNum + 1;
		PlayerIcons [PlayerNum].SetHealth = 0;
		PlayerIcons [PlayerNum].UseName = false;
		PlayerIcons [PlayerNum].StopAt100 = false;

		PlayerIcons [PlayerNum].Target = P.transform;

        //Destroy (SP [PlayerNum].gameObject);
        SP[PlayerNum].Target = P.transform;
        InGame[PlayerNum] = true;
	}

	//Resets spawn timers, and manages the spawnpointers (or eggs).
				//CREATE EGG
    private void StartSpawn(int PlayerNum) {
        if (SP[PlayerNum] == null) {
        SP[PlayerNum] = Instantiate(SpawnPointer, grid.MidCell.transform.position, Quaternion.Euler(new Vector3(90, 0, 0))).GetComponent<SpawnPointer>();
        //SP [Player].transform.rotation = ;
		
		
		
        SP[PlayerNum].PlayerNum = "P" + (PlayerNum + 1);

        SP[PlayerNum].GetComponent<SpriteRenderer>().color = Player.ChooseColor(PlayerNum);
        }

		//Create a floating egg as well (to replace the 'spawnpointer').
		GameObject EP = Instantiate(EggPrefab, grid.MidCell.transform.position + new Vector3(0, 4, 0), Quaternion.Euler(new Vector3(90, 0, 0)));
		Eggs[PlayerNum] = EP.GetComponent<Egg> ();
		Eggs[PlayerNum].FallingMode = false;
		Eggs[PlayerNum].PlayerNum = "P" + (PlayerNum + 1);
		Eggs[PlayerNum].GetComponent<MeshRenderer>().materials[1].color = Player.ChooseColor(PlayerNum);


        SP[PlayerNum].transform.position = grid.MidCell.transform.position;
		SP[PlayerNum].Target = EP.transform;
        SpawnTimer [PlayerNum] = 300;
	}

	//Check if the spawnpointer should change into the player (or egg), either from a button-press or the timer.
	//Also updates the shadow-circles (still spawnpointers atm), and SHOULD check if the pointer is above an island or the killbox
	private void ContinueSpawn() {
		
		for (int p = 0; p < 4; p++) {
			if (InGame [p] != true && SP[p] != null) {
				if (SP [p].ManualUpdate ()) {
					if (CrossPlatformInputManager.GetButtonDown ("P" + (p + 1) + "_AttackDirect"))
						DropEgg(p);
				}
				Eggs [p].ManualUpdate ();
				if (SpawnTimer [p] > 0)
					SpawnTimer [p] -= 1;
				else
					DropEgg (p);
			}
            else
            {
                if (SP[p] != null)
                    SP[p].ManualUpdate();
            }  
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

		KillPlayerByInt (PNumber);
	}

	public void KillPlayerByInt (int PNumber) {
		Lives [PNumber] -= 1;
		if (Lives [PNumber] != 0) {
			StartSpawn (PNumber);
			//CreatePlayer (PNumber);
			Debug.Log ("Created in Int");
		} else
        {
            Destroy(SP[PNumber].gameObject, 1f);
            SP[PNumber] = null;
        }
        Destroy(PlayerIcons[PNumber].gameObject);
        InGame[PNumber] = false;
		UpdateStockGraphics ();
	}

	public static int PNameToNumber (string PName) {
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
		return PNumber;
	}

	public static string PNumToName (int PNumber) {
		return "P" + (PNumber + 1);
	}
}
