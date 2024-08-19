using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
/*
 * 	The purpose of this script is to help you to see the path builded by nodes and the nodes.
*/

public class Trajectory : MonoBehaviour {

	#if UNITY_EDITOR
	[SerializeField]
	private bool showPath = true;
	[SerializeField]
	private bool showNodes = true;
	private bool showing = true;

	//	Drawing the Path
	private void OnDrawGizmos(){
		if (showPath) {
			for(int i = 0; i < transform.childCount; i++){
				Node[] nodes = transform.GetChild(i).GetComponentsInChildren<Node> ();
				foreach (Node node in nodes) {
					if (node.IsEdge ()) {
						Node incomingNode = node.GetIncomingNode ();
						Handles.DrawBezier (
							node.transform.position,
							incomingNode.transform.position,
							node.transform.TransformPoint (node.GetTangent (1)),
							incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
							Color.gray,
							null,
							2f
						);
					}
				}
			}
		}

		//	Showing the nodes
		if (showNodes && !showing) {
			MeshRenderer[] meshes = transform.GetComponentsInChildren<MeshRenderer> ();
			for (int j = 0; j < meshes.Length; j++) {
				meshes [j].enabled = true;
			}
			showing = true;
		}else if(!showNodes && showing){			
			MeshRenderer[] meshes = transform.GetComponentsInChildren<MeshRenderer> ();
			for (int j = 0; j < meshes.Length; j++) {
				meshes [j].enabled = false;
			}
			showing = false;
		}
	}
	#endif
}
