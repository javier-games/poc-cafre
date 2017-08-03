using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Section.
/// Script to define the structure of data to read and write on a file
/// with low cost.
/// 
/// </summary>

//	Class for Sections data
[System.Serializable]
public class Section{
	
	public Vector3 center;		//	Center of the Section
	public Vector3 size;		//	Size of the volume of the section
	public List<Item> item;		//	List of items in the section
	public List<Prop> prop;		//	List of props in the section


	//	Constructor
	public Section(){
		//	Initializing the lists
		item = new List<Item> ();
		prop = new List<Prop> ();
	}
}



//	Class for List of Sections data
[System.Serializable]
public class SectionList{

	public List<Section> section;	//	List of sections

	//	Constructor
	public SectionList(){
		//	Initializing the list
		section = new List<Section> ();
	}
}



//	Class for Items data
[System.Serializable]
public class Item{
	
	public string	id;				//	ID of the gameObject
	public Vector3	position;		//	Position of the gameObject
	public Quaternion	rotation;	//	Rotation of the gameObject
	public Vector3	scale;			//	Scale of the gameObject
}



//	Class for Props data
[System.Serializable]
public class Prop{
	
	public string 	id;				//	ID of the gameObject
	public Vector3	position;		//	Position of the gameObject
	public Quaternion	rotation;	//	Rotation of the gameObject
	public Vector3	scale;			//	Scale of the gameObject
}