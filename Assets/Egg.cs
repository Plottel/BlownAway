using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Egg : MonoBehaviour
{

	public bool OverTile = false;
	public string PlayerNum = "P3";
	public float MovementSpeed = 0.5f;
	public Transform Target = null;
	public bool FallingMode = false;
	public GameObject EggBrokenPrefab;

	public bool ManualUpdate()
	{
		float h = CrossPlatformInputManager.GetAxis(PlayerNum + "_Horizontal");
		float v = CrossPlatformInputManager.GetAxis(PlayerNum + "_Vertical");

		transform.position += new Vector3((h * 0.3f * MovementSpeed), 0, (v * 0.3f * MovementSpeed));

		RaycastHit Hit;

		Vector3 RayStart = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z);

		if (Physics.Raycast(RayStart, Vector3.down * 35, out Hit, 35, 12))
		{
			if (Hit.collider.GetComponent<KillBox>())
				return false;
			GetComponent<MeshRenderer>().material.color = Player.ChooseColor(PlayerNum);
			//Debug.DrawRay (RayStart, Vector3.down * 6, Color.red);
			Debug.Log(Hit.point);
			transform.position = Hit.point;
			return true;
		}
		return false;
	}

	void OnCollisionEnter (Collision col) {
		KillBox K = col.gameObject.GetComponent<KillBox> ();
		if (!K) {
			FindObjectOfType<MultiplayerController> ().EggBroke (PlayerNum);
			GameObject E = Instantiate (EggBrokenPrefab, transform.position + new Vector3 (0, -0.3f, 0), new Quaternion ());
			GetComponent<Collider> ().isTrigger = true;
			Destroy (gameObject);
		}
	}
}