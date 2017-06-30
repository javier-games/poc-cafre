using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDescription: MonoBehaviour {


	[SerializeField]
	private Text updateName;			//	Name field of properties
	[SerializeField]
	private Text updateDescription;			//	Decription field of properties

	private bool stade = false;

	void Start(){

	}

	public void Toggle(Description update){
		if (updateName.text == update.GetName ()) {
			stade = !stade;
		} else {
			stade = true;
			updateName.text = update.GetName ();
			updateDescription.text = update.GetDescription ();
		}
		gameObject.SetActive (stade);

	}
	public void Close(){
		gameObject.SetActive (false);
		stade = false;
	}

}
