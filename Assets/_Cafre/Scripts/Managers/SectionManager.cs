using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

/// <summary>
/// 
/// Section manager.
/// Class to manage the sections on scene according to the position of the player
/// in order to optimize the game.
/// 
/// </summary>

public class SectionManager : MonoBehaviour {

	//	Public Variables
	public static SectionManager instance;

	//	//	Serialized Fild Variables
	[SerializeField]
	private TextAsset sectionsDataText;
	[SerializeField]
	private string fileName = "sections.json";		//	Name of the JSON.
	[SerializeField]
	private bool build = false;						//	Flag to know if the user are going to build a JSON.


	//	Variables to read a JSON
	private SectionList data;						//	Section Data extracted from a JSON.
	private StreamReader sr;						//	Variable to read.
	private string contentToRead;					//	Content to read.

	//	Variables to manage sections
	private List<GameObject> sections;				//	Sections in the scene
	private int currentSection = 0;
	private int lastSection;


	//	Initializing
	void Awake(){
		
		//	Initializing the singleton.
		instance = this;
		//	Loading the sections data from a JSON.
		LoadSectionsData ();
	}

	void Start(){

		//	Initializing the list of sections in the scene
		sections = new List<GameObject>();

		//	Adding sections to the list using the Section Data
		if (!build) {

			for (int i = 0; i < data.section.Count; i++) {

				//	Crating a section game object with a box collider and a component to detect collisions
				GameObject section = new GameObject ( string.Concat( "Section", i), typeof(BoxCollider), typeof(SectionTrigger) );

				//	Extracting data from the Section Data
				section.transform.parent = transform;
				section.transform.position = data.section [i].center;
				section.GetComponent<BoxCollider> ().size = data.section [i].size;
				section.GetComponent<BoxCollider> ().isTrigger = true;

				//	Adding a new section to the list and turn it off
				sections.Add (section);
				sections [i].SetActive (false);

			}

			//	for the beginig initialize the scene with the first two sections.
			if( data.section.Count > 0 )
				ActiveSection (0);
			if (data.section.Count > 1)
				ActiveSection (1);
			lastSection = sections.Count - 1;
		}


	}



	//	Active a Section
	public void ActiveSection(int index){

		//	Turn On the section
		sections [index].SetActive (true);

		//	Obtaining props from the pool and modifying their transforms
		for (int i = 0; i < data.section [index].prop.Count; i++) {
			Prop propData = data.section [index].prop [i];
			GameObject tempProp = ObjectPool.instance.GetGameObjectOfType(propData.id, true,TypeOfID.PROP);
			if (tempProp) {
				tempProp.transform.parent = sections [index].transform;
				tempProp.transform.position = propData.position;
				tempProp.transform.rotation = propData.rotation;
				tempProp.transform.localScale = propData.scale;
			}
		}

		//	Obtaining items from the pool and modifying their transforms
		for (int i = 0; i < data.section [index].item.Count; i++) {
			Item itemData = data.section [index].item [i];
			GameObject tempItem = ObjectPool.instance.GetGameObjectOfType(itemData.id, true,TypeOfID.ITEM);
			if (tempItem) {
				tempItem.transform.parent = sections [index].transform;
				tempItem.transform.position = itemData.position;
				tempItem.transform.rotation = itemData.rotation;
				tempItem.transform.localScale = itemData.scale;
			}
		}
	}



	//	Desactive a Section
	public void DesactiveSection(int index){
		//	Turn Off the section
		sections [index].SetActive (false);

		while (sections [index].transform.childCount>0) {
			ObjectPool.instance.PoolGameObject (sections [index].transform.GetChild (0).gameObject);
		}

	}

	public void SetCurrentSection(int newSection){
			
		if (newSection >= sections.Count - 1) {
			ActiveSection (0);
		} 
		else {
			ActiveSection (newSection + 1);
		}

		DesactiveSection (lastSection);
		lastSection = currentSection;
		currentSection = newSection;


	}


	//	Return the name of the file
	public string GetFileName(){
		return fileName;
	}

	//	Load data from a JSON

	public void LoadSectionsData(){
		if (sectionsDataText != null) {
			contentToRead = "";
			contentToRead = sectionsDataText.ToString ();
			data = JsonUtility.FromJson<SectionList>(contentToRead);
		}else if (File.Exists(Application.persistentDataPath + "/" + fileName)){
			sr = new StreamReader(Application.persistentDataPath + "/" + fileName);
			contentToRead = "";
			contentToRead = sr.ReadToEnd();
			data = JsonUtility.FromJson<SectionList>(contentToRead);
			sr.Close();
		}
	}
}