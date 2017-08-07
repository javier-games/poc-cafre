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


}



/*
public class ObjectPool : MonoBehaviour {

	//	Public Variables
	public static ObjectPool instance;

	//	Custom structure to add prefabs.
	[System.Serializable]
	public struct PrefabPool{
		public GameObject prefab;
		public int	amountInBuffer;
	};

	//	Serialize Field Variables
	[SerializeField]
	private PrefabPool[] propPrefabs;		//	Array of types of prefabs for props.
	[SerializeField]
	private PrefabPool[] itemPrefabs;		// 	Array of types of prefabs for items.

	public List<GameObject>[] propPool;		//	Array of Lists of each kind of prefab.
	public List<GameObject>[] itemPool;		//	Array of Lists of each kind of prefab.

	protected Transform containerObject;	//	Game object that contains Props and Items.
	protected Transform containerProps;		//	Game object that contain Props.
	protected Transform containerItems;		//	Game object that contain Items.



	//	Initializing
	void Awake(){
		//	Initializing the singleton.
		instance = this;
	}

	void Start(){
		//	Crating a the containers.
		containerObject	= new GameObject ("Object Pool").transform;
		containerProps	= new GameObject ("Props Pool").transform;
		containerItems	= new GameObject ("Items Pool").transform;

		//	Assigning the contentObject as a parent.
		containerProps.parent = containerObject;
		containerItems.parent = containerObject;

		//	Initializing the array of list of objects.
		propPool = new List<GameObject>[propPrefabs.Length];
		itemPool = new List<GameObject>[itemPrefabs.Length];

		//	Creating props.
		int index = 0;
		foreach(PrefabPool propPrefab in propPrefabs){

			//	Initializing the list of objects.
			propPool [index] = new List<GameObject> ();
			//	Creating the objects according to the propPrefabs.
			for (int i = 0; i < propPrefab.amountInBuffer; i++) {
				GameObject temp = Instantiate (propPrefab.prefab);
				temp.name = propPrefab.prefab.name;
				//	Pooling the prop.
				PoolGameObject(temp,TypeOfID.PROP);
			}
			index++;
		}

		//	Creating items
		index = 0;
		foreach(PrefabPool itemPrefab in itemPrefabs){

			//	Initializing the list of objects.
			itemPool [index] = new List<GameObject> ();
			//	Creating the objects according to the itemPrefabs.
			for (int i = 0; i < itemPrefab.amountInBuffer; i++) {
				GameObject temp = Instantiate (itemPrefab.prefab);
				temp.name = itemPrefab.prefab.name;
				//	Pooling the item.
				PoolGameObject(temp,TypeOfID.ITEM);
			}
			index++;
		}
			
	}

		

	//	Pool an object.
	public void PoolGameObject(GameObject obj,TypeOfID type){


		switch (type) {

		case TypeOfID.ITEM:

			//	Search for the ID of the object
			for(int i = 0; i<itemPrefabs.Length; i++){
				if (itemPrefabs [i].prefab.GetComponent<ID>().GetID() == obj.GetComponent<ID>().GetID()) {

					//	Turn Off the object
					obj.SetActive (false);
					//	Set the parent
					obj.transform.parent = containerItems;
					//	Set the position
					obj.transform.position = containerObject.transform.position;
					//	Add to the pool
					itemPool [i].Add (obj);
				}
			}

			break;

		case TypeOfID.PROP:

			//	Search for the ID of the object
			for(int i = 0; i<propPrefabs.Length; i++){
				if (propPrefabs [i].prefab.GetComponent<ID>().GetID() == obj.GetComponent<ID>().GetID()) {

					//	Turn Off the object
					obj.SetActive (false);
					//	Set the parent
					obj.transform.parent = containerProps;
					//	Set the position
					obj.transform.position = containerObject.transform.position;
					//	Add to the pool
					propPool [i].Add (obj);
				}
			}

			break;

		}
	}

	//	Return an object
	public GameObject GetGameObjectOfType(string id,bool urgent,TypeOfID type){

		switch (type) {

		case TypeOfID.ITEM:

			for (int i = 0; i < itemPrefabs.Length; i++) {
				GameObject prefab = itemPrefabs[i].prefab;
				if (prefab.GetComponent<ID> ().GetID () == id) {
					Debug.Log (itemPool[i].Count);
				}
			}

			break;

		case TypeOfID.PROP:

			for (int i = 0; i < propPrefabs.Length; i++) {
				GameObject prefab = propPrefabs[i].prefab;
				if (prefab.GetComponent<ID> ().GetID () == id) {
					Debug.Log (propPool[0][0].ToString());
				}
			}

			break;

		}
			
		return null;
	}

}*/
