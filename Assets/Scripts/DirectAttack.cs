using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttack : MonoBehaviour {

	public int ExplosionForce = 400;
	public int ExplosionRadius = 100;

	private float endTime;

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

	public void Hit(Rigidbody target)
	{
		target.AddExplosionForce (ExplosionForce, this.gameObject.transform.position, ExplosionRadius);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			Rigidbody EnemyBody = col.gameObject.GetComponent<Rigidbody>();
			Hit (EnemyBody);
		}
	}
}