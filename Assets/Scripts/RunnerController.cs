using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RunnerMovement))]
public class RunnerController : MonoBehaviour {

	[SerializeField]
	private Node activeNode;
	[Range (-5f, 50f)]
	[SerializeField]
	private float speed=30f;

	[SerializeField]
	private float transitionTime = 1f;
	private float startTransitionTime = 0;
	private float currentTime = 0;

	private bool left = false;
	private bool right = false;

	private RunnerMovement movement;

	// Use this for initialization
	void Start () {
		movement = GetComponent<RunnerMovement>();
	}

	// Update is called once per frame
	void Update () {

		ReadInputs ();

		Vector3 position;
		Quaternion rotation;

		if (activeNode.IsEdge ()) {

			bool nextNode = false;
			if (speed >= 0f) {
				position = activeNode.GetPosition (speed, out nextNode);
			} else {
				position = activeNode.GetPosition (0f, out nextNode);
			}

			if (nextNode) {
				activeNode.ResetIndex ();
				activeNode = activeNode.GetIncomingNode ();
			}

			rotation = activeNode.GetRotation ();

		}
		else{
			position = transform.position;
			rotation = transform.rotation;
		}

		if (left && activeNode.GetLeftNode ()) {
			
			currentTime = Time.time - startTransitionTime;
			float index = currentTime / transitionTime;
			float indexTime = activeNode.GetIndexTime ();

			Vector3 leftPosition = activeNode.GetLeftNode ().GetPosition (indexTime);
			position = Vector3.Lerp (position,leftPosition,index);

			if (index > 1) {
				activeNode.ResetIndex ();
				activeNode = activeNode.GetLeftNode ();
				activeNode.SetIndexTime (indexTime);
				left = false;
				currentTime = 0;
				startTransitionTime = 0;
			}

		}
		if (right && activeNode.GetRightNode ()) {
			
			currentTime = Time.time - startTransitionTime;
			float index = currentTime / transitionTime;
			float indexTime = activeNode.GetIndexTime ();

			Vector3 rightPosition = activeNode.GetRightNode ().GetPosition (indexTime);
			position = Vector3.Lerp (position,rightPosition,index);

			if (index > 1) {
				activeNode.ResetIndex ();
				activeNode = activeNode.GetRightNode ();
				activeNode.SetIndexTime (indexTime);
				right = false;
				currentTime = 0;
				startTransitionTime = 0;
			}
		}
			
		transform.position = new Vector3(position.x,transform.position.y,position.z);
		transform.rotation = rotation;

		movement.Move (
			GetDirection(transform.forward,activeNode.transform.forward,activeNode.GetIncomingNode().transform.forward),
			GetSpeed()
		);


	}


	private float GetDirection(Vector3 currentDirection,Vector3 currentNodeDirection, Vector3 incomingNodeDirection){
		float totalAngle = Vector3.Angle (currentNodeDirection,incomingNodeDirection);
		float angle = Vector3.Angle (currentNodeDirection,currentDirection);
		angle = ConvertRange (0f,totalAngle,0f,180f,angle);
		return Mathf.Sin(angle*Mathf.Deg2Rad);
	}
	public float ConvertRange( float originalStart, float originalEnd, float newStart, float newEnd, float value){
		float scale = originalEnd - originalStart;
		if (scale != 0)
			scale = (newEnd - newStart) / (scale);
		return (newStart + ((value - originalStart) * scale));
	}
	public float GetSpeed(){
		return (speed / 50f);
	}

	private void ReadInputs(){
		if (Input.GetKeyDown (KeyCode.LeftArrow) && !right && activeNode.GetLeftNode () ) {
			startTransitionTime = Time.time;
			left = true;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow) && !left && activeNode.GetRightNode () ) {
			startTransitionTime = Time.time;
			right = true;
		}
	}

}