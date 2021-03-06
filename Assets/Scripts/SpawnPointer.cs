﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SpawnPointer : MonoBehaviour {

	public bool OverTile = false;
	public string PlayerNum = "P3";
	public float MovementSpeed = 0.5f;
	public Transform Target = null;
	public IslandGrid grid;
    public bool isPartOfUltimate = false;

	void Start()
	{
		grid = FindObjectOfType<IslandGrid>();
	}

	public bool ManualUpdate () {

		if (Target != null) { //Error prevention and legacy support.
			MovementControl M = Target.GetComponent<MovementControl> ();
			if (M != null) {
				//FOLLOWING PLAYER.
				var S = (float)M.TicksSinceAttack / (float)M.TicksPerAttack;
				if (S > 1)
					S = 1;

				transform.localScale = new Vector3 (S, S, S);
			} else {

			}

			if (grid == null)
				grid = FindObjectOfType<IslandGrid> ();

			//Set my position and rotation to that of the player, with the height of the grid.
			transform.position = new Vector3 (Target.position.x, grid.transform.position.y, Target.position.z);
			transform.eulerAngles = new Vector3(270, 0, Target.eulerAngles.y - 180);


			RaycastHit Hit;
			Vector3 RayStart = Target.position;

			if (Physics.Raycast (RayStart, Vector3.down * 35, out Hit, 35)) {
				if (Hit.collider.GetComponent<KillBox> () && !isPartOfUltimate) {
					GetComponent<SpriteRenderer> ().color = new Color (0.8f, 0.8f, 0.8f, 1);
					return false;
				}
                if (!isPartOfUltimate)
				    GetComponent<SpriteRenderer> ().color = Player.ChooseColor (PlayerNum);
				//Debug.DrawRay (RayStart, Vector3.down * 6, Color.red);
				transform.position = Hit.point;
				return true;
			}

			return false;
		}
		else
		{
			float h = CrossPlatformInputManager.GetAxis(PlayerNum + "_Horizontal");
			float v = CrossPlatformInputManager.GetAxis(PlayerNum + "_Vertical");

			transform.position += new Vector3((h * 0.3f * MovementSpeed), 0, (v * 0.3f * MovementSpeed));

			RaycastHit Hit;

			Vector3 RayStart = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z);

			if (Physics.Raycast(RayStart, Vector3.down * 35, out Hit, 35))
			{
				if (Hit.collider.GetComponent<KillBox>())
					return false;
				GetComponent<SpriteRenderer>().color = Player.ChooseColor(PlayerNum);
				//Debug.DrawRay (RayStart, Vector3.down * 6, Color.red);
				transform.position = Hit.point;
				return true;
			}
			return false;
		}


	}
}