using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public static HUDManager instance;

	[SerializeField]
	private Transform readySteadyGo;
	[SerializeField]
	private Text moneyText;
	[SerializeField]
	private Image crashesLevel;

	private AudioSource source;

	void Awake(){
		instance = this;
	}

	void Start () {
		UpdateMoneyText ();
		source = GetComponent<AudioSource> ();
		for (int j = 0; j < 3; j++) {
			readySteadyGo.GetChild (j).gameObject.SetActive (false);
		}
		StartCoroutine (ReadySteadyGo(0));
	}

	public void UpdateMoneyText (){
		moneyText.text = string.Concat("$", DataManager.instance.temp.money.ToString());
	}
	public void UpdateCrashesAmount(float amount){
		crashesLevel.fillAmount = amount;	
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
			i++;
		}
		AudioManager.instance.ReadyCount (i,source);
	}

}
