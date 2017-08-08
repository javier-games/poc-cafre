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
				Debug.Log("Bajame!!");
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







































/*
public enum PassengerState{ WALKING, WAITING, BOARDING, TRAVELING, WAITING4ARRIVE, ARRIVING, DYING }
public enum PAssengerType{ GENERAL, FILCHER}

public class PassengerController : MonoBehaviour {

	//	Serialized Field Variables.
	[SerializeField] private float		boardingTime = 0.2f;			//	
	[SerializeField] private float		travelingTransitionTime = 0.3f;	//
	[SerializeField] private float		waitingTravelTime = 1f;			//
	[SerializeField] private Vector3	offsetBus;						//
	[SerializeField] private Vector2	navMeshArea;					//
	[SerializeField] private float		updateDestinationTime;			//
	[SerializeField] private float		minTravelDuration;				//
	[SerializeField] private float		maxTravelDuration;				//
	[SerializeField] private float		botherTime = 0.5f;


	private Transform target;
	private Animator animator;
	private PassengerState state;

	private float travelDuration;

	//	Variebles for transition
	private float startTime;
	private float currentBoardingTime;
	private Vector3 starTravelingPosition;
	private Vector3 currentTravelingPosition;

	private Vector3 velocity;


	void Start () {
		animator = GetComponent<Animator>();
		state = PassengerState.WAITING;
		animator.SetTrigger ("Waiting");
		travelDuration = Random.Range (minTravelDuration,maxTravelDuration);

	}
	void Update () {

		switch (state) {
		case PassengerState.BOARDING:

			transform.position = Vector3.SmoothDamp (transform.position, target.position, ref velocity, currentBoardingTime);
			currentBoardingTime = Mathf.Lerp (boardingTime, 0.01f, (Time.time - startTime) / boardingTime);
			transform.rotation = Quaternion.LookRotation (target.forward);
			if (currentBoardingTime <= 0.01)
				StartCoroutine ("Travel",waitingTravelTime);
			break;

		case PassengerState.TRAVELING:
			transform.position = Vector3.SmoothDamp (transform.position, currentTravelingPosition, ref velocity, currentBoardingTime);
			currentTravelingPosition = Vector3.Lerp (target.position, target.position + target.TransformVector (offsetBus), (Time.time - startTime) / travelingTransitionTime);
			if ((Time.time - startTime) / travelingTransitionTime > 1f) {
				state = PassengerState.WAITING4ARRIVE;
				//ObjectPool.instance.PoolGameObject (transform.gameObject);//Destroy (transform.gameObject);
			}
			break;

		}
	}


	public void InitialState(PassengerState state){
		this.state = state;
		transform.GetComponent<Collider> ().enabled= true;
	}
	public void Take(Transform target,out PassengerController passenger){
		passenger = null;
		if (state == PassengerState.WAITING){
			passenger = this;
			this.target = target;
			startTime = Time.time;
			currentBoardingTime = boardingTime;
			transform.GetComponent<Collider> ().enabled= false;
			animator.SetTrigger ("Boarding");
			state = PassengerState.BOARDING;
			//StartCoroutine ("SetToTraveling");
		}
	}
	IEnumerator Travel(float timeToWait){
		yield return new WaitForSeconds (timeToWait);
		if (state == PassengerState.BOARDING) {
			startTime = Time.time;
			animator.SetTrigger ("Traveling");
			state = PassengerState.TRAVELING;
			currentTravelingPosition = target.position;
			Debug.Log (Time.time);
		}

	}

}
*/
/*

public enum Passen		gerState{ WALKING, WAITING, BOARDING, TRAVELING, DYING }
public enum PAssengerType{ GENERAL, FILCHER}

public class PassengerController : MonoBehaviour {

	//	Serialized Field Variables.
	[SerializeField] private float		boardingTime = 0.2f;			//	
	[SerializeField] private float		travelingTransitionTime = 0.3f;	//
	[SerializeField] private float		waitingTravelTime = 1f;			//
	[SerializeField] private Vector3	offsetBus;						//
	[SerializeField] private Vector2	navMeshArea;					//
	[SerializeField] private float		updateDestinationTime;


	private Transform target;
	private Animator animator;
	private NavMeshAgent agent;
	private PassengerState state;

	private float destinationMagnitude;

	//	Variebles for transition
	private float startTime;
	private float currentBoardingTime;
	private Vector3 starTravelingPosition;
	private Vector3 currentTravelingPosition;

	private Vector3 velocity;


	void Start () {
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent> ();

		if (agent != null) {
			InitialState (PassengerState.WALKING);
			StartCoroutine ("CalculateDestination", updateDestinationTime);
		} else {
			animator.SetTrigger ("Waiting");
		}

	}
	void Update () {

		switch (state) {
		case PassengerState.BOARDING:

			transform.position = Vector3.SmoothDamp (transform.position, target.position, ref velocity, currentBoardingTime);
			currentBoardingTime = Mathf.Lerp (boardingTime, 0.01f, (Time.time - startTime) / boardingTime);
			transform.rotation = Quaternion.LookRotation (target.forward);
			if (currentBoardingTime <= 0.01)
				StartCoroutine ("Travel");
			break;

		case PassengerState.TRAVELING:
			transform.position = Vector3.SmoothDamp (transform.position, currentTravelingPosition, ref velocity, currentBoardingTime);
			currentTravelingPosition = Vector3.Lerp (target.position, target.position + target.TransformVector (offsetBus), (Time.time - startTime) / travelingTransitionTime);
			if ((Time.time - startTime) / travelingTransitionTime > 1f)
				ObjectPool.instance.PoolGameObject (transform.gameObject);//Destroy (transform.gameObject);
			break;

		case PassengerState.WALKING:
			
			if (Vector3.Distance (agent.destination, transform.position) < 0.1f)
				animator.SetFloat ("Speed", 0f);
			else {
				animator.SetFloat ("Speed", 1f);
			}
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
	IEnumerator CalculateDestination(float timeToWait){
		yield return new WaitForSeconds (timeToWait);
		agent.SetDestination (new Vector3 (
			Random.Range(transform.position.x-navMeshArea.x,transform.position.x+navMeshArea.x),
			0,
			Random.Range(transform.position.y-navMeshArea.y,transform.position.y+navMeshArea.y)
		));
		if (state == PassengerState.WALKING)
			StartCoroutine ("CalculateDestination",updateDestinationTime);
	}

}

*/