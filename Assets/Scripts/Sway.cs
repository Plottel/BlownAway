using UnityEngine;
using System.Collections;

public class Sway : MonoBehaviour {
	private Vector3 position;
	private int randomAmountX;
	private int randomAmountY;
	private bool up;
	private bool right;
	private int randomChanger;
	private float maxMoveX = 0.5f;
	private float maxMoveY = 0.1f;

	
	private float maxSwayX = 0.3f;
	private float maxSwayY = 0.15f;
	public float swayAmountX = 0;
	public float swayAmountY = 0;

	// Use this for initialization
	void Start () {
		position = this.transform.position;

		Random.seed = System.DateTime.Now.Millisecond;

		int y = Random.Range (0, 1);
		if (y == 1) 
			up = true;
		else
			up = false;

		int x = Random.Range (0, 1);
		if (x == 1) 
			right = true;
		else
			right = false;
	}
	
	// Update is called once per frame
	void Update () {
		//roll to determine random change in direction
		randomChanger = Random.Range (1, 100);
		//Roll only has an effect if the object is halfway from starting Pos to max
		if (randomChanger == 1 && transform.position.y >= maxMoveY*0.5)
			up = false;
		if (randomChanger == 2 && transform.position.x >= maxMoveX*0.5)
			right = false;

		if (randomChanger == 1 && transform.position.y < -maxMoveY*0.5)
			up = true;
		if (randomChanger == 2 && transform.position.x < -maxMoveX*0.5)
			right = true;

		if (transform.position.x >= maxMoveX) {
//			swayAmountX = 0;
			right = false;
		} else if (transform.position.x <= -maxMoveX) {
//			swayAmountX = 0;
			right = true;
		}
		if (transform.position.y >= maxMoveY) {
//			swayAmountY = 0;
			up = false;
		} else if (transform.position.y <= -maxMoveY) {
//			swayAmountY = 0;
			up = true;
		}

		if(right)
			swayAmountX = Random.Range (0, maxSwayX);
		else
			swayAmountX = Random.Range (-maxSwayX, 0);
		if(up)
			swayAmountY = Random.Range (0, maxSwayY);
		else
			swayAmountY = Random.Range (-maxSwayY, 0);

		position = new Vector3 (transform.position.x + swayAmountX * Time.smoothDeltaTime, 
		                       transform.position.y + swayAmountY * Time.smoothDeltaTime,
		                       transform.position.z);
		transform.position = position;
	}
}
