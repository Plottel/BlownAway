using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateAttackGraphics : MonoBehaviour {

	public float AreaMinScale = 0.5f;
	public float AreaMaxScale = 5.0f;
	public float DirectMinScale = 0.5f;
	public float DirectMaxScale = 4.0f;
	float JumpChargeTEMP = 0;

	void Update () {
		transform.position = new Vector3(transform.position.x, 1.01f, transform.position.z);
		//MY TESTING CODE. DELETE.
		if (Input.GetKey (KeyCode.Q)) {
			JumpChargeTEMP += 0.01f;
			RenderDirectAttack (JumpChargeTEMP);
		}
		if (Input.GetKeyUp (KeyCode.Q)) {
			JumpChargeTEMP = 0;
		}
	}

	public void RenderAreaAttack (float Magnitude) {
		float size = (AreaMaxScale * Magnitude) + AreaMinScale;
		if (size > AreaMaxScale) {
			transform.localScale = new Vector3(AreaMaxScale, AreaMaxScale, 1);
		} else {
			transform.localScale = new Vector3(size, size, 1);
		}
	}

	//INCOMPLETE.
	public void RenderDirectAttack (float Magnitude) {
		float size = (DirectMaxScale * Magnitude) + DirectMinScale;
		if (size > DirectMaxScale) {
			transform.localScale = new Vector3(DirectMaxScale, DirectMaxScale, 1);
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
