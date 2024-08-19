using UnityEngine;

/// <summary>
/// ID.
/// Class to add to the object a string ID.
/// </summary>ç


public enum TypeOfID{ PROP,ITEM}

public class ID : MonoBehaviour {

	//	Serialized Fild Variables
	[SerializeField]
	private string iD;		//	ID of the object
	[SerializeField]
	private TypeOfID type;

	public string GetID(){
		return iD;
	}
	public TypeOfID GetTypeOfID(){
		return type;
	}

}