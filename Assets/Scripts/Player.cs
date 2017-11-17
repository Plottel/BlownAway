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
    private int numJumps = 1;
    private GameObject PE;

    bool isInAir = false;

    public float MaxSpeed = 0.8f;
    public int IncreaseAirMovementByFactorOf = 5;
	public float JumpHeight;
	public float JumpDist;

    public int ticksPerLavaHit = 180;
    public int ticksSinceLastLavaHit = 0;

    public int ticksPerPistonHit = 10;
    public int ticksSinceLastPistonHit = 0;

    public int ticksPerFanHit = 3;
    public int ticksSinceLastFanHit = 0;

    private float _forceTweaker = 0.18f;
    public float Health;
	public float UltimateCharge = 100;
    public bool UltimateEffectOn;

    public GameObject ChargeParticleEffect;



    public void HitMe(float force, Vector3 position, float addThisMuchDamage, bool isFan=false)
    {
        Health += addThisMuchDamage;

        var exponent = ((Health/100) * (Health/100)) * _forceTweaker;

        float forceToApply;

        if (isFan)
            forceToApply = 10 + force * exponent;
        else
            forceToApply = 100 + force * exponent;

        GetComponent<Rigidbody>().AddExplosionForce(forceToApply, position, 100f);
    }

	public void AddUltiCharge(int charge)
	{
		if (UltimateCharge < 100) 
		{
			UltimateCharge += charge;

			if (UltimateCharge > 100)
				UltimateCharge = 100;
		}
        
        if(UltimateCharge == 100)
        {
            if (!UltimateEffectOn)
            {
                UltimateOn();
            }
        }
	}

    // UltimateON and UltimateOff Are used to turn the Particle Effect
    // for the Player's 'Ultimate Ready' state on/off
    private void UltimateOn()
    {
        PE = Instantiate(Prefabs.GlowPE, transform);
        UltimateEffectOn = true;
    }


    public void UltimateOff()
    {
        UltimateEffectOn = false;
        UltimateCharge = 0;
        if(PE)
            Destroy(PE);
    }

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
        ++ticksSinceLastPistonHit;
        ++ticksSinceLastFanHit;
    }

	// Update is called once per frame
	void Update () 
	{
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
			case 5:
				return new Color(1, 1, 1); //White (For menu)
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
			case "titlePlayer":
				return ChooseColor(4);
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
        
        Debug.DrawRay (transform.position, Vector3.down, Color.red, groundCheckDistance);

        bool jumpRequested = GetComponent<MovementControl>().Jump;

        if (jumpRequested && numJumps > 0)
        {
            --numJumps;
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpHeight * 200);
        }

        if (isInAir)
            gameObject.GetComponent<Rigidbody>().AddForce(direction * 7);
        else
            gameObject.GetComponent<Rigidbody>().AddForce(direction * 15);
	}

    void HandleAirborneMovement(Vector3 direction)
    {
        direction = direction.normalized;
        direction *= JumpDist;

        if (GetComponent<MovementControl>().Jump)
            direction += Vector3.up * JumpHeight * 2;

        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > MaxSpeed)
            return;

        gameObject.GetComponent<Rigidbody>().AddForce(direction * 100);
    }

	void HandleGroundMovement(Vector3 direction, bool dodge)
	{
        Debug.Log("Ground");

		if (dodge && !dodging) {
            isInAir = true;
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

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.GetComponent<IslandPiece>())
        {
            // Move player to not be stuck inside island piece.
            
            if (col.gameObject.GetComponent<BoxCollider>().bounds.Intersects(GetComponent<CapsuleCollider>().bounds))
            {
                transform.position += new Vector3(0, 0.01f, 0);
            }
        }
    }

    void OnTriggerStay(Collider col)
    {

        if (col.GetComponent<Lava>())
        {
            ticksSinceLastLavaHit += 1;
            if (ticksSinceLastLavaHit > ticksPerLavaHit)
            {
                ticksSinceLastLavaHit = 0;
                var PE = Instantiate(Prefabs.OnFirePE, transform);
                Destroy(PE, 2f);
                GetComponent<Rigidbody>().AddExplosionForce(6000f, transform.position, 100f);
            }
        }
        if (col.GetComponent<IslandPiece>() || col.GetComponent<IslandTerrain>())
        {
            isInAir = false;
            int magicNum = 2;
            numJumps = magicNum;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.GetComponent<IslandPiece>() || col.GetComponent<IslandTerrain>())
            isInAir = true;
    }
}
