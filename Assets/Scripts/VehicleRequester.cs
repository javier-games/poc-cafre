using UnityEngine;
using System.Collections.Generic;

public class VehicleRequester : MonoBehaviour {

	[SerializeField]
	private List<string> iDs;

	void OnTriggerEnter(){
		VehiclesManager.instance.PutACar (VehiclePreposition.INFRONT,iDs);
	}
}
