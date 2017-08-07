using System.Collections;
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
	[SerializeField]
	private CoinFactory coins;				//	Factory of coins;

	//	Variables of Speed

	[Range (0f, 50f)]
	[SerializeField]
	private float regularSpeed = 15f;		//	Regular speed of the character.
	[SerializeField]
	private float speedIncrement = 10f;		//	Amount that can increase the speed.
	[SerializeField]
	private float speedDecrement = 15f;		//	Amount that can decrement the speed.

	[SerializeField]
	private float horizontalTime = 0.2f;	//	Time of the transition of the change between right and left.
	[SerializeField]
	private float brakeTime = 0.1f;			//	Time to brake.
	[SerializeField]
	private float accelerationTime = 1f;	//	Time to accelerate;
	[SerializeField]
	private float highSpeedDuration = 2f;	//	Duration of high speed.
	[SerializeField]
	private float lowSpeedDuration = 0.2f;	//	Duration of the brake.
	[SerializeField]
	private float minPassengerSpeed = 10f;	//	Minimum speed to get passenger
	[SerializeField]
	private float speed;					//	Speed of the character.
	private float accel;					//	Acceleration of the character.
	private float targetSpeed;				//	The speed desired.
	private float startSpeed; 				//	The speed before start to accelerate.
	private float startSpeedTime;			//	Time before start to accelerate.

	private Vector3 velocityTransition;		//	Velocity of the transition.
	private Vector3 lastPosition;			//	Last position.

	private bool trackPath = true;

	//	Required Components
	private RunnerMovement movement;		//	Class to move the character.
	private PassengerDetector detector;		//	Class to detect passengers



	// Initialization
	void Start () {
		movement = GetComponent<RunnerMovement>();
		detector = GetComponent<PassengerDetector>();

		speed = regularSpeed;
		targetSpeed = regularSpeed;
		startSpeed = regularSpeed;
		accel = accelerationTime;
	}

	// Update
	void Update () {
		

		//	Reading the inputs.
		ReadInputs ();

		//	If the current node is an edge
		if(currentNode.IsEdge () && trackPath){
			//	Get the incoming position.
			transform.position = GetIncomingPosition ();
			//	Get the incoming rotation.
			transform.rotation = GetIncomingRotation ();
		}
			
		//	Updating the speed
		speed =  Mathf.Lerp(startSpeed,targetSpeed,(Time.time - startSpeedTime)/accel );

		if (speed < minPassengerSpeed)
			detector.LookForAPassenger ();

		movement.Forward( speed/ 50f);
<<<<<<< HEAD

		if (Input.GetKeyDown (KeyCode.Space)) {
			coins.TossCoins (0,1,0.1f);
		}
=======
>>>>>>> Optimization
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
		#endif

	}
	private void MoveToLeft(){
		//	Update the index of thenew current node.
		currentNode.GetLeftNode ().SetIndexTime (currentNode.GetIndexTime ());
		//	Reset the index.
		currentNode.ResetIndexTime();
		//	Change the node.
		currentNode = currentNode.GetLeftNode ();
		movement.SetDirection (0.6f);
	}
	private void MoveToRight(){
		//	Update the index of thenew current node.
		currentNode.GetRightNode ().SetIndexTime (currentNode.GetIndexTime ());
		//	Reset the index.
		currentNode.ResetIndexTime();
		//	Change the node.
		currentNode = currentNode.GetRightNode ();
		movement.SetDirection (-0.6f);
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
		
		bool changeNode = false;	//	Variable used to know if it is necesary to change the node.
		//	Getting the incoming position.
		Vector3 incomingPosition = currentNode.GetPosition (speed,out changeNode);
		if (changeNode) {
			currentNode.ResetIndexTime ();
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
		Quaternion incomingRotation = Quaternion.LookRotation((transform.position-lastPosition).normalized);
		return incomingRotation;
	}

	public void StopTrackingPath(){
		trackPath = false;
	}
}