using UnityEngine;

/*
 *	This script has to be attached to the endless runner character to make it run
 */

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class RunnerMovement : MonoBehaviour {

	[SerializeField]
	private Animator animator;

	//	Applying the movement
	public void Move(float dir,float speed){
		
		animator.SetFloat ("Direction",dir);
		animator.SetFloat ("Speed", speed);

	}
}
