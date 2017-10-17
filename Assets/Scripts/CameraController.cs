using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : MonoBehaviour 
{
    public float maxDistX, maxDistY;
    public float padding = 0;
    public float camSpeed;

    private Vector3 initPos;

    private List<SpawnPointer> _pointers;

	// Use this for initialization
	void Start () 
	{
        _pointers = new List<SpawnPointer>();
        initPos = gameObject.transform.position;

        

        foreach (var x in FindObjectsOfType<SpawnPointer>())
            _pointers.Add(x);
    }
	
	// Update is called once per frame
	void Update () 
	{
        _pointers = new List<SpawnPointer>();
        foreach (var x in FindObjectsOfType<SpawnPointer>())
            _pointers.Add(x);

        if (_pointers.Count > 0)
        {
            Debug.Log("Players: " + _pointers.Count);
            UpdateCameraZoomAndPosition();
        }
	}

    private void UpdateCameraZoomAndPosition()
    {
        var newCamPos = new Vector3();

        var playerCenter = GetPlayerCenterPoint(_pointers);

        Debug.DrawLine(playerCenter, playerCenter+Vector3.up*2,Color.magenta, 1f);

        //var xMove = Vector3.MoveTowards(new Vector3(transform.position.x, 0, 0), new Vector3(playerCenter.x,0,0), maxDistX);
        //transform.position = initPos + new Vector3(xMove.x, 0, 0);

        //z Calculation
        _pointers = _pointers.OrderBy(p => p.transform.position.x).ToList();

        var minX = _pointers[0].transform.position.x;
        var maxX = _pointers[_pointers.Count - 1].transform.position.x;
        var xDistance = Mathf.Abs(maxX - minX);
        xDistance += padding;
        float zPos = Mathf.Clamp(playerCenter.z - xDistance * 1.73f, -30, -2);//Pre-calculated constant for 60 degrees
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,transform.position.y,zPos), camSpeed*Time.fixedDeltaTime*150);

        transform.LookAt(playerCenter);
        Vector3 direction = playerCenter - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, camSpeed * Time.fixedDeltaTime);

    }

    private Vector3 GetPlayerCenterPoint(List<SpawnPointer> players)
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
