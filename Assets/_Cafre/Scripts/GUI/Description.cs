using UnityEngine;

/*
 * 	A little script that stores the description of an item or update.
 * 	Could be change for a List.
*/

public class Description : MonoBehaviour {
	
	[SerializeField]
	private string nameString;
	[SerializeField]
	private string descriptionString;

	public string GetName(){
		return nameString;
	}
	public string GetDescription(){
		return descriptionString;
	}
}
