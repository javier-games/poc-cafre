using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			SectionManager.instance.SetCurrentSection(transform.GetSiblingIndex ());
		}
	}
	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
		}
	}

}
