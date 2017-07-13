using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class RunnerMovement : MonoBehaviour {

	[SerializeField]
	private Animator animator;

	// Use this for initialization
	void Start () {
		
	}

	public void Move(float dir,float speed){
		
		animator.SetFloat ("Direction",dir);
		animator.SetFloat ("Speed", speed);

	}
}
