using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// Passenger state.
/// Class to control the states of the passenger.
/// 
/// </summary>


public enum PassengerState{ WALKING, WAITING, BOARDING, TRAVELING, BOTHERING, ARRIVING, DYING }
public enum PAssengerType{ GENERAL, FILCHER}

public class PassengerController : MonoBehaviour {

	[SerializeField] int   amountOfCoins = 5;
	[SerializeField] float boardingSmooth = 0.1f;
	[SerializeField] float boardingDuration = 1.5f;
	[SerializeField] float travelingSmooth = 0.8f;
	[SerializeField] float travelingDuration = 10f;
	[SerializeField] float botheringDecrement = 0.3f;
	[SerializeField] float botheringDuration = 2f;
	[SerializeField] float arrivingDuration;
	[SerializeField] Vector3 offsetHandle;


	private PassengerState currentState;
	private Animator animator;
	private Transform target;
	private Vector3 arrivePoint;

	private float startTime;
	private float currentTime;

	private Vector3 velocity;

	void Start(){
		animator = GetComponent<Animator> ();
		SetState (PassengerState.WAITING);
	}

	void Update(){

		currentTime = Time.time - startTime;

		switch (currentState){

		case PassengerState.WAITING:
			break;

		case PassengerState.BOARDING:
			Vector3 handle = target.position + target.TransformVector (offsetHandle);
			transform.position = Vector3.SmoothDamp (transform.position, handle, ref velocity, boardingSmooth);
			if (currentTime/boardingDuration > 1f) {
				SetState (PassengerState.TRAVELING);
			}
			break;

		case PassengerState.TRAVELING:
			Vector3 center = target.position + target.TransformVector (offsetHandle.z * Vector3.forward);
			transform.position = Vector3.SmoothDamp (transform.position, center, ref velocity, travelingSmooth);
			if (currentTime/travelingDuration > 1f) {
				SetState (PassengerState.BOTHERING);
			}
			break;

		case PassengerState.BOTHERING:

			if (currentTime/botheringDuration > 1f) {
				AnnoyingCanvas.instance.SendAMessage ();
				botheringDuration = botheringDuration - botheringDecrement < 0 ? botheringDecrement :botheringDuration-botheringDecrement ;
				SetState (PassengerState.BOTHERING);
			}

			break;

		case PassengerState.ARRIVING:
			transform.position = Vector3.SmoothDamp (transform.position, arrivePoint, ref velocity, travelingSmooth);
			transform.LookAt (arrivePoint);
			StartCoroutine (PoolPassenger(arrivingDuration));
			break;

		case PassengerState.DYING:
			break;
		}
	}

	private void SetState(PassengerState newState){

		switch (newState){

		case PassengerState.WAITING:

			animator.SetTrigger ("Waiting");
			currentState = newState;
			startTime = Time.time;

			break;

		case PassengerState.BOARDING:
			
			transform.GetComponent<Collider> ().enabled = false;
			animator.SetTrigger ("Boarding");
			currentState = newState;
			startTime = Time.time;

			break;

		case PassengerState.TRAVELING:

			animator.SetTrigger ("Traveling");
			currentState = newState;
			startTime = Time.time;

			break;

		case PassengerState.BOTHERING:
			
			currentState = newState;
			startTime = Time.time;

			break;

		case PassengerState.ARRIVING:

			animator.SetTrigger ("Arriving");
			arrivePoint = target.position + target.TransformVector (offsetHandle.z *0.7f* Vector3.back + offsetHandle.x *4f* Vector3.right);
			currentState = newState;

			break;

		case PassengerState.DYING:
			break;
		}

	}

	public void Take(Transform target,out PassengerController passenger){
		passenger = this;
		this.target = target;
		transform.parent = target;
		SetState (PassengerState.BOARDING);
	}
	public bool Arrive(){
		if (currentState == PassengerState.BOTHERING) {
			SetState (PassengerState.ARRIVING);
			return true;
		}
		return false;
	}
	public int GetAmountOfCoins(){
		return amountOfCoins;
	}
	IEnumerator PoolPassenger(float timeToWait){
		yield return new WaitForSeconds(timeToWait);
		ObjectPool.instance.PoolGameObject (transform.gameObject);
	}

}
