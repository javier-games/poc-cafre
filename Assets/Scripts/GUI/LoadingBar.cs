using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 	This script make a loading effect in a bar.
 * 	It its posible to use Fill amount in an Image to make the same effect.
*/

public class LoadingBar : MonoBehaviour {

	//	Variables
	[SerializeField]
	private float	timeToWait = 2;		//	Time to wait for the coroutine.
	private float	progress = 0;		//	Progress of the loading.
	private float	screenWidth;		//	Width of the screen.

	//	Initialization
	void Start () {
		screenWidth=Screen.width;
		transform.position = new Vector3 ((screenWidth*(progress-1)),0,0);
		StartCoroutine (LoadNewScene("Menu"));
	}

	//	Coroutine
	IEnumerator LoadNewScene(string scene){
		yield return new WaitForSeconds(timeToWait);
		AsyncOperation async = SceneManager.LoadSceneAsync(scene);
		//	Updating the position of the bar.
		while (!async.isDone) {
			progress = async.progress;
			transform.position = new Vector3 ((screenWidth*(progress-1)),0,0);
			yield return null;
		}
	}
}