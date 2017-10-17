using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : MonoBehaviour 
{
    public float maxDistX, maxDistY;
    public int padding = 2;
    public float camSpeed;

    private Vector3 initPos;

    private List<Player> _players;

	// Use this for initialization
	void Start () 
	{
        _players = new List<Player>();
        initPos = gameObject.transform.position;

        

        foreach (var x in FindObjectsOfType<Player>())
            _players.Add(x);
    }
	
	// Update is called once per frame
	void Update () 
	{
        _players = new List<Player>();
        foreach (var x in FindObjectsOfType<Player>())
            _players.Add(x);

        if (_players.Count > 0)
        {
            Debug.Log("Players: " + _players.Count);
            UpdateCameraZoomAndPosition();
        }
	}

    private void UpdateCameraZoomAndPosition()
    {
        var newCamPos = new Vector3();

        var playerCenter = GetPlayerCenterPoint(_players);

        Debug.DrawLine(playerCenter, playerCenter+Vector3.up*2,Color.magenta, 1f);

        //var xMove = Vector3.MoveTowards(new Vector3(transform.position.x, 0, 0), new Vector3(playerCenter.x,0,0), maxDistX);
        //transform.position = initPos + new Vector3(xMove.x, 0, 0);

        //z Calculation
        _players = _players.OrderBy(p => p.transform.position.x).ToList();

        var minX = _players[0].transform.position.x;
        var maxX = _players[_players.Count - 1].transform.position.x;
        var xDistance = Mathf.Abs(maxX - minX);
        xDistance += padding;
        float zPos = Mathf.Clamp(playerCenter.z - xDistance * 1.73f, -30, -2);//Pre-calculated constant for 60 degrees
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,transform.position.y,zPos), camSpeed*Time.fixedDeltaTime*150);

        transform.LookAt(playerCenter);
        Vector3 direction = playerCenter - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, camSpeed * Time.fixedDeltaTime);

    }

    private Vector3 GetPlayerCenterPoint(List<Player> players)
    {
        var positions = new List<Vector3>();
        foreach (var p in players)
            positions.Add(p.transform.position);

        Vector3 result = Vector3.zero;

        foreach (var pos in positions)
        {
            result += pos;
        }

        result /= positions.Count;
        return result;
    }
}
