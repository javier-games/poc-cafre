#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

/*
 * Helper to make easy to build a path via inspector and OnScenGUI
*/

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeInspector : Editor{



	//	Variables
	private Node node;								//	Selected Node
	private Transform nodeTransform;				//	Transform of the selected node
	private Quaternion nodeRotation;				//	Rotation of the selected node
		
	private int tangentSelected = -1;				//	Tangent Point Selected
	private static Color[] modeColors = {			//	Code of colors for the BezierMode
		Color.magenta,
		Color.green,
		Color.cyan
	};

	private float size = 0;							//	Size of the handle button

	private MessageType type = MessageType.Info;	//	Type of the pop up message in inspector
	private string message = "";					//	Message of that pop up



	/* --------------------------------------------------- */

	//	OnSceneGUI
	private void OnSceneGUI () {

		//	Detecting the current selected node
		node = target as Node;
		nodeTransform = node.transform;
		nodeRotation = Tools.pivotRotation == PivotRotation.Local ? nodeTransform.rotation : Quaternion.identity;

		//	Drawing the tangents buttons
		Vector3 tangent1 = DrawTangent(0);
		Vector3 tangent2 = DrawTangent(1);

		//	Drawing a visual line bewteen tangent points and the position of the node
		Handles.color = modeColors [(int)node.GetMode ()]*0.8f;
		Handles.DrawLine(nodeTransform.position, tangent1);
		Handles.DrawLine(nodeTransform.position, tangent2);

		//	If the node is an edge...
		if (node.IsEdge ()) {
			
			//	Avoid the MissingReferenceException if the incoming node has been destroyed.
			if (!node.GetIncomingNode()) {
				node.SetLeftNode (null);
				node.SetRightNode (null);
				node.IsEdge ();
			}

			//	Update the lenght of the Bezier Curve
			if (nodeTransform.hasChanged) {
				node.SetLenght (LenghtUpdate ());
				nodeTransform.hasChanged = false;
			}

			//	Draw the entire Bezier Curve of the edge
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

	//	Drawing Tangents
	private Vector3 DrawTangent (int indexTangent) {

		//	Initialize variable to use
		Vector3 tangent = nodeTransform.TransformPoint (node.GetTangent (indexTangent));
		size = HandleUtility.GetHandleSize (tangent);
		Handles.color = modeColors [(int)node.GetMode ()];

		//	Drawing and detecting if a tangent has been selected
		if (Handles.Button (tangent, nodeRotation, size * 0.06f, size * 0.08f, Handles.DotHandleCap)) {
			tangentSelected = indexTangent;
			Repaint ();
		}

		//	Updating the node variables if a tangent has been modifyed
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

	//	Calculate an aproximation for the Bezie Curve
	private float LenghtUpdate(){
		float lenght = 0;
		if (node.IsEdge()) {

			int steps = 10;	//	Number of steps to segment the beziercurve
			Vector3[] stepPoints = new Vector3[steps];

			//	Getting the step points
			for (int i = 0; i < steps; i++) {
				Node incomingNode = node.GetIncomingNode();
				stepPoints [i] = node.BezierPoint (
					node.transform.position,
					incomingNode.transform.position,
					node.transform.TransformPoint (node.GetTangent(1)),
					incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
					((i*1f)/(steps*1f))
				);
				//	Calculating the distance between steps
				if (i > 0) {
					lenght += Vector3.Distance (stepPoints[i-1],stepPoints[i]);
				}
			}
		}
		return lenght;
	}

	//	Method to modify the Bezier Tangent Points
	private void EnforceMode () {

		//	If the mode is free the point do not have to be forced
		if (node.GetMode() == BezierMode.Free)
			return;

		int fixedIndex;			//	Point selected
		int enforcedIndex;		//	Poit to enforce 

		//	If the user select a tangent and change its mode then
		//	the oposite tangent point has to be enforced

		if ( tangentSelected < 1 ) {
			fixedIndex = 0;
			enforcedIndex = 1;
		}
		else {
			fixedIndex = 1;
			enforcedIndex = 0;
		}

		//	The node is the center so the local position is zero
		Vector3 middle = Vector3.zero;
		//	Enforcing the tangents
		Vector3 enforcedTangent = middle - node.GetTangent(fixedIndex);
		if (node.GetMode() == BezierMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, node.GetTangent(enforcedIndex));
		}
		node.SetTangent(enforcedIndex, (middle + enforcedTangent));
	}




	/* --------------------------------------------------- */

	// On Inspector
	public override void OnInspectorGUI () {
		
		//	Is a tangent is selected use an tangent inspector
		if (tangentSelected >= 0 && tangentSelected < 2) {
			TangentsInspector ();
		}

		//	Else use a complete node inspector
		else {
			NodesInspector();
		}

		//	Show if there are a message	
		if(message!="")
			EditorGUILayout.HelpBox(message, type, true);
	}

	//	Inspector for nodes
	private void NodesInspector(){

		//	Detecting the current selected node
		node = target as Node;

		//	Show on inspector if the node is an egde with its lenght
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


		//	Incoming Node
		EditorGUI.BeginChangeCheck();
		Node incomingNode	= EditorGUILayout.ObjectField ("  Incoming Node",	node.GetIncomingNode(),	typeof(Node), true) as Node;
		//	If the user change the incoming node
		if (EditorGUI.EndChangeCheck ()) {
			//	Avoid to use the selected node as incoming node becouse it is not a edge
			if (incomingNode != node) {
				Undo.RecordObject (node, "Incoming Node Edit");
				node.SetIncomingNode (incomingNode);
				//	A node can has a left and right node only if it is an edge
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


		//	Left Node
		EditorGUI.BeginChangeCheck ();
		Node leftNode = EditorGUILayout.ObjectField ("  Left Node", node.GetLeftNode (), typeof(Node), true) as Node;
		if (EditorGUI.EndChangeCheck ()) {
			//	A node can has a left and right node only if it is an edge
			if (node.IsEdge ()) {
				//	Avoid to use the incoming node as left node
				if (leftNode != node.GetIncomingNode () ) {
					//	Change the node if it exist
					if (leftNode) {
						//	The Right Node has to be an edge
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


		//	Right Node
		EditorGUI.BeginChangeCheck ();
		Node rightNode = EditorGUILayout.ObjectField ("  Right Node",	node.GetRightNode (),	typeof(Node), true) as Node;
		if (EditorGUI.EndChangeCheck ()) {
			//	A node can has a left and right node only if it is an edge
			if (node.IsEdge ()) {
				//	Avoid to use the incoming node as right node
				if (rightNode != node.GetIncomingNode ()) {
					//	Change the node if it exist
					if (rightNode) {
						//	The Right Node has to be an edge
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


		//	Showing the tangents variables

		GUILayout.BeginVertical (GUILayout.MaxHeight (80f));
		GUILayout.FlexibleSpace ();
		EditorGUILayout.LabelField ("Tangents");

		//	Behind Tangent
		EditorGUI.BeginChangeCheck();
		Vector3 backTangent = EditorGUILayout.Vector3Field("  Back Tangent", node.GetTangent(0));

		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Move Tangent");
			node.SetTangent(0, backTangent);
			EnforceMode ();
			node.SetLenght (LenghtUpdate());

		}

		//	Forward Tangent
		EditorGUI.BeginChangeCheck();
		Vector3 frontTangent = EditorGUILayout.Vector3Field("  Front Tangent", node.GetTangent(1));

		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Move Tangent");;
			node.SetTangent(1, frontTangent);
			EnforceMode ();
			node.SetLenght (LenghtUpdate());

		}

		//	 Mode
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

	//	Inspector for tangents only
	private void TangentsInspector() {

		//	Showing the tangent local position
		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", node.GetTangent(tangentSelected));
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Move Point");
			EditorUtility.SetDirty(node);
			node.SetTangent(tangentSelected, point);
			EnforceMode ();
			node.SetLenght (LenghtUpdate());

		}
		//	Showing the mode of the node of the selected tangent
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