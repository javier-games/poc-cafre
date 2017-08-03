using UnityEngine;

/*
 * 	This script helps the bus to look for Passengers.
*/

public class PassengerDetector : MonoBehaviour {

	[SerializeField]
	private Vector3 offset;				//	Offset of the origin of the ray
	[SerializeField]
	private float maxDistance;			//	Maximum distance of the ray
	[SerializeField]
	private LayerMask passengerMask;	//	Layer of the passengers
	[SerializeField]
	private Transform doorHandle;

	private RaycastHit passengerHit;	//	Info of the passenger reached by the ray.

	public void LookForAPassenger(){

		#if UNITY_EDITOR
		//	Drawing the ray
		Debug.DrawLine(transform.position + offset, transform.position + offset + transform.right*maxDistance,Color.cyan);
		#endif
		//	Trigger the ray.
		if (Physics.Raycast (transform.position + offset, transform.right, out passengerHit, maxDistance, passengerMask)) {
			passengerHit.transform.GetComponent<PassengerController> ().Take(doorHandle);
		}
	}

}
