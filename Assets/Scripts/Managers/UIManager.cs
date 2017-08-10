using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public static UIManager instance;



	[SerializeField]
	private GameObject hudPanel;
	[SerializeField]
	private GameObject pausePanel;
	[SerializeField]
	private GameObject gameOverPanel;
	[SerializeField]
	private GameObject startPanel;
	[SerializeField]
	private GameObject victoryPanel;
	[SerializeField]
	private GameObject storePanel;
	[SerializeField]
	private GameObject itemsPanel;



	void Awake(){
		instance = this;
	}
	void Start () {

		GameManager.instance.ChangeStateEvent += ShowPanel;

		ClearUI ();
		startPanel.SetActive (true);
	}



	//	Enable Methods
	void ClearUI(){
		hudPanel.SetActive (false);
		pausePanel.SetActive (false);
		gameOverPanel.SetActive (false);
		startPanel.SetActive (false);
		victoryPanel.SetActive (false);
		storePanel.SetActive (false);
		itemsPanel.SetActive (false);
	}
	public  void StartGame(){
		GameManager.instance.ChangeToNewState (GameState.PREPARE);
	}
	public void Pause(){
		GameManager.instance.ChangeToNewState (GameState.PAUSE);
	}
	public void Continue(){
		GameManager.instance.ChangeToNewState (GameState.PLAYING);
	}
	public void GameOver(){
		GameManager.instance.ChangeToNewState (GameState.GAME_OVER);
	}
	public void Exit(){
		GameManager.instance.ChangeToNewState (GameState.EXIT);
	}
	public void ReStart(){
		GameManager.instance.ChangeToNewState (GameState.START);
	}
	public void Store(){
		GameManager.instance.ChangeToNewState (GameState.STORE);
	}
	public void Items(){
		GameManager.instance.ChangeToNewState (GameState.ITEMS);
	}



	public void ShowPanel(){
		switch (GameManager.instance.currentState) {

		case GameState.START:
			break;

		case GameState.CUTSCENE:
			ClearUI ();
			break;

		case GameState.PLAYING:
			ClearUI ();
			hudPanel.SetActive (true);
			Time.timeScale = 1;
			break;

		case GameState.PREPARE:
			ClearUI ();
			hudPanel.SetActive (true);
			Time.timeScale = 1;
			break;

		case GameState.PAUSE:
			ClearUI ();
			pausePanel.SetActive (true);
			Time.timeScale = 0;
			break;

		case GameState.CONTINUE:
			break;

		case GameState.GAME_OVER:
			ClearUI ();
			gameOverPanel.SetActive (true);
			StartCoroutine ("LoadScene", 4f);
			break;

		case GameState.EXIT:
			Time.timeScale = 1;
			//This should be an other scene.
			StartCoroutine ("LoadScene", 0f);
			break;

		case GameState.RESTART:
			StartCoroutine ("LoadScene", 0f);
			break;

		case GameState.WIN:
			ClearUI ();
			victoryPanel.SetActive (true);
			StartCoroutine ("LoadScene", 4f);
			break;
		}
	}

	public IEnumerator LoadScene(float timeToWait){
		yield return new WaitForSeconds (timeToWait);
		ClearUI ();
		ReStart ();
		SceneManager.LoadScene ("Manu",LoadSceneMode.Single);
	}
}