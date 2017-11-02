using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayUpright : MonoBehaviour {

	private Quaternion initRot;

	// Use this for initialization
	void Start () {
		initRot = gameObject.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.rotation = initRot;
	}
}
