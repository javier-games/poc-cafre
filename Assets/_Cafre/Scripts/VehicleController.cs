﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {

	[SerializeField]
	private Node currentNode;				//	Current node.
	[SerializeField]
	private float regularSpeed = 15f;		//	Regular speed of the character
	[SerializeField]
	private float horizontalTime = 0.2f;	//	Time of the transition of the change between right and left.
	[SerializeField]
	private float limitDistance = 15;
	[SerializeField]
	private float timeToPool = 2.0f;

	private Transform target;

	private float nodeTransition;
	private float speed;					//	Speed of the character.
	private Vector3 velocityTransition;		//	Velocity of the transition.
	private Vector3 lastPosition;			//	Last position.
	private bool trackPath = true;
	private RaycastHit vehicleHit;

	void Start(){
		speed = regularSpeed;
	}

	void Update(){
		if (currentNode != null) {
			if (
				currentNode.IsEdge () && 
				trackPath &&
				Vector3.Distance(transform.position,target.position)<limitDistance
			) {
				//	Get the incoming position.
				transform.position = GetIncomingPosition ();
				//	Get the incoming rotation.
				transform.rotation = GetIncomingRotation ();
			} else {
				StartCoroutine ("GetBackToPool");
			}
		}
	}

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
		Quaternion incomingRotation = Quaternion.LookRotation((transform.position-lastPosition).normalized);
		return incomingRotation;
	}

	public void SetUp(Node currentNode,float nodeTransition,Transform target){
		this.currentNode = currentNode;
		this.nodeTransition = nodeTransition;
		this.target = target;
	}

	void OnCollisionEnter(Collision other){
		if (other.transform.CompareTag ("Player")) {
			trackPath = false;
		}
		if (other.transform.CompareTag ("Vehicle")) {
			if (currentNode.GetLeftNode () != null) {
				currentNode = currentNode.GetLeftNode ();
			} else if (currentNode.GetRightNode () != null) {
				currentNode = currentNode.GetRightNode ();
			} else { 
				nodeTransition--;
			}
		}
	}

	IEnumerator GetBackToPool(){
		yield return new WaitForSeconds(timeToPool);
		ObjectPool.instance.PoolGameObject (transform.gameObject);
	}



}
