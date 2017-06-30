using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBus : MonoBehaviour {

	[SerializeField]
	private float speed = 1;
	[SerializeField]
	private Quaternion originalRotation;


	void Update () {
		transform.Rotate (new Vector3(0,speed*Time.deltaTime,0));
	}

	public void Reset(){
		//transform.localRotation = originalRotation;
	}
}
