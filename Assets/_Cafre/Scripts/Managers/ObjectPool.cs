using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// Object pool.
/// Class to manage game objects that constantly apear on the game.
/// 
/// </summary>

public class ObjectPool : MonoBehaviour {


	public static ObjectPool instance;

	[System.Serializable]
	public struct PrefabPool{
		public GameObject prefab;
		public int	amountInBuffer;
	};

	//	Serialize Field Variables
	[SerializeField]
	private PrefabPool[] propPrefabs;
	[SerializeField]
	private PrefabPool[] itemPrefabs;

	//	Variables for pool
	private List<GameObject>[] propPool;
	private List<GameObject>[] itemPool;
	protected GameObject propContainer;
	protected GameObject itemContainer;

	void Awake(){
		instance = this;

		GameObject containerOnject = new GameObject ("Object Pool");
		propContainer = new GameObject ("Prop Pool");
		propContainer.transform.parent = containerOnject.transform;
		itemContainer = new GameObject ("Item Pool");
		itemContainer.transform.parent = containerOnject.transform;

		propPool = new List<GameObject>[propPrefabs.Length];
		itemPool = new List<GameObject>[itemPrefabs.Length];

		//	For Props
		int index = 0;
		foreach(PrefabPool propPrefab in propPrefabs){
			propPool [index] = new List<GameObject> ();
			for (int i = 0; i < propPrefab.amountInBuffer; i++) {
				GameObject temp = Instantiate (propPrefab.prefab);
				temp.name = propPrefab.prefab.name;// + " ("+i+")";
				PoolGameObject (temp);
			}
			index++;
		}

		//	For Items
		index = 0;
		foreach(PrefabPool itemPrefab in itemPrefabs){
			itemPool [index] = new List<GameObject> ();
			for (int i = 0; i < itemPrefab.amountInBuffer; i++) {
				GameObject temp = Instantiate (itemPrefab.prefab);
				temp.name = itemPrefab.prefab.name;// + " ("+i+")";
				PoolGameObject (temp);
			}
			index++;
		}

	}

	public void PoolGameObject(GameObject obj){

		switch (obj.GetComponent<ID>().GetTypeOfID()){
		case TypeOfID.PROP:
			for(int i = 0; i<propPrefabs.Length; i++){
				if (propPrefabs [i].prefab.GetComponent<ID>().GetID() == obj.GetComponent<ID>().GetID()) {
					obj.SetActive (false);
					obj.transform.parent = propContainer.transform;
					obj.transform.position = propContainer.transform.position;
					propPool [i].Add (obj);
				}
			}
			break;
		case TypeOfID.ITEM:
			for(int i = 0; i<itemPrefabs.Length; i++){
				if (itemPrefabs [i].prefab.GetComponent<ID>().GetID() == obj.GetComponent<ID>().GetID()) {
					obj.SetActive (false);
					obj.transform.parent = itemContainer.transform;
					obj.transform.position = itemContainer.transform.position;
					itemPool [i].Add (obj);
				}
			}
			break;
		}


	}

	public GameObject GetGameObjectOfType(string objectType,bool onlyPooled,TypeOfID type){

		switch (type){
		case TypeOfID.PROP:
			for (int i = 0; i < propPrefabs.Length; i++) {
				GameObject propPrefab = propPrefabs[i].prefab;
				if (propPrefab.name == objectType) {
					if (propPool [i].Count > 0) {
						GameObject pooledObject = propPool [i] [0];
						pooledObject.transform.parent = null;
						propPool [i].RemoveAt (0);
						pooledObject.SetActive (true);
						return pooledObject;
					}
					else if(!onlyPooled){
						return Instantiate (propPrefabs[i].prefab);
					}
					break;
				}
			}
			break;
		case TypeOfID.ITEM:
			for (int i = 0; i < itemPrefabs.Length; i++) {
				GameObject itemPrefab = itemPrefabs[i].prefab;
				if (itemPrefab.name == objectType) {
					if (itemPool [i].Count > 0) {
						GameObject pooledObject = itemPool [i] [0];
						pooledObject.transform.parent = null;
						itemPool [i].RemoveAt (0);
						pooledObject.SetActive (true);
						return pooledObject;
					}
					else if(!onlyPooled){
						return Instantiate (itemPrefabs[i].prefab);
					}
					break;
				}
			}
			
			break;
		}
			
		return null;
	}
}

/*
 * public class ObjectPool : MonoBehaviour {

	public static ObjectPool instance;

	[System.Serializable]
	public struct PrefabPool{
		public GameObject prefab;
		public int	amountInBuffer;
	};

	public PrefabPool[] prefabs;
	public List<GameObject>[] generalPool;
	protected GameObject containerOnject;

	void Awake(){
		instance = this;
		containerOnject = new GameObject ("Object Pool");
		generalPool = new List<GameObject>[prefabs.Length];

		int index = 0;
		foreach(PrefabPool objectPrefab in prefabs){
			generalPool [index] = new List<GameObject> ();
			for (int i = 0; i < objectPrefab.amountInBuffer; i++) {
				GameObject temp = Instantiate (objectPrefab.prefab);
				temp.name = objectPrefab.prefab.name;// + " ("+i+")";
				PoolGameObject (temp);
			}
			index++;
		}
	}

	public void PoolGameObject(GameObject obj){
		for(int i = 0; i<prefabs.Length; i++){
			if (prefabs [i].prefab.name == obj.name) {
				obj.SetActive (false);
				obj.transform.parent = containerOnject.transform;
				obj.transform.position = containerOnject.transform.position;
				generalPool [i].Add (obj);
			}
		}
	}

	public GameObject GetGameObjectOfType(string objectType,bool onlyPooled){
		for (int i = 0; i < prefabs.Length; i++) {
			GameObject prefab = prefabs[i].prefab;
			if (prefab.name == objectType) {
				if (generalPool [i].Count > 0) {
					GameObject pooledObject = generalPool [i] [0];
					pooledObject.transform.parent = null;
					generalPool [i].RemoveAt (0);
					pooledObject.SetActive (true);
					return pooledObject;
				}
				else if(!onlyPooled){
					return Instantiate (prefabs[i].prefab);
				}
				break;
			}
		}
		return null;
	}
}*/