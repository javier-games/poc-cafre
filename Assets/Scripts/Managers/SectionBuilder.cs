using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Section builder.
/// Only for build a JSON with the information of sections.
/// 
/// </summary>

public class SectionBuilder : MonoBehaviour {

	//	Serializable structure to easy add a new section with props.
	[SerializeField]
	private string fileName = "SectionsData.json";			//	Name of the JSON.
	[SerializeField]
	private Transform sectionContainer;					//	Transform that contains the sections as children

	private StreamWriter	sw;							//	Variable to write a JSON.
	private SectionList		section;					//	Data to write in a JSON file.
	private string			contentToWrite;				// 	String of the data to write in a JSON.

	void Start () {

		//	Initializing a empty SectionList
		section = new SectionList ();

		//	For all the sections added from editor
		for (int i = 0; i < sectionContainer.childCount; i++) {

			//	Create a new Section
			Section temp = new Section ();

			//	Set Up of the section
			Transform currentSection = sectionContainer.GetChild (i);
			//	Change the center.
			temp.center = currentSection.position;

			//	Change the size.
			temp.size	= currentSection.GetComponent<BoxCollider>().size;

			//	Add Objects.
			for (int j = 0; j < currentSection.childCount; j++) {
				
				Transform currentChild = currentSection.GetChild (j);

				switch (currentChild.GetComponent<ID> ().GetTypeOfID ()) {

				case TypeOfID.ITEM:
					
					Item itemTemp = new Item ();
					itemTemp.id = currentChild.GetComponent<ID> ().GetID ();
					itemTemp.position = currentChild.position;
					itemTemp.rotation = currentChild.rotation;
					itemTemp.scale = currentChild.localScale;
					temp.item.Add (itemTemp);
					break;

				case TypeOfID.PROP:

					Prop propTemp = new Prop ();
					propTemp.id = currentChild.GetComponent<ID> ().GetID ();
					propTemp.position = currentChild.position;
					propTemp.rotation = currentChild.rotation;
					propTemp.scale = currentChild.lossyScale;
					temp.prop.Add (propTemp);
					break;

				}
			}

			//	Add the new section to the list.
			section.section.Add (temp);
		}


		//	Write data into a JSON.
		sw = new StreamWriter(Application.persistentDataPath+"/"+ fileName, false);
		contentToWrite = "";
		contentToWrite = JsonUtility.ToJson(section);
		sw.Write(contentToWrite);
		sw.Close();

		Debug.Log("Sections File Saved at: " + Application.persistentDataPath + "/" + fileName);

	}
}


