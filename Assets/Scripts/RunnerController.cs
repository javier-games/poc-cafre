using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RunnerMovement))]
public class RunnerController : MonoBehaviour {


	[SerializeField]
	private Node currentNode;
	[Range (0f, 50f)]
	[SerializeField]
	private float speed=30f;

	//	Transition
	[SerializeField]
	private float	transitionTime = 1f;
	private Vector3 velocityTransition;

	//	Required Components
	private RunnerMovement movement;



	// Initialization

	void Start () {
		movement = GetComponent<RunnerMovement>();
		velocityTransition = Vector3.zero;
	}

	// Update
	void Update () {

		ReadInputs ();

		transform.position = currentNode.IsEdge () ? GetIncomingPosition () : transform.position;
		transform.rotation = currentNode.IsEdge () ? GetIncomingRotation () : transform.rotation;

		movement.Move (0,0);
	}
		
	private void ReadInputs(){
		if (Input.GetKeyDown (KeyCode.LeftArrow) && currentNode.GetLeftNode () ) {
			currentNode.GetLeftNode ().SetIndexTime (currentNode.GetIndexTime ());
			currentNode.ResetIndexTime();
			currentNode = currentNode.GetLeftNode ();
		}
		if (Input.GetKeyDown (KeyCode.RightArrow) && currentNode.GetRightNode () ) {
			currentNode.GetRightNode ().SetIndexTime (currentNode.GetIndexTime ());
			currentNode.ResetIndexTime();
			currentNode = currentNode.GetRightNode ();
		}
	}

	private Vector3 GetIncomingPosition(){
		
		bool changeNode = false;
		Vector3 incomingPosition = currentNode.GetPosition (speed,out changeNode);
		if (changeNode) {
			currentNode.ResetIndexTime ();
			currentNode = currentNode.GetIncomingNode ();
		}
			
		incomingPosition = Vector3.SmoothDamp (
			transform.position,
			incomingPosition,
			ref velocityTransition,
			transitionTime
		);
		return incomingPosition;

	}

	private Quaternion GetIncomingRotation(){
		Quaternion incomingRotation = currentNode.GetRotation ();
		return incomingRotation;
	}


}