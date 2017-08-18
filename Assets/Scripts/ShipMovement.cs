using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

	public Spawner spawner;

	public GameObject Spawnzone;
	public GameObject Dropzone;
	public int speed = 1;
	public string Mode = "Approach";

	private Vector3 drop;
	private Vector3 spawn;
	// Use this for initialization
	void Start () {
		spawn = Spawnzone.transform.position;
		drop = Dropzone.transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (Mode == "Approach")
			Approach ();
		if (Mode == "Unloading")
			Unload ();
		if (Mode == "Leave")
			Leave ();
		if (Mode == "Wait")
			Wait ();
	}

	void Approach()
	{
		float step = speed * Time.deltaTime;
		var dest = drop;
		dest.y = transform.position.y;
		this.transform.position = Vector3.MoveTowards (transform.position, dest, step);
	}

	void Unload()
	{
		bool done = spawner.CheckForWaveUnload();
		if (done)
			Mode = "Leave";
	}

	void Leave()
	{
		float step = speed * Time.deltaTime;
		var dest = spawn;
		dest.y = transform.position.y;
		this.transform.position = Vector3.MoveTowards (transform.position, dest, step);
	}

	void Wait()
	{
		//Wait until ready for next wave
		spawner.RequestNextWave ();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name.Contains ("Spawn"))
		{
			Mode = "Wait";
		}
		if (col.gameObject.name.Contains ("Drop"))
		{
			Mode = "Unloading";
			spawner.UnloadWave ();
		}

	}
}
