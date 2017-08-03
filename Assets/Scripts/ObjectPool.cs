using System.Collections.Generic;
using UnityEngine;

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
				temp.name = objectPrefab.prefab.name;
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
