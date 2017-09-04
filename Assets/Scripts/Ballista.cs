using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : IslandTerrain
{
    private List<Player> _players;

    public float ActivationDistance = 0.5f;


    public override void ApplyEffect(Collision c)
    {
    }

    // Use this for initialization
    void Start ()
    {
        _players = new List<Player>(FindObjectsOfType<Player>());
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

            foreach (Player p in _players)
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
            transform.LookAt(2 * transform.position - ClosestPlayer.transform.position); // Look away from closest player
	}
}
