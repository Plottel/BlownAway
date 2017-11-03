using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttack : MonoBehaviour {

	public int ExplosionForce = 40;
	public int ExplosionRadius = 50;

    public static float Force = 525f;
    public static float Damage = 25f;
	public static int Charge = 10;

	private float endTime;

    private bool[] playerHit = new bool[4];

	// Use this for initialization
	void Start () 
	{
		endTime = Time.fixedTime + 0.05f;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Time.fixedTime >= endTime) 
		{
			Destroy (this.gameObject);
		}
	}

    void OnTriggerEnter(Collider col)
	{
		Player P = col.GetComponent<Player> ();

        if (P == null)
            return;

        if (!playerHit[MultiplayerController.PNameToNumber(P.name)])
        {
            transform.parent.GetComponent<Player>().AddUltiCharge(20);
            playerHit[MultiplayerController.PNameToNumber(P.name)] = true;
            P.HitMe(Force, transform.position, Damage);
        }           
	}
}