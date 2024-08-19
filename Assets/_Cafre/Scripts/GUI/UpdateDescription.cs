using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 	Script that toggles the state of the UpdateDescription Window 
 * 	and update the information of the selected update.
*/

public class UpdateDescription: MonoBehaviour {

	//	Variables

	[SerializeField]
	private Text updateName;				//	Name field of properties.
	[SerializeField]
	private Text updateDescription;			//	Decription field of properties.

	private bool state = false;				//	Current state of the window.

	//	Enable Methods

	//	Toggle the Window.
	public void Toggle(Description update){

		//	If the selected update is selected again...
		if (updateName.text == update.GetName ()) {
			//	Close de window
			state = !state;
		} else {
			//	Else.. Update the information .
			state = true;
			updateName.text = update.GetName ();
			updateDescription.text = update.GetDescription ();
		}
		gameObject.SetActive (state);

	}

	//	Close the window
	public void Close(){
		gameObject.SetActive (false);
		state = false;
	}

}
