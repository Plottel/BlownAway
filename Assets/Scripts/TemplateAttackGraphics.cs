using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateAttackGraphics : MonoBehaviour {

	public float AreaMinScale = 0.5f;
	public float AreaMaxScale = 5.0f;
	public float DirectMinScale = 0.5f;
	public float DirectMaxScale = 4.0f;
	float _charge = 0;

	void Update () 
	{
		transform.position = new Vector3(transform.position.x, 1.01f, transform.position.z);
	}

	public void Grow()
	{
		_charge += 1f;
		RenderAreaAttack (_charge);
	}

	public void Show(bool show)
	{
		if (show) {
			this.gameObject.layer = 8;	//8 = "Attacks"
		} else {
			this.gameObject.layer = 9;	//9 = "Hidden"
		}
	}

	public void RenderAreaAttack (float Magnitude) 
	{
		float size = (AreaMaxScale * Magnitude) + AreaMinScale;
		if (_charge > AreaMaxScale) {
			transform.localScale = new Vector3(AreaMaxScale, AreaMaxScale, 1);
		} else {
			transform.localScale = new Vector3(_charge, _charge, 1);
		}
	}

	//INCOMPLETE.
	public void RenderDirectAttack (float Magnitude) 
	{
		float size = (DirectMaxScale * Magnitude) + DirectMinScale;
		if (size > DirectMaxScale) {
			transform.localScale = new Vector3(DirectMaxScale, DirectMaxScale, 1);
		} else {
			transform.localScale = new Vector3(size, size, 1);
		}
	}

	/*
	//Direct Value Mode
	void RenderAreaAttack (float Magnitude) {
		float size = Magnitude + MinScale;
		if (size > MaxScale) {
			transform.localScale = new Vector3(MaxScale, MaxScale, 1);
		} else {
			transform.localScale = new Vector3(size, size, 1);
		}
	}
	*/
}
