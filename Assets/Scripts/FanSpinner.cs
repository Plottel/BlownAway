using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSpinner : MonoBehaviour {
	 /// <summary>
	 /// Revs per second.
	 /// </summary>
	public float Speed = 1f;

	// Update is called once per frame
	void Update () {
		transform.eulerAngles = transform.rotation.eulerAngles + new Vector3 (0, 0, Speed * Time.deltaTime * 360);
	}
}
