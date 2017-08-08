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
			
			Node currentNode = mainVehicle.GetCurrentNode ();
			Node incomingNode = currentNode.GetIncomingNode ();

			currentNodeTransition = mainVehicle.GetNodeTransition();
			distance = 0;

			if (currentNode.IsEdge ()) {
				for (int i = 0; i < accuaracy; i++) {
					float vehicleTransition = currentNodeTransition + i * (1f - currentNodeTransition) / (accuaracy * 1f);

					currentComputedPosition = currentNode.GetPosition (currentNode,incomingNode,vehicleTransition);
					//	Calculating the distance between steps.
					if (i > 0) {
						distance += Vector3.Distance (lastComputedPosition, currentComputedPosition);
						if (distance > inFrontOffset) {
							GameObject car = ObjectPool.instance.GetGameObjectOfType (iDs[Random.Range(0,iDs.Count)],true,TypeOfID.PROP);
							car.transform.position = currentComputedPosition;
							Debug.Break();
							break;
						}
					} 
					lastComputedPosition = currentComputedPosition;
				}
			}


			break;

		case VehiclePreposition.BEHIND:
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

}
