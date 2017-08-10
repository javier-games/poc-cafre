using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

	[SerializeField]
	private Transform readySteadyGo;


	void Start () {
		for (int j = 0; j < 3; j++) {
			readySteadyGo.GetChild (j).gameObject.SetActive (false);
		}
		StartCoroutine (ReadySteadyGo(0));
	}

	IEnumerator ReadySteadyGo(int i){
		yield return new WaitForSeconds(1f);
		if (i < 3) {
			if(i > 0)
				readySteadyGo.GetChild (i-1).gameObject.SetActive (false);
			readySteadyGo.GetChild (i).gameObject.SetActive (true);
			i++;
			StartCoroutine (ReadySteadyGo (i));
		} else{
			readySteadyGo.GetChild (i-1).gameObject.SetActive (false);
			GameManager.instance.ChangeToNewState (GameState.PLAYING);
		}
	}

}
