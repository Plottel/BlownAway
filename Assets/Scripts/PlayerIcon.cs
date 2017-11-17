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

	public GameObject ChevronPrefab;
	private GameObject ChevronInstance;

	private Text TextObject;
	private float Health = 0;


	void Start() {
		TextObject = GetComponent<Text> ();
		TextObject.color = Player.ChooseColor (PlayerNumber - 1);
		ChevronInstance = Instantiate (ChevronPrefab, transform);
		ChevronInstance.GetComponent<Image>().color = Player.ChooseColor (PlayerNumber - 1);
		ChevronInstance.GetComponent<Image> ().enabled = false;
	}

	// Update is called once per frame
	void Update () {
		Vector3 wantedPos = Camera.main.WorldToScreenPoint (Target.position);
		transform.position = wantedPos;


		bool OffScreen = false;
		float DesiredAngle = -1;

		if (transform.position.x > Screen.width) {
			Debug.Log ("OutsideScreen Right");
			//transform.position = new Vector3 (Screen.width - 100, transform.position.y, transform.position.z);
			ChevronInstance.transform.position = new Vector3 (Screen.width - 100, transform.position.y, ChevronInstance.transform.position.z);
			DesiredAngle = 270;
			OffScreen = true;
		} else if (transform.position.x < 0) {
			Debug.Log ("OutsideScreen Left");
			ChevronInstance.transform.position = new Vector3 (0 + 100, transform.position.y, ChevronInstance.transform.position.z);
			DesiredAngle = 90;
			OffScreen = true;
		}

		if (transform.position.y > Screen.height) {
			Debug.Log ("OutsideScreen Top ?");
			ChevronInstance.transform.position = new Vector3 (ChevronInstance.transform.position.x, Screen.width - 70, ChevronInstance.transform.position.z);
			if (DesiredAngle == -1)
				DesiredAngle = 0;
			else
				DesiredAngle = DesiredAngle / 2;
			OffScreen = true;
		} else if (transform.position.y < 0) {
			Debug.Log ("OutsideScreen Bottom");
			ChevronInstance.transform.position = new Vector3 (ChevronInstance.transform.position.x, 0 + 70, ChevronInstance.transform.position.z);
			if (DesiredAngle == -1)
				DesiredAngle = 180;
			else
				DesiredAngle = (DesiredAngle + 180) / 2;
			OffScreen = true;
		}

		if (OffScreen) {
			ChevronInstance.GetComponent<Image> ().enabled = true;
			ChevronInstance.transform.rotation = new Quaternion ();
			ChevronInstance.transform.Rotate (0, 0, DesiredAngle);
		} else {
			ChevronInstance.GetComponent<Image> ().enabled = false;
		}

		//height is 100
		//halve and add 20 is *70*

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

	/*
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
	*/
}
