using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVehicle : MonoBehaviour {

	private Vector3 position;
	private Quaternion rotation;

	private bool stay = true;

	void Start () {
		position = transform.position;
		rotation = transform.rotation;
	}

	void Update(){
		if (stay) {
			transform.position = position;
			transform.rotation = rotation;
		}
	}

	void OnCollisionStay(Collision other){
		if (!other.collider.CompareTag ("Player")) {
			stay = false;
		}
	}
}
