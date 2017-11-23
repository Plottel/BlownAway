using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utility;

public class MultiplayerController : MonoBehaviour {

    /*
	 * #### Most of the player data is done using arrays: each player has a fixed slot in each array,
	 * 		and so the data can be accessed simply by knowing the players number, or vice-versa.
	 */


    public IslandGrid grid;
    public GameObject PlayerPrefab;
    public GameObject EggPrefab;
    public GameObject PlayerIconPrefab;
    public bool[] ActivePlayers = new bool[4];              //if a player joined in the menu (and has lives etc).
    public int StartingLives = 4;//ToDo: make OBSOlETE.
    private int MaxLives = 10;//ToDo: make OBSOlETE.
    private int Paused = -1;                                //Who paused the game.
    public GameObject SpawnPointer;
    public Egg[] Eggs = new Egg[4];
    private SpawnPointer[] SP = new SpawnPointer[4];
    public bool[] InGame = new bool[4];                     //true from when the egg is falling until the player is dead.
    private int[] SpawnTimer = new int[4];
    public GameObject PauseMenuPrefab;
    public GameObject VictoryPrefab;
    private GameObject PauseMenu;
    public EventSystem ES;
    private PlayerIcon[] PlayerIcons = new PlayerIcon[4];
    private int[] DeathCounters = new int[4];
    public Vector3[] TutorialSpawnPositions = new Vector3[4];
    public Text[] HeartNumbers = new Text[4];
    public GameObject BrokenHeart;
    public int GameTime;
    public bool GameEnded;

    public Vector3[] PlayerSpawnPos = new Vector3[4];
    public Text[] TutorialText = new Text[5];

    private Image[,] Stocks = new Image[4, 4];
    private GameMode Game = new GameMode();
    private GameType GT;
    private ScoreMode SM;

    // Use this for initialization
    public void StartManual()
    {
        ES = FindObjectOfType<EventSystem>();

        TutorialText = new Text[5];

        for (int i = 0; i < 5; ++i)
            TutorialText[i] = GetComponentsInChildren<Text>()[i];

        //If spawn positions are not set, set them to these defaults.
        if (PlayerSpawnPos == new Vector3[4]) {
            PlayerSpawnPos[0] = new Vector3(0, 5, -10);
            PlayerSpawnPos[0] = new Vector3(10, 5, -10);
            PlayerSpawnPos[0] = new Vector3(10, 5, 0);
            PlayerSpawnPos[0] = new Vector3(0, 5, -0);
        }

        // Setup tutorial spawn positions
        TutorialSpawnPositions[0] = new Vector3(-3.15f, 5, -12.19f);
        TutorialSpawnPositions[1] = new Vector3(4.18f, 5, -12.19f);
        TutorialSpawnPositions[2] = new Vector3(4.18f, 5, -19.96f);
        TutorialSpawnPositions[3] = new Vector3(-3.78f, 5, -19.96f);


        //Get the settings chosen in the main menu.
        ActivePlayers = MainMenu.ActivePlayers;
        StartingLives = MainMenu.Lives;

        if (StartingLives == 0)
            StartingLives = -1;

        bool n = true;
        for (int i = 0; i < 4; i++) {
            if (ActivePlayers[i] == true) {
                n = false;
                break;
            }
        }
        if (n)
        {
            ActivePlayers[0] = true;
            ActivePlayers[1] = true;
            ActivePlayers[3] = true;
            ActivePlayers[2] = true;
        }


        //Disable the tutorial text if not in tutorial mode.
        if (MainMenu.Area != "Tutorial")
        {
            TutorialText[0].enabled = false;
            TutorialText[1].enabled = false;
            TutorialText[2].enabled = false;
            TutorialText[3].enabled = false;
            TutorialText[4].enabled = false;
        }
        else
        {
            TutorialText[0].enabled = false;
            TutorialText[1].enabled = false;
            TutorialText[2].enabled = false;
            TutorialText[3].enabled = false;
            TutorialText[4].enabled = true;
            StartingLives = 3;
        }


        if (StartingLives > MaxLives) {
            StartingLives = MaxLives;
        }


        //Create and gives lives to the players in this match, and create and setup their icon.
        for (int i = 0; i <= 3; i++) {
            if (ActivePlayers[i] == true) {
                 Game.Lives[i] = StartingLives;
                StartSpawn(i);
            }
        }

        //NEW GOOD lives system.
        Image Heart;
        for (int i = 0; i <= 3; i++) {
            Heart = GetComponentsInChildren<Image>()[i];
            Heart.color = Player.ChooseColor(i);
            HeartNumbers[i] = Heart.GetComponentInChildren<Text>();
            HeartNumbers[i].text = "" + StartingLives;

            if (!ActivePlayers[i]) {
                Heart.enabled = false;
            }
        }
        int score = 4;

        if(SM == ScoreMode.Time)
            Game.StartGame(GT, SM, ActivePlayers, GameTime);
        if(SM == ScoreMode.Score)
            Game.StartGame(GT, SM, ActivePlayers, maxScore: score);
        if(SM == ScoreMode.Lives)
            Game.StartGame(GT, SM, ActivePlayers, lives: StartingLives);
        UpdateStockGraphics();
    }

    //Should probably change to Update and use Delta.Time
    void FixedUpdate() {
        ContinueSpawn();
    }

    // Update is called once per frame
    void Update() {

        //Check if any of the players in the game pressed start, and pause (or unpause) if they did.
        for (int p = 0; p <= 3; p++) {
            if (ActivePlayers[p]) {
                if (CrossPlatformInputManager.GetButtonDown("P" + (p + 1) + "_Start")) {
                    if (Paused == -1 && !GameEnded) {
                        Paused = p;
                        Time.timeScale = 0;
                        PauseMenu = Instantiate(PauseMenuPrefab, transform.parent);

                        PauseMenu.GetComponentInChildren<Button>().Select();

                        StandaloneInputModule SIM = ES.GetComponent<StandaloneInputModule>();
                        SIM.submitButton = ("P" + (p + 1) + "_Attack");
                        SIM.horizontalAxis = ("P" + (p + 1) + "_Horizontal");
                    } else if (Paused == p) {
                        Paused = -1;
                        Time.timeScale = 1;
                        Destroy(PauseMenu);
                    }
                }

            }

            if (MainMenu.Area == "Tutorial") {
                if (CrossPlatformInputManager.GetAxis("P" + (p + 1) + "_Horizontal") != 0)
                    TutorialText[0].enabled = false;
                if (CrossPlatformInputManager.GetAxis("P" + (p + 1) + "_RotationX") != 0)
                    TutorialText[1].enabled = false;
                if (CrossPlatformInputManager.GetButtonDown("P" + (p + 1) + "_Jump"))
                    TutorialText[2].enabled = false;
                if (CrossPlatformInputManager.GetButtonDown("P" + (p + 1) + "_AttackDirect"))
                    TutorialText[3].enabled = false;
            }
        }
    }

    //Show or hide stocks based on the number of lives the player has.
    private void UpdateStockGraphics() {

        //Update Corner info.
        for (int p = 0; p <= 3; p++) {
            if (ActivePlayers[p])
                HeartNumbers[p].text = "" + Game.Lives[p];
        }
    }
	
	//Resets spawn timers, and manages the spawnpointers (or eggs).
				//CREATE EGG
    private void StartSpawn(int PlayerNum) {
        if (SP[PlayerNum] == null) {
	        SP[PlayerNum] = Instantiate(SpawnPointer, grid.MidCell.transform.position, Quaternion.Euler(new Vector3(90, 0, 0))).GetComponent<SpawnPointer>();
			
	        SP[PlayerNum].PlayerNum = "P" + (PlayerNum + 1);

	        SP[PlayerNum].GetComponent<SpriteRenderer>().color = Player.ChooseColor(PlayerNum);
        }

		//Create a floating egg as well (to replace the 'spawnpointer').
		GameObject EP = Instantiate(EggPrefab, grid.MidCell.transform.position + new Vector3(0, 4, 0), Quaternion.Euler(new Vector3(270, 0, 0)));
		Eggs[PlayerNum] = EP.GetComponent<Egg> ();
		Eggs[PlayerNum].FallingMode = false;
		Eggs[PlayerNum].PlayerNum = "P" + (PlayerNum + 1);
		Eggs[PlayerNum].GetComponent<MeshRenderer>().materials[1].color = Player.ChooseColor(PlayerNum);
		if (MainMenu.Area == "Tutorial") {
			EP.transform.position = TutorialSpawnPositions [PlayerNum];
			SpawnTimer [PlayerNum] = 0;
		} else
			SpawnTimer [PlayerNum] = 300;

        SP[PlayerNum].transform.position = grid.MidCell.transform.position;
		SP[PlayerNum].Target = EP.transform;
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

    //Starts the egg falling, (starts checking it hit the ground?).
    public void DropEgg(int PlayerNum)
    {
        Eggs[PlayerNum].GetComponent<Rigidbody>().useGravity = true;
        Eggs[PlayerNum].FallingMode = true; // Used for Debugging ONLY
        InGame[PlayerNum] = true;
    }

    //The egg tells the MC that it has broken, so the pointers etc should be attatched to the player now not it.
    public void EggBroke(string PlayerName)
    {
        if (Eggs[PNameToNumber(PlayerName)] != null)
        {
            CreatePlayer(PNameToNumber(PlayerName));
            Eggs[PNameToNumber(PlayerName)] = null;
        }
    }

    //The actual creation of the bird-man, ie from the egg.
    //Called (triggered) by the egg, and creates a player at (just above) the eggs position.
    private void CreatePlayer(int PlayerNum)
    {

        GameObject P = Instantiate(PlayerPrefab); // ToDo: Change to Load();
        P.GetComponent<MovementControl>().PlayerName = "P" + (PlayerNum + 1);
        P.GetComponent<Transform>().position = SP[PlayerNum].transform.position;

        PlayerIcons[PlayerNum] = Instantiate(PlayerIconPrefab, transform).GetComponent<PlayerIcon>(); //Needs to be in the canvas.
        PlayerIcons[PlayerNum].PlayerNumber = PlayerNum + 1;
        PlayerIcons[PlayerNum].SetHealth = 0;
        PlayerIcons[PlayerNum].UseName = false;
        PlayerIcons[PlayerNum].StopAt100 = false;

        PlayerIcons[PlayerNum].Target = P.transform;

        SP[PlayerNum].Target = P.transform;
        InGame[PlayerNum] = true;
    }

    public void CheckAndEndGame() {
        //int DeadPlayers = 0;
        //int NotDead = -1;
        //for (int i = 0; i < 4; ++i) {
        //	if (Game.Lives [i] == 0) {
        //		DeadPlayers += 1;
        //	} else {
        //		NotDead = i;
        //	}
        //}
        //if (DeadPlayers >= 3) {
        //	GameObject V = Instantiate (VictoryPrefab, transform.parent);
        //          if (NotDead >= 0)
        //          {
        //              V.GetComponent<Text>().color = Player.ChooseColor(NotDead);
        //              StandaloneInputModule SIM = ES.GetComponent<StandaloneInputModule>();
        //              SIM.submitButton = ("P" + (NotDead + 1) + "_Attack");
        //              SIM.horizontalAxis = ("P" + (NotDead + 1) + "_Horizontal");
        //          }
        //          else
        //          {
        //              V.GetComponent<Text>().text = "Draw!";
        //          }
        //	V.GetComponentInChildren<Button> ().Select();
        //}

        int winner = Game.CheckGameEnd();
        if (winner == -1)
        {
            return;
        }
        else if (winner == 4)
        {
            //Draw screen
            GameEnded = true;
            Time.timeScale = 0;
            GameObject V = Instantiate(VictoryPrefab, transform.parent);
            StandaloneInputModule SIM = ES.GetComponent<StandaloneInputModule>();
            SIM.submitButton = ("P" + (MainMenu.PlayerHoldingMenu + 1) + "_Attack");
            SIM.horizontalAxis = ("P" + (MainMenu.PlayerHoldingMenu + 1) + "_Horizontal");
            V.GetComponent<Text>().text = "Draw!";
        }
        else
        {
            //Victory screen for winner
            GameEnded = true;
            Time.timeScale = 0;
            GameObject V = Instantiate(VictoryPrefab, transform.parent);
            V.GetComponent<Text>().color = Player.ChooseColor(winner);
            StandaloneInputModule SIM = ES.GetComponent<StandaloneInputModule>();
            SIM.submitButton = ("P" + (winner + 1) + "_Attack");
            SIM.horizontalAxis = ("P" + (winner + 1) + "_Horizontal");
            MainMenu.PlayerHoldingMenu = winner;
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
		Game.Lives [PNumber] -= 1;
		if (Game.Lives [PNumber] != 0) {
			Instantiate (BrokenHeart, HeartNumbers [PNumber].transform.parent).GetComponent<Image>().color = Player.ChooseColor(PNumber);
			StartSpawn (PNumber);
			//Debug.Log ("Created in Int");
		} else
        {
			ActivePlayers [PNumber] = false;
			HeartNumbers [PNumber].GetComponentInParent<Image> ().sprite = BrokenHeart.GetComponent<Image> ().sprite;
			HeartNumbers [PNumber].text = "";
			CheckAndEndGame ();
            Destroy(SP[PNumber].gameObject, 1f);
            SP[PNumber] = null;
        }
		if (PlayerIcons[PNumber] != null)
        	Destroy(PlayerIcons[PNumber].gameObject);
        InGame[PNumber] = false;
		UpdateStockGraphics ();
        //update stats
        Game.UpdateStats(PNumber,KillType.P1);
        //checkifgameover
        Game.CheckGameEnd();
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