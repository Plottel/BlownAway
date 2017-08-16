using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour {

	public int ExplosionForce = 10000;
	public int ExplosionRadius = 100;

	private Vector3 GrowthFactor;
	private Vector3 MaxSize;

	// Use this for initialization
	void Start () {
		GrowthFactor = new Vector3(1, 1, 1);
		MaxSize = new Vector3(ExplosionRadius, ExplosionRadius, ExplosionRadius);
		this.transform.localScale = GrowthFactor;
	}
	
	// Update is called once per frame
	void Update () {
		Grow ();
	}

	void Grow()
	{
		if (transform.localScale.x >= MaxSize.x) 
		{
			this.transform.localScale += GrowthFactor;
		}
	}

	public void Hit(Rigidbody target)
	{
		target.AddExplosionForce (ExplosionForce, this.gameObject.transform.position, ExplosionRadius);
	}

	void OnTriggerEnter(Collider col)
	{
		Debug.Log ("Sphere Hit!");
		if (col.gameObject.name.Contains ("Enemy"))
		{
			Rigidbody RB = col.gameObject.GetComponent<Rigidbody>();
			RB.AddExplosionForce (ExplosionForce, this.gameObject.transform.position, ExplosionRadius);
		}
	}
}
