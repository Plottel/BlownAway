using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class MovementControl : MonoBehaviour {

	private ThirdPersonCharacter m_Character; 			// A reference to the Player Abilities on the game object
	private Transform m_Cam;                // A reference to the main camera in the scene
	private Vector3 m_CamForward;           // The current forward direction of the camera
	private Vector3 m_Move;
	private bool m_Jump;                    // the world-relative desired move direction, calculated from the camForward and user input.
	private bool m_attack_area;
	private bool m_attack_direct;
	public string Player = "P1";

	private void Start()
	{
		// get the transform of the main camera
		if (Camera.main != null)
		{
			m_Cam = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning(
				"Warning: no main camera found. Need a Camera named \"MainCamera\", for camera-relative controls.", gameObject);
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		// get the third person character ( this should never be null due to require component )
		m_Character = GetComponent<ThirdPersonCharacter>();
	}


	private void Update()
	{
		if (!m_Jump)
		{
			m_Jump = CrossPlatformInputManager.GetButtonDown(Player + "_Jump");
		}
		if (!m_attack_area && !m_attack_direct) 
		{
			m_attack_area = CrossPlatformInputManager.GetButtonDown (Player + "_Jump");
			m_attack_direct = CrossPlatformInputManager.GetButtonDown (Player + "_AttackDirect");
		}
	}


	// Fixed update is called in sync with physics
	private void FixedUpdate()
	{
		// read inputs
		float h = CrossPlatformInputManager.GetAxis(Player + "_Horizontal");
		float v = CrossPlatformInputManager.GetAxis(Player + "_Vertical");
		bool crouch = Input.GetKey(KeyCode.C);

		// calculate move direction to pass to character
		if (m_Cam != null)
		{
			// calculate camera relative direction to move:
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
			m_Move = v*m_CamForward + h*m_Cam.right;
		}
		else
		{
			// we use world-relative directions in the case of no main camera
			m_Move = v*Vector3.forward + h*Vector3.right;
		}
		#if !MOBILE_INPUT
		// walk speed multiplier
		if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
		#endif

		// pass all parameters to the character control script
		m_Character.Move(m_Move, crouch, m_Jump);
		if (m_Jump)
			m_Character.AttackArea ();
		m_Jump = false;
	}
}
