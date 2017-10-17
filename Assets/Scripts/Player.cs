using System.Collections;
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
    private bool notInAir = true;


    public float MaxSpeed = 0.8f;
    public int IncreaseAirMovementByFactorOf = 5;
	public float JumpHeight = 1.3f;
	public int JumpDist = 20;

    public int ticksPerLavaHit = 180;
    public int ticksSinceLastLavaHit = 0;


    public float Health;

	/// <summary>
	/// Player Damage
	/// Setter must do '+= value;'
	/// </summary>

	// Use this for initialization
	void Start () {
		dirVector = this.gameObject.transform.rotation.eulerAngles;
	}

    void FixedUpdate()
    {
        ++ticksSinceLastLavaHit;
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
		}
	}


    public static Color ChooseColor(int PlayerArrayNumber)
    {
        switch (PlayerArrayNumber + 1)
        {
            case 1:
                return new Color(1, 0.1f, 0.072f); //Red
            case 2:
                return new Color(0.125f, 0.398f, 1); //Blue
            case 3:
                return new Color(0.665f, 0.152f, 1); //Purple
            case 4:
                return new Color(1, 0.875f, 0.214f); //Yellow
        }


        return new Color(1, 1, 1);
    }

    public static Color ChooseColor(string PlayerName)
    {
        switch (PlayerName)
        {
            case "P1":
                return ChooseColor(0);
            case "P2":
                return ChooseColor(1);
            case "P3":
                return ChooseColor(2);
            case "P4":
                return ChooseColor(3);
        }


        return new Color(1, 1, 1);
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
        //bool notInAir = Physics.Raycast (transform.position, Vector3.down, out airborne, groundCheckDistance);
        
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
        var proposedVel = gameObject.GetComponent<Rigidbody>().velocity + (IncreaseAirMovementByFactorOf * direction);
        var vertV = proposedVel.y;
        proposedVel.y = 0;

        if (proposedVel.magnitude > MaxSpeed)
        {
            proposedVel = proposedVel.normalized * MaxSpeed;
        }

        proposedVel.y = vertV;

        gameObject.GetComponent<Rigidbody>().velocity = proposedVel;
    }

	void HandleGroundMovement(Vector3 direction, bool dodge)
	{
		if (dodge && !dodging) {
            notInAir = false;
			direction = direction.normalized; 
			direction *= JumpDist;

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
    
    void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<Lava>())
        {
            if (ticksSinceLastLavaHit > ticksPerLavaHit)
            {
                ticksSinceLastLavaHit = 0;
                Debug.Log("Lava poked me");
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 60f, 0));
            }
        }
        if (!col.GetComponent<Player>())
        {
            notInAir = true;
        }
    }
}
