using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
