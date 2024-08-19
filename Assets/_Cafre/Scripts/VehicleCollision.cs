using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollision : MonoBehaviour {

	void Start(){
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	void OnCollisionEnter(Collision other){
		if (other.collider.CompareTag ("Player")) {
			GetComponent<Rigidbody> ().AddForce(other.contacts[0].normal*10000f,ForceMode.Impulse);
			AudioManager.instance.Crash (GetComponent<AudioSource>());
		}
	}
}
