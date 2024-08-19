using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
///
/// Passenger detector.
/// This script helps the bus to look for Passengers.
///
/// </summary>

[RequireComponent(typeof(CoinFactory))]
[RequireComponent(typeof(RunnerSoundFX))]
public class PassengerDetector : MonoBehaviour {

	[SerializeField]
	private Vector3 offset;				//	Offset of the origin of the ray
	[SerializeField]
	private float maxDistance;			//	Maximum distance of the ray
	[SerializeField]
	private LayerMask passengerMask;	//	Layer of the passengers
	[SerializeField]
	private float releaseOffsetTime = 1.5f;

	private RaycastHit passengerHit;	//	Info of the passenger reached by the ray.
	private List<PassengerController> passengers;

	private CoinFactory factory;		//	System to animate a toss.
	private RunnerSoundFX sound;

	//	Initializing
	void Start(){
		factory = GetComponent<CoinFactory> ();
		sound = GetComponent<RunnerSoundFX> ();
		passengers = new List<PassengerController> ();
	}

	public void LookForAPassenger(){

		#if UNITY_EDITOR
		//	Drawing the ray
		Debug.DrawLine(transform.position + offset, transform.position + offset + transform.right*maxDistance,Color.cyan);
		#endif
		//	Trigger the ray.
		if (Physics.Raycast (transform.position + offset, transform.right, out passengerHit, maxDistance, passengerMask)) {
			PassengerController passenger;
			passengerHit.transform.GetComponent<PassengerController> ().Take(transform, out passenger);
			if (passenger != null) {
				passengers.Add (passenger);
				StartCoroutine (AirBrakeRelease());
			}
		}

		if(passengers.Count>0){
			for (int i = 0; i < passengers.Count; i++) {
				if (passengers [i].Arrive ()) {
					factory.TossCoins (1,passengers [i].GetAmountOfCoins(),0.1f);
					passengers.RemoveAt (i);

					StartCoroutine (AirBrakeRelease());
				}
			}
		}
	}

	IEnumerator AirBrakeRelease(){
		yield return new WaitForSeconds (releaseOffsetTime);
		sound.TakePassengerFX();
	}

}
