using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;

public class MovementControl : MonoBehaviour {

	private Player m_Character; 		// A reference to the Player Abilities on the game object
	private Transform m_Cam;                		// A reference to the main camera in the scene
	private Vector3 m_CamForward;           		// The current forward direction of the camera
	private Vector3 m_Move;
	private Animator anim;


	private bool jump;                    // the world-relative desired move direction, calculated from the camForward and user input.
    private int jumpCount;

	private bool m_attack_ultimate;
    private bool m_attack_direct;
    public int TicksPerAttack = 30;
    public int TicksSinceAttack = 0;
	public string PlayerName = "P1";
	public DirectAttack AttackCone;
	public Ultimate Ultimate;

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
				"Warning: no main camera found in scene. Add a camera named \"MainCamera\", for camera-relative controls.", gameObject);
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		// get the third person character ( this should never be null due to require component )
		m_Character = GetComponent<Player>();
		anim = GetComponentInChildren<Animator> ();

        GetComponentsInChildren<Renderer>()[1].material.color = Player.ChooseColor(PlayerName);
	}

	private void Update()
	{
		if (!jump) 
		{
			jump = CrossPlatformInputManager.GetButtonDown(PlayerName + "_Jump");
		}
	}

	// Fixed update is called in sync with physics
	private void FixedUpdate()
	{
        m_attack_direct = CrossPlatformInputManager.GetButtonDown(PlayerName + "_AttackDirect");
		m_attack_ultimate = CrossPlatformInputManager.GetButtonDown(PlayerName + "_Ultimate");

        TicksSinceAttack += 1;

		// Read motion inputs

		// Movement
		float h = CrossPlatformInputManager.GetAxis(PlayerName + "_Horizontal");
		float v = CrossPlatformInputManager.GetAxis(PlayerName + "_Vertical");

		// Rotation
		float rX = CrossPlatformInputManager.GetAxis(PlayerName + "_RotationX");
		float rZ = CrossPlatformInputManager.GetAxis(PlayerName + "_RotationZ");
	

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
		m_Character.Move(m_Move, rX, rZ, jump);
		anim.SetBool("isJumping", jump);
		jump = false;

		if (m_attack_direct) 
		{
            if (TicksSinceAttack > TicksPerAttack)
            {
            	anim.SetBool ("isAttacking", true);


                TicksSinceAttack = 0;
                var attack = GameObject.Instantiate(AttackCone, this.gameObject.transform.position, this.gameObject.transform.localRotation, this.gameObject.transform);
                var PE = Instantiate(Prefabs.TempAttack, attack.transform.position, attack.transform.rotation);
                var partSystem = PE.GetComponent<ParticleSystem>();
                var main = partSystem.main;
                main.startColor = Player.ChooseColor(GetComponent<MovementControl>().PlayerName);


                Destroy(PE, 0.2f);
            }
		}

		if (m_attack_ultimate) 
		{
			if (m_Character.UltimateCharge >= 100) 
			{
				var attack = GameObject.Instantiate (Ultimate, gameObject.transform.position, gameObject.transform.localRotation, gameObject.transform);
				var PE = Instantiate (Prefabs.cannonBlast, attack.transform.position, attack.transform.rotation);

				Destroy (PE, 0.2f);
			}
		}
	}
}
