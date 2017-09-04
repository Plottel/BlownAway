using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : IslandTerrain
{
    private List<GameObject> _players;

    [SerializeField]
    public float BoltSpeed;

    [SerializeField]
    public float ActivationDistance;


    public override void ApplyEffect(Collision c)
    {
    }

    // Use this for initialization
    void Start ()
    {
        
        _players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        Debug.Log(_players.Count + " : Player Objects");
	}

    private bool AtLeastOnePlayerIsInRange
    {
        get
        {
            return _players.Find(p => Vector3.Distance(transform.position, p.transform.position) < ActivationDistance) != null;
        }
    }

    private GameObject ClosestPlayer
    {
        get
        {
            float closestDistance = float.MaxValue;
            GameObject closestPlayer = null;

            foreach (GameObject p in _players)
            {
                float dist = Vector3.Distance(transform.position, p.transform.position);

                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestPlayer = p;
                }
            }

            return closestPlayer;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (AtLeastOnePlayerIsInRange)
        {
            Vector3 toBallista = transform.position - ClosestPlayer.transform.position;
            Vector3 target = toBallista * 2; // Project past Ballista to look in opposite direction
            target.y = 0; // Assume player always same height as Ballista.

            // Change rotation.
            transform.LookAt(target);

            transform.rotation = Quaternion.LookRotation(target);
            transform.Rotate(Prefabs.Ballista.transform.rotation.eulerAngles); // Maintain sideways cylinder   

            // F for fire!
            // Eventually this will be handled in "Player" as an
            // "Interact" sort of event.
            // Hard-coded here just for testing.
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Instantiate bolt
                var bolt = Instantiate(Prefabs.BallistaBolt, transform.position, Prefabs.BallistaBolt.transform.rotation);
                bolt.transform.LookAt(target);

                bolt.transform.rotation = Quaternion.LookRotation(target);
                bolt.transform.Rotate(Prefabs.Ballista.transform.rotation.eulerAngles);

                // Set bolt velocity
                bolt.GetComponent<Rigidbody>().velocity = target.normalized;
                bolt.GetComponent<Rigidbody>().velocity *= BoltSpeed;
                Debug.Log("Bolt Speed: " + BoltSpeed);
            }
        }

        
    }
}
