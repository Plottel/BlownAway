using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Ballista : IslandTerrain
{
    public int ticksPerShot = 30;
    private int _ticksSinceLastShot = 0;


    private List<Player> _players;

    [SerializeField]
    public float BoltSpeed;

    [SerializeField]
    public float ActivationDistance;

    // Use this for initialization
    void Start ()
    {
        _players = new List<Player>();
        Debug.Log(_players.Count + " : Player Objects");
	}
    private bool AtLeastOnePlayerIsInRange
    {
        get
        {
            return _players.Find(p => Vector3.Distance(transform.position, p.transform.position) < ActivationDistance) != null;
        }
    }

    private Player ClosestPlayer
    {
        get
        {
            float closestDistance = float.MaxValue;
            Player closestPlayer = null;

            foreach (var p in _players)
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

    void FixedUpdate()
    {
        ++_ticksSinceLastShot;
    }
	
	// Update is called once per frame
	void Update ()
    {
        _players.Clear();
        foreach(var x in FindObjectsOfType<Player>())
            _players.Add(x);

        if (_ticksSinceLastShot < ticksPerShot)
            return;

        if (AtLeastOnePlayerIsInRange)
        {
            Debug.Log("HEY IM IN RANGE HERE");
            string playerName = ClosestPlayer.GetComponent<MovementControl>().Player;

            Vector3 toBallista = transform.position - ClosestPlayer.transform.position;
            Vector3 target = toBallista * 2; // Project past Ballista to look in opposite direction
            target.y = 0; // Assume player always same height as Ballista.

            // Change rotation.
            transform.LookAt(target);

            transform.rotation = Quaternion.LookRotation(target);
            //transform.Rotate(Prefabs.Ballista.transform.rotation.eulerAngles); // Maintain sideways cylinder   

            if (CrossPlatformInputManager.GetButtonDown(playerName + "_AttackDirect"))
            {
                if (_ticksSinceLastShot > ticksPerShot)
                {
                    _ticksSinceLastShot = 0;

                    var spawnPoint = new Vector3();
                    spawnPoint = transform.position;
                    spawnPoint += transform.forward * 0.2f;
                    spawnPoint.y += 0.3f;

                    var newRotation = transform.rotation * Quaternion.Euler(0, -90, 0);

                    // Instantiate bolt
                    var bolt = Instantiate(Prefabs.BallistaBolt, spawnPoint, Prefabs.BallistaBolt.transform.rotation);
                    var cannonBlast = Instantiate(Prefabs.cannonBlast, spawnPoint, newRotation);
                    Destroy(cannonBlast, 1f);


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
}
