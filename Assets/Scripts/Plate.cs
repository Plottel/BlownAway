using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour {
	
	void Update () {
		//transform.localPosition.y;
		Vector3 Force = new Vector3 (0, -transform.localPosition.y + 5, 0);
		GetComponent<Rigidbody> ().AddForce (Force);

		if (transform.localPosition.y > 0) {
			transform.localPosition = new Vector3 (0, 0, 0);
		}

		/*
		if (GetComponent<Rigidbody> ().velocity.magnitude < 0.5f) {
			if (GetComponent<Rigidbody> ().velocity.magnitude < 0.5f && transform.localPosition.y < 1) {
				Vector3 Force = new Vector3 (0, 5f, 0);
				GetComponent<Rigidbody> ().AddForce (Force);
			} else if (transform.localPosition.y > 1) {
				Vector3 Force = new Vector3 (0, -4f, 0);
				GetComponent<Rigidbody> ().AddForce (Force);
			}
		}*/
	}
}
