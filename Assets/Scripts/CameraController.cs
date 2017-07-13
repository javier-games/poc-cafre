using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController: MonoBehaviour {

	[SerializeField]
	private Transform target;
	[SerializeField]
	private float smooth = 0.3f;
	[SerializeField]
	private float distance = 5.0f;
	[SerializeField]
	private float height = 3.5f;
	[SerializeField]
	private float offset;

	private float yVelocity = 30f;

	void Start(){

	}

	void LateUpdate () 
	{
		float yAngle = Mathf.SmoothDampAngle (
			transform.eulerAngles.y,
			target.eulerAngles.y,
			ref yVelocity,
			smooth
		);

		Vector3 position = target.position;

		position += Quaternion.Euler (0,yAngle, 0) *  new Vector3(0,height, -distance);

		transform.position = position;

		transform.LookAt (target.position-new Vector3(0,offset,0));
	}
}
