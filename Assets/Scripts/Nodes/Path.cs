using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
/*
 * 	The purpose of this script is to help you to see the path builded by nodes and the nodes.
*/

public class Path : MonoBehaviour {

	#if UNITY_EDITOR
	[SerializeField]
	private bool showPath = true;
	[SerializeField]
	private bool showNodes = true;
	private bool showing = true;

	//	Drawing the Path
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

		//	Showing the nodes
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
	#endif
}
