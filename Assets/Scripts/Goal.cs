using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		if (other.collider.CompareTag ("Player")) {
			GameManager.instance.ChangeToNewState (GameState.WIN);
		}
	}
}
