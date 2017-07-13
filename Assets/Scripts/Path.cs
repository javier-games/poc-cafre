﻿using UnityEngine;
using UnityEditor;

public class Path : MonoBehaviour {

	[SerializeField]
	private bool showPath = true;
	[SerializeField]
	private bool showNodes = true;
	private bool showing = true;

	private void OnDrawGizmos(){
		if (showPath) {
			Node[] nodes = transform.GetComponentsInChildren<Node> ();
			for (int i = 0; i < transform.childCount; i++) {
				if (nodes [i].IsEdge ()) {
					Node incomingNode = nodes [i].GetIncomingNode ();
					Handles.DrawBezier (
						nodes [i].transform.position,
						incomingNode.transform.position,
						nodes [i].transform.TransformPoint (nodes [i].GetTangent (1)),
						incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
						Color.gray,
						null,
						2f
					);
				}
				
			}
		}
		if (showNodes && !showing) {
			MeshRenderer[] meshes = transform.GetComponentsInChildren<MeshRenderer> ();
			for (int i = 0; i < transform.childCount; i++) {
				meshes [i].enabled = true;
			}
			showing = true;
		}else if(!showNodes && showing){
			MeshRenderer[] meshes = transform.GetComponentsInChildren<MeshRenderer> ();
			for (int i = 0; i < transform.childCount; i++) {
				meshes [i].enabled = false;
			}
			showing = false;
		}
	}	
}
