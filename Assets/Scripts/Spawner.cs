using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public int FirstWaveUnits;
	public int PerWaveIncrease;
	public EnemyAI Unit;
	public GameObject EvacPoint;
	public ShipMovement ship;

	private int wave;
	private List<EnemyAI> units;

	// Use this for initialization
	void Start () {
		wave = 1;
		units = new List<EnemyAI> ();
		ship = FindObjectOfType<ShipMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void RequestNextWave()
	{
		if (units.Count == 0)
		{
			for (int i = 0; i < FirstWaveUnits + wave * PerWaveIncrease; i++)
			{
				var spawnPos = transform.position + Random.insideUnitSphere * 2;
				EnemyAI spawned = (EnemyAI)GameObject.Instantiate (Unit, spawnPos, this.transform.rotation);
				units.Add (spawned);
				spawned.spawner = this;
			}
			ship.Mode = "Approach";
		}
	}

	public void UnloadWave()
	{
		foreach (EnemyAI eAI in units)
		{
			eAI.Mode = "Disembark";
		}
	}

	public bool CheckForWaveUnload()
	{
		bool allOut = true;
		allOut = units.Find (u => u.Mode == "Disembark") != null;
		return allOut;
	}

	public void RemoveChild(EnemyAI child)
	{
		units.Remove (child);
	}
}
