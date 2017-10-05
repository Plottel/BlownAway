using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour {

	public Transform Target;
	public bool ChangeText = true;
	public bool RoundUp = true;
	public bool StopAt100 = true;
	public bool UseName = false;
	public string Name;
	public int PlayerNumber;

	private Text TextObject;
	private float Health = 0;


	void Start() {
		TextObject = GetComponent<Text> ();
		SetColour (PlayerNumber);
	}

	// Update is called once per frame
	void Update () {
		Vector3 wantedPos = Camera.main.WorldToScreenPoint (Target.position);
		transform.position = wantedPos;
		SetHealth = Target.GetComponent<Player> ().Health;

		if (ChangeText) {
			if (UseName)
				TextObject.text = Name + " " + Health + "%";
			else
				TextObject.text = "P" + PlayerNumber + " - " + Health + "%";
				
		}
	}

	public float SetHealth {
		set {
			float InputHealth = value;
			if (InputHealth < 0)
				InputHealth = 0;
			if (StopAt100) {
				if (InputHealth > 100)
					InputHealth = 100;
			}
			if (RoundUp) {
				InputHealth = Mathf.Ceil (InputHealth);
			}
			Health = InputHealth;
		}
	}

	public void SetColour(int PlayerNumber) {
		switch (PlayerNumber) {
		case 1:
			TextObject.color = new Color (1, 0, 0);
			break;
		case 2:
			TextObject.color = new Color (0, 0, 1);
			break;
		case 3:
			TextObject.color = new Color (0, 1, 0);
			break;
		case 4:
			TextObject.color = new Color (1, 1, 0);
			break;
		}
	}
}
