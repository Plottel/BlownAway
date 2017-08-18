using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public Spawner spawner;
	public Rigidbody m_Rigidbody;
	public ShipMovement ship;

	public GameObject Target;
	public GameObject Evac;
	public int speed = 1;
	public string Mode = "Wait";

	// Use this for initialization
	void Start () {
		Target = FindObjectOfType<Player> ().gameObject;
		Evac = spawner.EvacPoint;
		ship = FindObjectOfType<ShipMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Mode == "Disembark")
			Disembark ();
		if (Mode == "Attack")
			Attack ();
		if (Mode == "Wait")
			WaitOnShip ();
	}

	void Disembark()
	{
		float step = speed * Time.deltaTime;
		var dest = Evac.transform.position;
		dest.y = transform.position.y;
		this.transform.position = Vector3.MoveTowards (transform.position, dest, step);
	}

	void Attack()
	{
		float step = speed * Time.deltaTime;
		var dest = Target.transform.position;
		dest.y = transform.position.y;
		this.transform.position = Vector3.MoveTowards (transform.position, dest, step);
	}

	void WaitOnShip()
	{
		float step = speed * Time.deltaTime;
		var dest = ship.Dropzone.transform.position;
		dest.y = transform.position.y;
		this.transform.position = Vector3.MoveTowards (transform.position, dest, step);
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.collider.gameObject.name.Contains ("Player") || col.collider.gameObject.name.Contains ("Bound"))
		{
			this.spawner.RemoveChild (this);
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name.Contains ("Disembark"))
		{
			Mode = "Attack";
		}
		if (col.gameObject.name.Contains ("Sphere")) 
		{
			Debug.Log ("Sphere col");
			var blast = col.GetComponent<AreaAttack> ();
			blast.Hit (m_Rigidbody);
		}
		
	}
}
