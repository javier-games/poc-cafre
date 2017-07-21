using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 	This MonoBehaviour class set the current animator state of an "active button".
*/

public class ActiveBtnAnimation : MonoBehaviour {

	private  Animator animator;

	//	Initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}

	//	Methods to Set the Trigger on Animator.
	public void Move2Cup(){
		animator.SetTrigger ("Cup");
	}
	public void Move2Item(){
		animator.SetTrigger ("Item");
	}
	public void Move2Map(){
		animator.SetTrigger ("Map");
	}
	public void Move2Soci(){
		animator.SetTrigger ("Soci");
	}

}
