using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : IslandTerrain 
{
	public Transform _pushPlate;

    public int PushFrequency = 80;
    private int _ticksSinceLastPush;

	public Collider TriggerCollider;

	private int _extensionCounter;
	public int _extentionTime = 10; //Between Closed and Full Extension (in 30ths of a second)
	private float _extensionDistance = 1.2f;

	private bool _startPush;

	public int ExplosionForce = 400;
    public int ExplosionRadius = 100;

    public static float Force = 600f;
    public static float Damage = 40f;



    void Start () 
	{
        _ticksSinceLastPush = Random.Range(0, PushFrequency);
		_extensionCounter = _extentionTime;
		if (_pushPlate == null)
			_pushPlate = GetComponentsInChildren<Transform> () [1];
		
		Collider[] Cols = GetComponentsInChildren<Collider>();
		foreach (Collider Col in Cols) {
			if (Col.isTrigger == true)
				TriggerCollider = Col;
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
        ++_ticksSinceLastPush;

        if (_ticksSinceLastPush == PushFrequency)
        {
            _ticksSinceLastPush = 0;
            StartPush();
        }
		Push ();
	}

	private void ExtendBy(float dist)
	{
		_pushPlate.Translate (0, 0, dist * transform.lossyScale.z);

		//_pushBar.Translate (0, 0, dist * transform.lossyScale.z);
		/*
		_pushPlate.transform.localPosition += (new Vector3 (0, 0, dist));

		float MidZ = (_pushPlate.localPosition.z - 0.4f) / 2f;

		_pushBar.localPosition = new Vector3 (_pushBarStart.x, _pushBarStart.y, MidZ + 0.3f);
		_pushBar.localScale = new Vector3 (_pushBarStartScale.x, _pushBarStartScale.y, MidZ*2);
		*/
	}

	public void Push()
	{
		if (_startPush) {
			if (_extensionCounter > 0) {
				ExtendBy (-_extensionDistance/_extentionTime);
				_extensionCounter -= 1;
			} else {
				_startPush = false;
				PushFinished ();
			}
		} else if (_extensionCounter < _extentionTime) {
			ExtendBy (+_extensionDistance/_extentionTime);
			_extensionCounter += 1;
		}

	}

	public void StartPush() {
		_startPush = true;
		TriggerCollider.enabled = true;
	}

	public void PushFinished() {
		TriggerCollider.enabled = false;
	}
		
	public override void Operate()
	{
		StartPush();
	}

	public void Hit(Player target)
	{
		float Multiplier = target.Health;
		Multiplier = (Multiplier / 100f) + 1;
		Multiplier = Multiplier * Multiplier; //Square for pistons (maybe too much);
		target.Health += Damage;
		target.GetComponent<Rigidbody>().AddExplosionForce (ExplosionForce * Multiplier, this.gameObject.transform.position, ExplosionRadius);
	}

	public override void Triggered(Collider Col) {
		Player P = Col.GetComponent<Player> ();
		if (P)
		{
            P.HitMe(Force, transform.position, Damage);
		}
	}
}
