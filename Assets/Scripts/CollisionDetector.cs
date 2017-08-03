using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour {

	private CoinFactory factory;

	void Start(){
		factory = GetComponent<CoinFactory> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.transform.CompareTag ("Coin")) {
			Destroy (other.gameObject);
			factory.TossCoins (0,1,0.1f);
		}
	}
	void OnCollisionEnter(Collision other){
		if (other.transform.CompareTag ("Passenger")) {
			Debug.Log ("onrg");
		}
	}
}
