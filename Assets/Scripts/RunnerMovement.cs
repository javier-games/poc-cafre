using System.Collections;
using UnityEngine;

/*
 *	This script has to be attached to the endless runner character to make it run
 */

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class RunnerMovement : MonoBehaviour {

	[SerializeField]
	private Animator animator;						//	Animator of the character.
	[SerializeField]
	private float speedTurnBack = 0.3f;				//	Speed to turn back.
	[SerializeField]
	private float decreaseDirectionAmount = 0.2f;	//	Amount to decrease the direction.

	private float direction = 0f;

	//	Forward Movement.
	public void Forward(float speed){
		animator.SetFloat ("Speed", speed);

	}

	//	Brake Animation.
	public void Brake(){
		animator.SetTrigger("Brake");
	}

	//	Set the direction of the animator.
	public void SetDirection(float direction){
		StopCoroutine ("TurnBack");
		this.direction = direction;
		animator.SetFloat ("Direction", direction);
		//	Start Coroutine to turn back the direction.
		StartCoroutine ("TurnBack", direction);
	}

	// Coroutine to turn back the direction.
	IEnumerator TurnBack(float multiplier){
		yield return new WaitForSeconds (speedTurnBack);

		//	Decrease direction.
		direction -= multiplier * decreaseDirectionAmount;
		//	Stop if it turned right
		if (direction <= 0 && multiplier >= 0) {
			direction = 0;
		} 
		//	Or stop if it turned left
		else if (direction > 0 && multiplier < 0) {
			direction = 0;
		}else{
			animator.SetFloat ("Direction", direction);
			StartCoroutine ("TurnBack",multiplier);
		}
	}
}
