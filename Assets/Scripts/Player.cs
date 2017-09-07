using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private bool dodging;
	private bool dodgeTimer;
	private Vector3 dodgeDirection;
	private float dodgeStart;
	private int groundCheckDistance;

	public float MaxSpeed = 10;
	public int ReduceAirMovementByFactorOf = 3;
	public int JumpHeight = 4;

	// Use this for initialization
	void Start () {
		
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
		if (Time.time - dodgeStart > 0.02f && dodgeTimer) 
		{
			dodgeTimer = false;
			gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			gameObject.GetComponent<Rigidbody> ().velocity += new Vector3 (0, -10, 0);
		}
	}

	public void Move (Vector3 direction, bool dodge) 
	{
		RaycastHit airborne;
		Physics.Raycast (transform.position + (Vector3.down * 0.3f), Vector3.down, out airborne, groundCheckDistance);
		//Check if airborne
		if (airborne.collider != null) 
		{
			HandleAirborneMovement (direction);
		} 
		else 
		{
			HandleGroundMovement (direction, dodge);	
		}
	}

	void HandleAirborneMovement(Vector3 direction)
	{
		gameObject.GetComponent<Rigidbody> ().AddForce (direction/ReduceAirMovementByFactorOf);
	}

	void HandleGroundMovement(Vector3 direction, bool dodge)
	{
		if (dodge && !dodging) 
		{
			direction = direction.normalized; 
			direction *= 80;
			direction += Vector3.up*JumpHeight*2;
			dodging = true;
			dodgeDirection = direction;
		}
		if (gameObject.GetComponent<Rigidbody> ().velocity.magnitude > MaxSpeed) 
		{
			return;
		}

		gameObject.GetComponent<Rigidbody> ().AddForce (direction*100);
	}
}
