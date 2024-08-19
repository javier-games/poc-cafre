using UnityEngine;

/*
 *	In each frame set the rotation of the gameObject
*/

public class RotateBus : MonoBehaviour {

	[SerializeField]
	private float speed = 1;

	//	Rotate the GO in specific time
	void Update () {
		transform.Rotate (new Vector3(0,speed*Time.deltaTime,0));
	}
}
