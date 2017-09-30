﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private bool dodging;
	private bool dodgeTimer;
	private Vector3 dodgeDirection;
	private float dodgeStart;
	private float groundCheckDistance = 0.3f;
	private Vector3 dirVector;
	private Vector3 lastDir;
	private bool usedJump;

	public float MaxSpeed = 10;
	public int ReduceAirMovementByFactorOf = 3;
	public int JumpHeight = 3;
	public int JumpDist = 20;

	/// <summary>
	/// Player Damage
	/// Setter must do '+= value;'
	/// </summary>

	// Use this for initialization
	void Start () {
		dirVector = this.gameObject.transform.rotation.eulerAngles;
	}

	// Update is called once per frame
	void Update () 
	{
//		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionX;
//		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ;
		if (dodging) 
		{
			dodgeStart = Time.time;
			dodging = false;
			dodgeTimer = true;
		}
		if (Time.time - dodgeStart > 0.04f && dodgeTimer) 
		{
			dodgeTimer = false;
			gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			gameObject.GetComponent<Rigidbody> ().velocity += new Vector3 (0, -10, 0);
		}
	}

	public void Move (Vector3 direction, float rotX, float rotZ, bool dodge) 
	{
		dirVector = this.gameObject.transform.rotation.eulerAngles;

		// Rotate Player
		if (rotX != 0.0f || rotZ != 0.0f) 
		{
			dirVector = new Vector3 (rotX * 10, 0, rotZ * 10);
		} 
		else 
		{
			dirVector = lastDir;
		}

		lastDir = dirVector;

		transform.rotation = Quaternion.LookRotation( dirVector );

		RaycastHit airborne;
		bool notInAir = Physics.Raycast (transform.position, Vector3.down, out airborne, groundCheckDistance);
		Debug.DrawRay (transform.position, Vector3.down, Color.red, groundCheckDistance);
		//Check if airborne
		if (notInAir)
		{
			HandleGroundMovement (direction, dodge);
		} 
		else 
		{	
			Debug.Log ("Airborne");
			HandleAirborneMovement (direction);
		}
	}

	void HandleAirborneMovement(Vector3 direction)
	{
		gameObject.GetComponent<Rigidbody> ().AddForce (direction/ReduceAirMovementByFactorOf);
	}

	void HandleGroundMovement(Vector3 direction, bool dodge)
	{
		if (dodge && !dodging) {
			direction = direction.normalized; 
			direction *= JumpDist;
			if (direction != Vector3.zero)
				direction += Vector3.up * JumpHeight * 2;

			dodging = true;
			dodgeDirection = direction;
		}
		else if (gameObject.GetComponent<Rigidbody> ().velocity.magnitude > MaxSpeed) 
		{
			return;
		}

		gameObject.GetComponent<Rigidbody> ().AddForce (direction*100); // Do not use dodgeDirection here. That persists after the initial dodge happens.
	}
}
