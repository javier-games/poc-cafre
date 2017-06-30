using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

	public GameObject loadBar;

	private Transform loadBarTr;
	private float screenWidth;
	private float progress = 0;
	private bool loadScene = false;
	private bool start = false;


	void Start () {
		loadBarTr = loadBar.transform;
		screenWidth=Screen.width;
		loadBarTr.position = new Vector3 ((screenWidth*(progress-1)),0,0);
	}
		
	void Update () {
		if (start == false) {
			start = true;
			ChangeScene ("Menu");
		}

		if (loadScene == true) {
			//Space for some animation here
		}
	}

	public void ChangeScene(string scene){
		StartCoroutine (LoadNewScene(scene));
		loadScene = true;
	}

	IEnumerator LoadNewScene(string scene){
		yield return new WaitForSeconds(3f);
		AsyncOperation async = SceneManager.LoadSceneAsync(scene);
		while (!async.isDone) {
			
			progress = async.progress;
			loadBarTr.position = new Vector3 ((screenWidth*(progress-1)),0,0);

			yield return null;
		}
	}
}