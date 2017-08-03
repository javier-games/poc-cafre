using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassengerState{ WALKING, WAITING, BOARDING, TRAVELING, DYING}

public class PassengerController : MonoBehaviour {

	[SerializeField]
	private float boardingTime = 0.2f;
	[SerializeField]
	private float travelingTransitionTime = 0.3f;
	[SerializeField]
	private float waitingTravelTime = 1f;
	[SerializeField]
	private Vector3 offsetBus;

	private Transform target;
	private Animator animator;
	private PassengerState state;

	private float startTime;
	private float currentBoardingTime;
	private Vector3 starTravelingPosition;
	private Vector3 currentTravelingPosition;

	private Vector3 velocity;

	void Start () {
		animator = GetComponent<Animator>();
		state = PassengerState.WAITING;
	}
	void Update () {


		switch (state) {
		case PassengerState.BOARDING:

			transform.position = Vector3.SmoothDamp (transform.position, target.position, ref velocity, currentBoardingTime);
			currentBoardingTime = Mathf.Lerp (boardingTime, 0.01f, (Time.time - startTime) / boardingTime);
			transform.rotation = Quaternion.LookRotation (target.forward);
			if (currentBoardingTime <= 0.01) {
				StartCoroutine ("Travel");
			}
				
			
			break;

		case PassengerState.TRAVELING:
			transform.position = Vector3.SmoothDamp (transform.position, currentTravelingPosition, ref velocity, currentBoardingTime);
			currentTravelingPosition = Vector3.Lerp (target.position, target.position + target.TransformVector (offsetBus), (Time.time - startTime) / travelingTransitionTime);
			if((Time.time - startTime) / travelingTransitionTime >1f)
				Destroy (transform.gameObject);
			break;
		}
	}


	public void InitialState(PassengerState state){
		this.state = state;
		transform.GetComponent<Collider> ().enabled= true;
	}
	public void Take(Transform target){
		if (state == PassengerState.WAITING){
			this.target = target;
			startTime = Time.time;
			currentBoardingTime = boardingTime;
			transform.GetComponent<Collider> ().enabled= false;
			animator.SetTrigger ("Boarding");
			state = PassengerState.BOARDING;
			//StartCoroutine ("SetToTraveling");
		}
	}
	IEnumerator Travel(){
		yield return new WaitForSeconds(waitingTravelTime);
		if (state == PassengerState.BOARDING) {
			startTime = Time.time;
			animator.SetTrigger ("Traveling");
			state = PassengerState.TRAVELING;
			currentTravelingPosition = target.position;
		}
	}

}
