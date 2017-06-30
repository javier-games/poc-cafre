using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManagerFooter : MonoBehaviour {

	private  Animator animator;

	void Start () {

		animator = GetComponent<Animator> ();

	}

	public void Move2Cup(){
		animator.SetBool ("Soci",false);
		animator.SetBool ("Map" ,false);
		animator.SetBool ("Item",false);
		animator.SetBool ("Cup" ,true );
	}
	public void Move2Item(){
		animator.SetBool ("Soci",false);
		animator.SetBool ("Cup" ,false);
		animator.SetBool ("Map",false);
		animator.SetBool ("Item" ,true );
	}
	public void Move2Map(){
		animator.SetBool ("Soci",false);
		animator.SetBool ("Cup" ,false);
		animator.SetBool ("Item",false);
		animator.SetBool ("Map" ,true );
	}
	public void Move2Soci(){
		animator.SetBool ("Cup" ,false);
		animator.SetBool ("Item",false);
		animator.SetBool ("Map" ,false);
		animator.SetBool ("Soci",true );
	}

}
