using UnityEngine;

/*
 * The porpose of this script is to follow a target in an smoothy way and adjust it position.
*/

public class CameraController: MonoBehaviour {

	//	Variables

	[SerializeField]
	private Transform target;			//	Target to follow.
	[SerializeField]
	private float smooth = 0.3f;		//	Smooth to move the camera.
	[SerializeField]
	private float distance = 5.0f;		//	Distance in Z between target and camera.
	[SerializeField]
	private float height = 3.5f;		//	Distance in Y bewteen target and camera.
	[SerializeField]
	private float offset;				//	Offset to move the target out of the center.
	[SerializeField]
	private bool LookAt = true;

	private float yVelocity = 30f;		//	Variable required by the SmoothDampAngle.


	//	Update Method

	//	LateUpdate have to be called to avoid a horrible shaking effect in the camera.
	void LateUpdate () {

		//	Geting the Smooth Damb Angle.
		float yAngle = Mathf.SmoothDampAngle (
			transform.eulerAngles.y,
			target.eulerAngles.y,
			ref yVelocity,
			smooth
		);

		//	Applying a smooth movement to the position.
		Vector3 position = target.position;
		position += Quaternion.Euler (0,yAngle, 0) *  new Vector3(0,height, -distance);
		transform.position = position;

		//	Rotating the camero to the target.
		if(LookAt)
			transform.LookAt (target.position-new Vector3(0,offset,0));
	}
}
