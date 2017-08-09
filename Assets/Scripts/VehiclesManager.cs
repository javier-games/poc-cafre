using UnityEngine;
using System.Collections.Generic;

public enum VehiclePreposition{INFRONT, BEHIND, RANDOM}
public class VehiclesManager : MonoBehaviour {

	//	Singleton
	public static VehiclesManager instance;

	//	Serialize Field Variables
	[SerializeField]
	private RunnerController mainVehicle;
	[SerializeField]
	private float inFrontOffset = 1f;
	[SerializeField]
	private float behindOffset  = 1f;
	[SerializeField]
	private int accuaracy = 20;


	private float distance = 0f;
	private float currentNodeTransition = 0f;
	private Vector3 currentComputedPosition;
	private Vector3 lastComputedPosition;

	//	Singleton Initialization
	void Awake(){
		instance = this;
	}

	//	Method to put a car
	public void PutACar(VehiclePreposition preposition, List<string> iDs){

		switch (preposition) {
			
		case VehiclePreposition.INFRONT:

			currentNodeTransition = mainVehicle.GetNodeTransition ();
			distance = 0;
			CheckDistance(iDs,1,
				mainVehicle.GetCurrentNode (),
				mainVehicle.GetCurrentNode ().GetIncomingNode (),
				currentNodeTransition, inFrontOffset
			);

			break;

		case VehiclePreposition.BEHIND:
			
			currentNodeTransition = mainVehicle.GetNodeTransition ();
			distance = 0;
			CheckDistance(iDs,-1,
				mainVehicle.GetCurrentNode (),
				mainVehicle.GetCurrentNode ().GetIncomingNode (),
				currentNodeTransition,
				behindOffset
			);

			break;

		case VehiclePreposition.RANDOM:
			PutACar (
				(VehiclePreposition)Random.Range (
					0,System.Enum.GetValues (typeof(VehiclePreposition)).Length
				), iDs
			);
			break;
		}
	}

	private void CheckDistance( List<string> iDs, int sense, Node currentNode, Node incomingNode , float transition, float offset){

		if (currentNode.IsEdge ()) {
			for (int i = 0; i < accuaracy; i++ ) {

				float vehicleTransition = sense > 0 ?
					transition + i * (1f - transition) / (accuaracy * 1f) :
					transition - i * (transition) / (accuaracy * 1f);

				currentComputedPosition = currentNode.GetPosition (currentNode,incomingNode,vehicleTransition);

				//	Calculating the distance between steps.
				if (i > 0 ) {
					distance += Vector3.Distance (lastComputedPosition, currentComputedPosition);
					if ( distance > offset) {
						distance = 0;
						GameObject vehicle = ObjectPool.instance.GetGameObjectOfType (
							iDs[Random.Range(0,iDs.Count)],
							true,
							TypeOfID.PROP
						);
						vehicle.GetComponent<VehicleController> ().SetUp (currentNode,vehicleTransition,mainVehicle.transform);
						vehicle.transform.position = currentComputedPosition;
						break;
					}
				} 
				lastComputedPosition = currentComputedPosition;
				if (i == accuaracy - 1 && sense > 0) {
					distance = 0;
					CheckDistance (iDs,sense,incomingNode,incomingNode.GetIncomingNode(),0f,offset-distance);
				}
			}

		}

	}

}
