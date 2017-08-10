﻿using System.Collections;
using UnityEngine;

/// <summary>
///
/// Runner controller.
/// This script has to be attached to the endless runner character to make it run.
///
/// </summary>

[RequireComponent(typeof(RunnerMovement))]
[RequireComponent(typeof(PassengerDetector))]
public class RunnerController : MonoBehaviour {


	[SerializeField]
	private Node currentNode;				//	Current node.

	//	Variables of Speed

	[Range (0f, 50f)]
	[SerializeField]
	private float regularSpeed = 15f;				//	Regular speed of the character.
	[SerializeField]
	private float speedIncrement = 10f;			//	Amount that can increase the speed.
	[SerializeField]
	private float speedDecrement = 15f;			//	Amount that can decrement the speed.

	[SerializeField]
	private float horizontalTime = 0.2f;		//	Time of the transition of the change between right and left.
	[SerializeField]
	private float brakeTime = 0.1f;					//	Time to brake.
	[SerializeField]
	private float accelerationTime = 1f;		//	Time to accelerate;
	[SerializeField]
	private float highSpeedDuration = 2f;		//	Duration of high speed.
	[SerializeField]
	private float lowSpeedDuration = 0.2f;	//	Duration of the brake.
	[SerializeField]
	private float minPassengerSpeed = 10f;	//	Minimum speed to get passenger

	private float speed;										//	Speed of the character.
	private float accel;										//	Acceleration of the character.
	private float targetSpeed;							//	The speed desired.
	private float startSpeed; 							//	The speed before start to accelerate.
	private float startSpeedTime;						//	Time before start to accelerate.
	private float nodeTransition = 0f;			//

	private Vector3 velocityTransition;			//	Velocity of the transition.
	private Vector3 lastPosition;						//	Last position.

	private RaycastHit klaxonHit;

	private bool trackPath = false;

	//	Required Components
	private RunnerMovement movement;			//	Class to move the character.
	private PassengerDetector detector;		//	Class to detect passengers
	private RunnerSoundFX sound;



	// Initialization
	void Start () {
		movement = GetComponent<RunnerMovement>();
		detector = GetComponent<PassengerDetector>();
		sound = GetComponent<RunnerSoundFX> ();

		speed = regularSpeed;
		targetSpeed = regularSpeed;
		startSpeed = regularSpeed;
		accel = accelerationTime;

		GameManager.instance.ChangeStateEvent += StartTrackingPath;
	}

	// Update
	void Update () {

		if (trackPath) {
			//	Reading the inputs.
			ReadInputs ();

			//	If the current node is an edge
			if (currentNode.IsEdge ()) {
				//	Get the incoming position.
				transform.position = GetIncomingPosition ();
				//	Get the incoming rotation.
				transform.rotation = GetIncomingRotation ();
			}

			//	Updating the speed
			speed = Mathf.Lerp (startSpeed, targetSpeed, (Time.time - startSpeedTime) / accel);

			if (speed < minPassengerSpeed && currentNode.GetRightNode () == null)
				detector.LookForAPassenger ();

			movement.Forward (speed / 50f);
		}
	}




	//	Reading Inputs

	private void ReadInputs(){

		#if UNITY_EDITOR
		//	Move to Left.
		if (Input.GetKeyDown (KeyCode.LeftArrow) && currentNode.GetLeftNode () )
			MoveToLeft();
		//	Move to Right.
		if (Input.GetKeyDown (KeyCode.RightArrow) && currentNode.GetRightNode() )
			MoveToRight();
		//	Increase the speed
		if (Input.GetKeyDown(KeyCode.UpArrow) )
			Accelerate();
		//	Decrease the speed
		if (Input.GetKeyDown(KeyCode.DownArrow) )
			Brake();

		if(Input.GetMouseButtonDown(0)){
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out klaxonHit)){
				if(klaxonHit.transform.CompareTag("Passenger")){
					//klaxonHit.transform.GetComponent<PassengerController>().GetAtention(transform.position);
				}
			}
		}
		#endif

		#if UNITY_IOS || UNITY_ANDROID
		//	Update Gesture State
		Gestures.instance.ReadGestures();

		//	Move to Left.
		if (Gestures.instance.swipeState == SwipeState.Left && currentNode.GetLeftNode () )
			MoveToLeft();
		//	Move to right.
		if (Gestures.instance.swipeState == SwipeState.Right && currentNode.GetRightNode () )
			MoveToRight();
		//	Increase the speed
		if (Gestures.instance.swipeState == SwipeState.Up )
			Accelerate();
		//	Decrease the speed
		if (Gestures.instance.swipeState == SwipeState.Down )
			Brake();

		if (Input.touchCount > 0){
			if (Input.touches [0].tapCount == 1) {
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out klaxonHit)){
					if(klaxonHit.transform.CompareTag("Passenger")){
						//klaxonHit.transform.GetComponent<PassengerController>().GetAtention(transform.position);
					}
				}
			}
		}
		#endif
	}
	private void MoveToLeft(){
		//	Change the node.
		currentNode = currentNode.GetLeftNode ();
		movement.SetDirection (0.6f);
		sound.HorizontalFX ();
	}
	private void MoveToRight(){
		//	Change the node.
		currentNode = currentNode.GetRightNode ();
		movement.SetDirection (-0.6f);
		sound.HorizontalFX ();
	}
	private void Brake(){
		StopCoroutine ("ReturnSpeed");
		//	Start transition to brake.
		accel = brakeTime;
		startSpeed = speed;
		targetSpeed = targetSpeed - speedDecrement < 5f ? 5f : targetSpeed - speedDecrement;
		movement.Brake ();
		//	Start coroutine to stop state.
		StartCoroutine("ReturnSpeed",lowSpeedDuration);
		sound.BrakeFX ();
	}
	private void Accelerate(){
		StopCoroutine ("ReturnSpeed");
		//	Start transition to accelerate.
		accel = accelerationTime;
		startSpeed = speed;
		startSpeedTime = Time.time;
		targetSpeed = targetSpeed + speedIncrement > 50f ? 50f : targetSpeed + speedIncrement;
		//	Start coroutine to stop state.
		StartCoroutine("ReturnSpeed",highSpeedDuration);
		sound.RunFX ();
	}
	IEnumerator ReturnSpeed(float timeToWait){
		yield return new WaitForSeconds(timeToWait);
		//	Start transition to back to the regular speed.
		accel = lowSpeedDuration;
		startSpeed = speed;
		startSpeedTime = Time.time;
		targetSpeed = regularSpeed;
	}



	//	Update to the new position.

	private Vector3 GetIncomingPosition(){


		//	Getting the incoming position.
		Vector3 incomingPosition = currentNode.GetPosition (speed, ref nodeTransition);

		//	If the index is grather than the total length change to the incoming position.
		if (nodeTransition >= 1) {
			nodeTransition = 0;
			currentNode = currentNode.GetIncomingNode ();
		}

		//	Applying a transition effect.
		incomingPosition = Vector3.SmoothDamp (
			transform.position,
			incomingPosition,
			ref velocityTransition,
			horizontalTime*(regularSpeed/speed)
		);

		//	Updating the position.
		lastPosition = transform.position;
		return incomingPosition;
	}
	private Quaternion GetIncomingRotation(){
		//	Aligning the current rotation with the trajectory.
		Vector3 forward = (transform.position-lastPosition).normalized;
		Quaternion incomingRotation = Quaternion.LookRotation(forward==Vector3.zero? transform.forward:forward);
		return incomingRotation;
	}

	public void StopTrackingPath(){
		trackPath = false;
	}
	public void StartTrackingPath(){
		if(GameManager.instance.currentState == GameState.PLAYING)
			trackPath = true;
	}

	public Node GetCurrentNode(){
		return currentNode;
	}
	public float GetNodeTransition(){
		return nodeTransition;
	}

}
