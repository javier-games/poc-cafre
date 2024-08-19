using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossroad : MonoBehaviour {

	[SerializeField]
	private int amount = 3;
	[SerializeField]
	private Transform[] origin;
	[SerializeField]
	private Transform[] destiny;
	[SerializeField]
	private string[] iDs;
	[SerializeField]
	private float minSpeed = 0.001f;
	[SerializeField]
	private float maxSpeed = 0.02f;

	private List<float> transitions;
	private List<float> speeds;
	private List<GameObject> vehicles;

	void Start(){

		transitions = new List<float> ();
		speeds = new List<float>();
		vehicles = new List<GameObject>();

		for (int i = 0; i < amount; i++) {
			vehicles.Add (ObjectPool.instance.GetGameObjectOfType(iDs[Random.Range(0,iDs.Length)],false,TypeOfID.PROP));
			speeds.Add (Random.Range(minSpeed,maxSpeed));
			transitions.Add( Random.Range (0f, 1f));
			vehicles [i].SetActive (false);
			vehicles [i].transform.parent = transform;
			vehicles [i].transform.position = Vector3.Lerp (origin[i].position,destiny[i].position,transitions[i]);
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			for (int i = 0; i < vehicles.Count; i++) {
				vehicles [i].SetActive (true);
			}
		}
	}

	void OnTriggerStay(Collider other){
		if (other.CompareTag ("Player")) {

			for(int i = 0; i < vehicles.Count; i++){
				transitions [i] += speeds[i];
				vehicles [i].transform.position = Vector3.Lerp (origin[i].position,destiny[i].position,transitions[i]);
				vehicles [i].transform.rotation = Quaternion.LookRotation (destiny[i].forward);
				if(transitions[i] >= 1){
					transitions [i] = 0;
					GameObject toDelete = vehicles [i];
					vehicles [i] = ObjectPool.instance.GetGameObjectOfType (iDs [Random.Range (0, iDs.Length)], false, TypeOfID.PROP);
					ObjectPool.instance.PoolGameObject(toDelete);
					speeds.Add (Random.Range(minSpeed,maxSpeed));
				}
			}


		}
	}

	void OnTriggerExit(Collider other){
		for (int i = 0; i < vehicles.Count; i++) {
			ObjectPool.instance.PoolGameObject (vehicles [i]);
		}
	}



}
