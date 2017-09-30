using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : IslandTerrain 
{
	public Transform _pushBar;
	public Transform _pushPlate;

	private Vector3 _pushBarStart;
	private Vector3 _pushBarStartScale;
	private Vector3 _pushPlateStart;
	private Vector3 _baseStart;

	private int _extensionCounter;
	public int _extentionTime = 10; //Between Closed and Full Extension (in 30ths of a second)
	private float _extensionDistance = 0.5f;

	public bool _startPush;

	// Use this for initialization
	void Start () 
	{
		_extensionCounter = _extentionTime;

		_pushPlate = GetComponentsInChildren<Transform> () [1];
		_pushPlateStart = new Vector3 (0, 0, 0.4f);

		_pushBar = GetComponentsInChildren<Transform> () [13];
		_pushBarStart = new Vector3 (0, 0, 0.2f);

		_baseStart = new Vector3 (0, 0, 0.2f);

	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Input.GetKeyDown (KeyCode.P)) {
			_startPush = true;
		}

		Push ();
	}

	private void ExtendBy(float dist)
	{
		_pushPlate.Translate (0, 0, dist * transform.lossyScale.z);

		_pushBar.Translate (0, 0, dist * transform.lossyScale.z);
		/*
		_pushPlate.transform.localPosition += (new Vector3 (0, 0, dist));

		float MidZ = (_pushPlate.localPosition.z - 0.4f) / 2f;

		_pushBar.localPosition = new Vector3 (_pushBarStart.x, _pushBarStart.y, MidZ + 0.3f);
		_pushBar.localScale = new Vector3 (_pushBarStartScale.x, _pushBarStartScale.y, MidZ*2);
		*/
	}

	public void Push()
	{
		if (_startPush) {
			if (_extensionCounter > 0) {
				ExtendBy (-_extensionDistance/_extentionTime);
				_extensionCounter -= 1;
			} else {
				_startPush = false;
				PushFinished ();
			}
		} else if (_extensionCounter < _extentionTime) {
			ExtendBy (+_extensionDistance/_extentionTime);
			_extensionCounter += 1;
		}

	}

	public void PushFinished() {

	}
}
