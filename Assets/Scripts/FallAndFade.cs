using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallAndFade : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (0, -2, 0);

		Image I = GetComponent<Image> ();
		Text T = GetComponent<Text> ();

		if (I) {
			I.color = new Color (I.color.r, I.color.g, I.color.b, I.color.a - 0.02f);
			if (I.color.a <= 0)
				Destroy (gameObject);
		} 
		else if (T) {
			T.color = new Color (T.color.r, T.color.g, T.color.b, T.color.a - 0.02f);
			if (I.color.a <= 0)
				Destroy (gameObject);
		}
	}
}
