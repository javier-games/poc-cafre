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

	private RaycastHit passengerHit;	//	Info of the passenger reached by the ray.

	public bool LookForAPassenger(){

		#if UNITY_EDITOR
		//	Drawing the ray
		Debug.DrawRay(transform.position + offset, transform.right,Color.cyan);
		#endif
		//	Trigger the ray.
		if (Physics.Raycast (transform.position + offset, transform.right, out passengerHit, maxDistance, passengerMask)) {
			Debug.Log ("Passenger Detected at " + passengerHit.distance);
			return true;
		}

		return false;
	}

}
