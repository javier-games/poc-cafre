using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * 	Manager of the Graphic User Interface.
*/

public class MenuManager : MonoBehaviour {

	//	Panels
	[SerializeField]
	private GameObject mapPanel;
	[SerializeField]
	private GameObject socialPanel;
	[SerializeField]
	private GameObject itemsPanel;
	[SerializeField]
	private GameObject cupsPanel;

	//	Initialization
	void Start () {
		ClearUI ();
		mapPanel.SetActive (true);
	}


	//	Enable Methods

	void ClearUI(){
		mapPanel.SetActive (false);
		socialPanel.SetActive (false);
		itemsPanel.SetActive (false);
		cupsPanel.SetActive (false);
	}

	public void Items(){
		ClearUI ();
		itemsPanel.SetActive (true);
	}

	public void Map(){
		ClearUI ();
		mapPanel.SetActive (true);
	}

	public void Social(){
		ClearUI ();
		socialPanel.SetActive (true);
	}

	public void Cups(){
		ClearUI ();
		cupsPanel.SetActive (true);
	}

	//	Scene Management Methods

	public void Continue(){
		SceneManager.LoadScene ("LVL0");
	}

	public void Exit(){
		ClearUI ();
		//Aplication.Quit();
	}
}