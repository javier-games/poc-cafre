#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeInspector : Editor{

	//	Variables
	private Node node;
	private Transform nodeTransform;
	private Quaternion nodeRotation;

	private int tangentSelected = -1;
	private static Color[] modeColors = {
		Color.magenta,
		Color.green,
		Color.cyan
	};

	private float size = 0;

	private MessageType type = MessageType.Info;
	private string message = "";



	/*	ON SCENE GUI	*/

	private void OnSceneGUI () {

		node = target as Node;
		nodeTransform = node.transform;
		nodeRotation = Tools.pivotRotation == PivotRotation.Local ? nodeTransform.rotation : Quaternion.identity;

		Vector3 tangent1 = DrawTangent(0);
		Vector3 tangent2 = DrawTangent(1);

		Handles.color = modeColors [(int)node.GetMode ()]*0.8f;
		Handles.DrawLine(nodeTransform.position, tangent1);
		Handles.DrawLine(nodeTransform.position, tangent2);
		if (node.IsEdge ()) {
			//	Avoid the MissingReferenceException if the incoming node has been destroyed.
			if (!node.GetIncomingNode()) {
				node.SetLeftNode (null);
				node.SetRightNode (null);
				node.IsEdge ();
			}
			if (nodeTransform.hasChanged) {
				node.SetLenght (LenghtUpdate ());
				nodeTransform.hasChanged = false;
			}
			Node incomingNode = node.GetIncomingNode();
			Handles.DrawBezier (
				node.transform.position,
				incomingNode.transform.position,
				node.transform.TransformPoint (node.GetTangent(1)),
				incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
				Color.white,
				null,
				4f
			);
		}
	}

	private Vector3 DrawTangent (int indexTangent) {

		Vector3 tangent = nodeTransform.TransformPoint (node.GetTangent (indexTangent));
		size = HandleUtility.GetHandleSize (tangent);
		Handles.color = modeColors [(int)node.GetMode ()];

		if (Handles.Button (tangent, nodeRotation, size * 0.06f, size * 0.08f, Handles.DotHandleCap)) {
			tangentSelected = indexTangent;
			Repaint ();
		}
		if (tangentSelected == indexTangent) {
			EditorGUI.BeginChangeCheck ();
			tangent = Handles.DoPositionHandle (tangent, nodeRotation);
			if (EditorGUI.EndChangeCheck ()) {
				Undo.RecordObject (node, "Move Tangent");
				node.SetTangent (indexTangent, nodeTransform.InverseTransformPoint (tangent));
				EnforceMode ();
				node.SetLenght (LenghtUpdate());
			}
		}
		return tangent;
	}

	private float LenghtUpdate(){
		float lenght = 0;
		if (node.IsEdge()) {

			int steps = 10;
			Vector3[] stepPoints = new Vector3[steps];

			for (int i = 0; i < steps; i++) {
				Node incomingNode = node.GetIncomingNode();
				stepPoints [i] = node.BezierPoint (
					node.transform.position,
					incomingNode.transform.position,
					node.transform.TransformPoint (node.GetTangent(1)),
					incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
					((i*1f)/(steps*1f))
				);
				if (i > 0) {
					lenght += Vector3.Distance (stepPoints[i-1],stepPoints[i]);
				}
			}
		}
		return lenght;
	}

	private void EnforceMode () {

		if (node.GetMode() == BezierMode.Free)
			return;

		int fixedIndex;
		int enforcedIndex;

		if ( tangentSelected < 1 ) {
			fixedIndex = 0;
			enforcedIndex = 1;
		}
		else {
			fixedIndex = 1;
			enforcedIndex = 0;
		}

		Vector3 middle = Vector3.zero;
		Vector3 enforcedTangent = middle - node.GetTangent(fixedIndex);
		if (node.GetMode() == BezierMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, node.GetTangent(enforcedIndex));
		}
		node.SetTangent(enforcedIndex, (middle + enforcedTangent));
	}



	/*	ON INSPECTOR	*/

	public override void OnInspectorGUI () {
		if (tangentSelected >= 0 && tangentSelected < 2) {
			TangentsInspector ();
		} else {
			NodesInspector();
		}
		if(message!="")
			EditorGUILayout.HelpBox(message, type, true);
	}

	private void NodesInspector(){

		node = target as Node;

		GUILayout.BeginVertical (GUILayout.MaxHeight (30f));
		GUILayout.FlexibleSpace ();
		if (node.IsEdge ()) {
			EditorGUILayout.FloatField ("Lenght of the edge: ", node.GetLenght ());
		}  else {
			EditorGUILayout.LabelField ("Single Node");
		}
		GUILayout.EndVertical ();

		//	Showing the Nodes
		GUILayout.BeginVertical (GUILayout.MaxHeight (80f));
		GUILayout.FlexibleSpace ();
		EditorGUILayout.LabelField ("Nodes");

		EditorGUI.BeginChangeCheck();
		Node incomingNode	= EditorGUILayout.ObjectField ("  Incoming Node",	node.GetIncomingNode(),	typeof(Node), true) as Node;
		if (EditorGUI.EndChangeCheck ()) {
			if (incomingNode != node) {
				Undo.RecordObject (node, "Incoming Node Edit");
				node.SetIncomingNode (incomingNode);
				if (incomingNode == null) {
					node.SetLeftNode (null);
					node.SetRightNode (null);
					node.IsEdge ();
				}
				node.SetLenght (LenghtUpdate());
				message = "";
			}  else {
				message = "You can not set the incoming node as the current node";
				type = MessageType.Error;
			}
		}

		EditorGUI.BeginChangeCheck ();
		Node leftNode = EditorGUILayout.ObjectField ("  Left Node", node.GetLeftNode (), typeof(Node), true) as Node;
		if (EditorGUI.EndChangeCheck ()) {
			if (node.IsEdge ()) {
				if (leftNode != node.GetIncomingNode () ) {
					if (leftNode) {
						if (leftNode.IsEdge ()) {
							Undo.RecordObject (node, "Left Node Edit");
							node.SetLeftNode (leftNode);
							message = "";
						}
						else {
							message = "Has to be an edge";
							type = MessageType.Error;
						}
					} else {
						Undo.RecordObject (node, "Left Node Edit");
						node.SetLeftNode (leftNode);
						message = "";
					}
				} else {
					message = "You can not set the left node as the incoming node and has to be an edge";
					type = MessageType.Error;
				}
			}  else {
				message = "To add a Left Node you first need to add an incoming node";
				type = MessageType.Info;
			}
		}

		EditorGUI.BeginChangeCheck ();
		Node rightNode = EditorGUILayout.ObjectField ("  Right Node",	node.GetRightNode (),	typeof(Node), true) as Node;
		if (EditorGUI.EndChangeCheck ()) {
			if (node.IsEdge ()) {
				if (rightNode != node.GetIncomingNode ()) {
					if (rightNode) {
						if (rightNode.IsEdge ()) {
							Undo.RecordObject (node, "Right Node Edit");
							node.SetRightNode (rightNode);
							message = "";
						}
						else{
							message = "Has to be an edge";
							type = MessageType.Error;
						}
					} else {
						Undo.RecordObject (node, "Right Node Edit");
						node.SetRightNode (rightNode);
						message = "";
					}
				} else {
					message = "You can not set the right node as the incoming node and has to be an edge";
					type = MessageType.Error;
				}
			}  else {
				message = "To add a Right Node you first need to add an incoming node";
				type = MessageType.Info;

			}
		}

		GUILayout.EndVertical ();

		GUILayout.BeginVertical (GUILayout.MaxHeight (80f));
		GUILayout.FlexibleSpace ();
		EditorGUILayout.LabelField ("Tangents");

		EditorGUI.BeginChangeCheck();
		Vector3 backTangent = EditorGUILayout.Vector3Field("  Back Tangent", node.GetTangent(0));

		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Move Tangent");
			node.SetTangent(0, backTangent);
			EnforceMode ();
			node.SetLenght (LenghtUpdate());

		}
		EditorGUI.BeginChangeCheck();
		Vector3 frontTangent = EditorGUILayout.Vector3Field("  Front Tangent", node.GetTangent(1));

		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Move Tangent");;
			node.SetTangent(1, frontTangent);
			EnforceMode ();
			node.SetLenght (LenghtUpdate());

		}
		EditorGUI.BeginChangeCheck();
		BezierMode mode = (BezierMode)EditorGUILayout.EnumPopup("  Mode", node.GetMode());
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Change Point Mode");
			node.SetMode(mode);
			EnforceMode ();
			node.SetLenght (LenghtUpdate());
		}

		GUILayout.EndVertical ();


	}

	private void TangentsInspector() {

		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", node.GetTangent(tangentSelected));

		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Move Point");
			EditorUtility.SetDirty(node);
			node.SetTangent(tangentSelected, point);
			EnforceMode ();
			node.SetLenght (LenghtUpdate());

		}
		EditorGUI.BeginChangeCheck();
		BezierMode mode = (BezierMode)EditorGUILayout.EnumPopup("Mode", node.GetMode());
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Change Point Mode");
			node.SetMode(mode);
			EnforceMode ();
			EditorUtility.SetDirty(node);
			node.SetLenght (LenghtUpdate());
		}
	}

}

#endif