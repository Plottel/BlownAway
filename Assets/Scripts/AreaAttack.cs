using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour {

	public int ExplosionForce = 400;
	public int ExplosionRadius = 100;

	private Vector3 growthFactor;
	private Vector3 maxSize;
	private float currentScale;
	private float growAmount;
	private float endTime;

	// Use this for initialization
	void Start () {
		growthFactor = new Vector3(1, 1, 1);
		maxSize = new Vector3(ExplosionRadius, ExplosionRadius, ExplosionRadius);
		this.transform.localScale = growthFactor;
		endTime = Time.time + Time.fixedTime + 0.5f;
	
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