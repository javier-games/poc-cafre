using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour {

	public GameObject noticeLoad;

	private bool loadScene = false;
	[SerializeField]
	private Text loadingText;

	// Use this for initialization
	void Start () {
		noticeLoad.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		// si la nueva escena ha iniciado a cargar
		if (loadScene == true) {
			// se pone el texto a parpadear, para dar a entender que se esta cargando
			loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
		}
	}
	
	public void ChangeScene(string scene){
		StartCoroutine (LoadNewScene(scene));
	}

	IEnumerator LoadNewScene(string scene){
		// Inicia una operacion asincronica para cargar la escena que se le paso a la corrutina, dicha operacion se guarda en asyn
		AsyncOperation async = SceneManager.LoadSceneAsync(scene);

		// Mientras que la operación asincrónica para cargar la nueva escena no se haya completado todavía, hay que loopear hasta que se completa.
		while (!async.isDone) {
			yield return null;
		}
	}
}
