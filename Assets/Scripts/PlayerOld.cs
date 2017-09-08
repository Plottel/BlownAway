using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]

public class PlayerOld : MonoBehaviour 
{
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;
	[SerializeField] float m_JumpPower = 12f;
	[Range(1f, 4f)][SerializeField] float gravityMultiplier = 2f;
	[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
	[SerializeField] float m_MoveSpeedMultiplier = 1f;
	[SerializeField] float m_AnimSpeedMultiplier = 1f;
	[SerializeField] float m_GroundCheckDistance = 0.1f;

	Rigidbody m_Rigidbody;
	bool isGrounded;
	float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	Vector3 m_GroundNormal;
	float m_CapsuleHeight;
	Vector3 m_CapsuleCenter;
	CapsuleCollider m_Capsule;
	bool m_Crouching;
	int height = 1;

	public int speed = 1;


	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();
		m_CapsuleHeight = m_Capsule.height;
		m_CapsuleCenter = m_Capsule.center;

		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;
	}


	public void Move(Vector3 move, bool crouch, bool jump)
	{

		// convert the world relative moveInput vector into a local-relative
		// turn amount and forward amount required to head in the desired
		// direction.
		if (move.magnitude > 1f) move.Normalize();
		//move *= speed;
		//move = transform.InverseTransformDirection(move);
		CheckGroundStatus();
		//move = Vector3.ProjectOnPlane(move, m_GroundNormal);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;


		//ApplyExtraTurnRotation();

		// control and velocity handling is different when grounded and airborne:
		if (isGrounded)
		{
			HandleGroundMovement(move, jump);
		}
		else
		{
			HandleAirborneMovement(move);
		}

	}



	void HandleAirborneMovement(Vector3 move)
	{
		// apply extra gravity from multiplier:
		Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
		m_Rigidbody.AddForce(extraGravityForce);

		m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;

		//movement
		float step = speed*0 * Time.deltaTime;

		move.y=0;
		Debug.Log("speed: " + move*Time.deltaTime);
		m_Rigidbody.AddForce (move*step);
		m_Rigidbody.drag = 0;
	}


	void HandleGroundMovement(Vector3 move, bool jump)
	{
		// check whether conditions are right to allow a jump:
		if (jump && isGrounded) {
			// jump!
			m_Rigidbody.velocity = new Vector3 (m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
			isGrounded = false;
			m_GroundCheckDistance = 0.1f;
		} else {
			float step = speed*10 * Time.deltaTime;

			move.y=0;
			Debug.Log("speed: " + move*Time.deltaTime);
			m_Rigidbody.AddForce (move*step);
			m_Rigidbody.drag = 0;

		}
	}

	void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}



	void CheckGroundStatus()
	{
		RaycastHit hitInfo;


		#if UNITY_EDITOR
		// This will show the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.5f*height), transform.position + (Vector3.up * 0.5f*height) + (Vector3.down * m_GroundCheckDistance));
		#endif


		// 0.1f is a small offset to start the ray from inside the character
		// Transform position ishould be at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			isGrounded = true;
			Debug.Log ("Grounded");
		}
		else
		{
			isGrounded = false;
			m_GroundNormal = Vector3.up;
			Debug.Log ("Airborn");
		}
	}
}