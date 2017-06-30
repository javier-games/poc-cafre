using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverController : MonoBehaviour {
	
	[SerializeField]
	private Node activeNode;

	[Range (10f, 80f)]
	[SerializeField]
	private float speed=30f;

	[Range (0f, 1f)]
	[SerializeField]
	private float smooth = 0.1f;

	[SerializeField]
	private bool left = false;
	[SerializeField]
	private bool right = false;


	private Vector3 currentVelocity;

	// Use this for initialization
	void Start () {
		currentVelocity = new Vector3 (0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 suggestedPosition;
		Quaternion suggestedRotation;
		if (left || right) {
		}
		if (activeNode.IsEdge ()) {
			
			bool nextNode = false;

			suggestedPosition = Vector3.SmoothDamp (
				transform.position,
				activeNode.GetPoint (speed, out nextNode),
				ref currentVelocity,
				smooth
			);

			if (nextNode) {
				activeNode.ResetIndex ();
				activeNode = activeNode.GetIncomingNode (0);
			}

			suggestedRotation = activeNode.GetRotation (0);

		}
		else{
			suggestedPosition = transform.position;
			suggestedRotation = transform.rotation;
		}
		transform.position = new Vector3(suggestedPosition.x,transform.position.y,suggestedPosition.z);
		transform.rotation = suggestedRotation;

	}
		
}
