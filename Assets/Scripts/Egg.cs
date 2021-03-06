﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Egg : MonoBehaviour
{

	public bool OverTile = false;
	public string PlayerNum = "P3";
	public float MovementSpeed = 0.5f;
	public Transform Target = null;
	public bool FallingMode = false;
	public GameObject EggBrokenPrefab;

    private Camera cam;
    private float scaleFactor = 0.3f;

    public void Start ()
    {
        cam = FindObjectOfType<Camera>();
    }

	public bool ManualUpdate()
	{
        if (cam == null)
            return false;

        float y = transform.position.y;
		float h = CrossPlatformInputManager.GetAxis(PlayerNum + "_Horizontal");
		float v = CrossPlatformInputManager.GetAxis(PlayerNum + "_Vertical");
        Vector3 camForward = cam.gameObject.transform.forward;
        Vector3 camRight = cam.gameObject.transform.right;
        Vector3 movement = v * camForward + h * camRight;
        

        //transform.position += new Vector3((h * 0.3f * MovementSpeed), 0, (v * 0.3f * MovementSpeed));
        transform.position += movement * scaleFactor * MovementSpeed;
		transform.position = new Vector3 (transform.position.x, y, transform.position.z);

        RaycastHit Hit;

		Vector3 RayStart = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z);

		if (Physics.Raycast(RayStart, Vector3.down * 35, out Hit, 35, 12))
		{
			Debug.Log ("Raycast hit something.");
			Debug.Log (Hit.transform.gameObject.name);
			/*
			if (Hit.collider.GetComponent<KillBox>())
				return false;
			GetComponent<MeshRenderer>().material.color = Player.ChooseColor(PlayerNum);
			//Debug.DrawRay (RayStart, Vector3.down * 6, Color.red);
			Debug.Log(Hit.point);
			transform.position = Hit.point;
			return true;
			*/
			return false;
		}

		return false;
	}

	void OnCollisionEnter (Collision col) {
		KillBox K = col.gameObject.GetComponent<KillBox> ();
		if (!K) {
			FindObjectOfType<MultiplayerController> ().EggBroke (PlayerNum);
			GameObject E = Instantiate (EggBrokenPrefab, transform.position + new Vector3 (0, -0.3f, 0), new Quaternion ());
			Renderer[] Renderers = E.GetComponentsInChildren<Renderer> ();
			foreach (Renderer R in Renderers) {
				R.materials[1].color = Player.ChooseColor (PlayerNum);
			}
			GetComponent<Collider> ().isTrigger = true;
			Destroy (gameObject);
		}
	}
}