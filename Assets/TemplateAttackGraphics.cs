using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateAttackGraphics : MonoBehaviour {

	float MinScale = 0.5f;
	float MaxScale = 5.0f;
	float JumpChargeTEMP = 0;


	void Update () {
		transform.position = new Vector3(transform.position.x, 1.01f, transform.position.z);
		//MY TESTING CODE. DELETE.
		if (Input.GetKey (KeyCode.Q)) {
			JumpChargeTEMP += 0.01f;
			RenderAreaAttack (JumpChargeTEMP);
		}
		if (Input.GetKeyUp (KeyCode.Q)) {
			JumpChargeTEMP = 0;
		}
	}

	void RenderAreaAttack (float Magnitude) {
		float size = (MaxScale * Magnitude) + MinScale;
		if (size > MaxScale) {
			transform.localScale = new Vector3(MaxScale, MaxScale, 1);
		} else {
			transform.localScale = new Vector3(size, size, 1);
		}
	}

	//INCOMPLETE.
	void RenderDirectAttack (float Magnitude) {
		float size = (MaxScale * Magnitude) + MinScale;
		if (size > MaxScale) {
			transform.localScale = new Vector3(MaxScale, MaxScale, 1);
		} else {
			transform.localScale = new Vector3(size, size, 1);
		}
	}

	/*
	//Direct Value Mode
	void RenderAreaAttack (float Magnitude) {
		float size = Magnitude + MinScale;
		if (size > MaxScale) {
			transform.localScale = new Vector3(MaxScale, MaxScale, 1);
		} else {
			transform.localScale = new Vector3(size, size, 1);
		}
	}
	*/
}
