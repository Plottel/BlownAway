using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour {

	public int ExplosionForce = 400;
	public int ExplosionRadius = 100;

	private Vector3 growthFactor;
	private Vector3 maxSize;
	private float currentScale;
	private TemplateAttackGraphics Template;

	// Use this for initialization
	void Start () {
		growthFactor = new Vector3(1, 1, 1);
		maxSize = new Vector3(ExplosionRadius, ExplosionRadius, ExplosionRadius);
		this.transform.localScale = growthFactor;
		Template = GameObject.Instantiate<TemplateAttackGraphics> (Template);
	}
	
	// Update is called once per frame
	void Update () {
		Grow ();
		DrawTemplate ();
	}

	void Grow()
	{
		if (transform.localScale.x >= maxSize.x) 
		{
			currentScale = this.transform.localScale + growthFactor;
			this.transform.localScale = currentScale;
		}
	}

	void DrawTemplate()
	{
		
	}

	public void Hit(Rigidbody target)
	{
		target.AddExplosionForce (ExplosionForce, this.gameObject.transform.position, ExplosionRadius);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name.Contains ("Enemy"))
		{
			Rigidbody EnemyBody = col.gameObject.GetComponent<Rigidbody>();
			Hit (EnemyBody);
		}
	}
}
