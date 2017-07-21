using UnityEngine;

/*
 *	This script has to be attached to the endless runner character to make it run.
 */

[RequireComponent(typeof(RunnerMovement))]
public class RunnerController : MonoBehaviour {


	[SerializeField]
	private Node currentNode;				//	Current node.
	[Range (0f, 50f)]
	[SerializeField]
	private float speed=30f;				//	Speed of the character.
	//private float dir = 0f;				//	Direction of the character.

	//	Transition
	[Range (0.1f, 2f)]
	[SerializeField]
	private float	transitionTime = 1f;	//	Time of the transition of the change between right and left.
	private Vector3 velocityTransition;		//	Velocity of the transition.
	private Vector3 lastPosition;			//	Last position.

	//	Required Components
	private RunnerMovement movement;		//	Class to move the character.



	// Initialization
	void Start () {
		movement = GetComponent<RunnerMovement>();
		velocityTransition = Vector3.zero;
	}


	// Update
	void Update () {

		//	Reading the inputs.
		ReadInputs ();

		//	If the current node is an edge get the incoming position.
		transform.position = currentNode.IsEdge () ? GetIncomingPosition () : transform.position;
		//	If the current node is an edge get the incoming rotation.
		transform.rotation = currentNode.IsEdge () ? GetIncomingRotation () : transform.rotation;

		//	Mode the character with the direction and the speed.
		movement.Move (GetDirection(),speed/50f);
	}

	//	Reading Inputs
	private void ReadInputs(){
		//	Move to Left.
		if (Input.GetKeyDown (KeyCode.LeftArrow) && currentNode.GetLeftNode () ) {
			//	Update the index of thenew current node.
			currentNode.GetLeftNode ().SetIndexTime (currentNode.GetIndexTime ());
			//	Reset the index.
			currentNode.ResetIndexTime();
			//	Change the node.
			currentNode = currentNode.GetLeftNode ();
		}
		//	Move to right.
		if (Input.GetKeyDown (KeyCode.RightArrow) && currentNode.GetRightNode () ) {
			//	Update the index of thenew current node.
			currentNode.GetRightNode ().SetIndexTime (currentNode.GetIndexTime ());
			//	Reset the index.
			currentNode.ResetIndexTime();
			//	Change the node.
			currentNode = currentNode.GetRightNode ();
		}
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
			transitionTime
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

	//	Update to the new direction.
	private float GetDirection(){
		//	Getting the angle bewteen the current node and the incoming node.
		float totalAngle = Vector3.Angle (currentNode.transform.forward,currentNode.GetIncomingNode().transform.forward);
		//	Getting the angle bewteen the current node and the current rotation.
		float angle = Vector3.Angle (currentNode.transform.forward,transform.forward);
		//	A little mapping.
		angle = ConvertRange (0f,totalAngle,0f,180f,angle);
		//	Using a Sin functiong give us a number in the range of one and zero.
		return Mathf.Sin(angle*Mathf.Deg2Rad); 
	} 
	public float ConvertRange( float originalStart, float originalEnd, float newStart, float newEnd, float value){ 
		float scale = originalEnd - originalStart; 
		if (scale != 0) 
			scale = (newEnd - newStart) / (scale); 
		return (newStart + ((value - originalStart) * scale)); 
	} 


}