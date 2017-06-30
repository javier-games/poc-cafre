using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class NodeInspector : Editor {



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
	//	Variables of Inspector
	private  bool showIncomingNodes = false;
	private Node auxiliarNode;

	private MessageType type = MessageType.Info;
	private string message = "";



	//	Modify in scene
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
			if (node.transform.hasChanged) {
				node.SetLenght (LenghtUpdate());
				node.transform.hasChanged = false;
			}
			Node incomingNode = node.GetIncomingNode(0);
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

	//	Drawing tangents
	private Vector3 DrawTangent (int indexTangent) {

		Vector3 tangent;

		tangent = nodeTransform.TransformPoint (node.GetTangent (indexTangent));

		float size;
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
				LenghtUpdate ();
				Undo.RecordObject (node, "Move Tangent");
				EditorUtility.SetDirty (node);
				node.SetTangent (indexTangent, nodeTransform.InverseTransformPoint (tangent));
				node.SetLenght (LenghtUpdate());
			}
		}
		return tangent;
	}



	/******** INSPECTOR ********/
	public override void OnInspectorGUI () {
		if (tangentSelected >= 0 && tangentSelected < 2) {
			TangentsInspector ();
		} else {
			NodesInspector();
		}
	}

	//	Show Node Settings
	private void NodesInspector(){

		/*	LENGHT	*/
		GUILayout.BeginVertical (GUILayout.MaxHeight (30f));
		GUILayout.FlexibleSpace ();
		if (node.IsEdge ()) {
			EditorGUILayout.FloatField ("Lenght", node.GetLenght ());
		} else {
			EditorGUILayout.LabelField ("Single Node");
		}
		GUILayout.EndVertical ();
		

		//	Button to add new nodes
		GUILayout.BeginVertical (GUILayout.MaxHeight(70f));
		GUILayout.FlexibleSpace();

		EditorGUI.BeginChangeCheck();
		Node nodeToAttach = EditorGUILayout.ObjectField ("New Node", auxiliarNode , typeof(Node), true) as Node;
		if (EditorGUI.EndChangeCheck ()) {
			auxiliarNode = nodeToAttach;
			if (auxiliarNode)
				message = "";
		}

		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();

		bool addNode = GUILayout.Button ("Attach a Node", GUILayout.MinWidth (200f), GUILayout.MaxHeight (20f));

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical ();

		if (auxiliarNode) {
			if (node.Exist (auxiliarNode)) {
				type = MessageType.Warning;
				message = "This node already exist.";
				addNode = false;
				auxiliarNode = null;
			}
			if (addNode) {
				node.AddNode (auxiliarNode);
				auxiliarNode = null;
				type = MessageType.Info;
				message = "";
				addNode = false;
				node.SetToEdge ();
				node.SetLenght (LenghtUpdate());
			}
		} else if(addNode){
			type = MessageType.Warning;
			message = "Add a valid node";
			addNode = false;
		}

		//	Foldout of Incoming Nodes
		showIncomingNodes = EditorGUILayout.Foldout (showIncomingNodes, "Adiministrate Nodes");

		if (showIncomingNodes) {
			if (node.IncomingNodesCount () >= 1) {
				
				int nodeToDelete = -1;
				for (int i = 0; i < node.IncomingNodesCount (); i++) {
					EditorGUILayout.ObjectField ("Node "+i, node.GetIncomingNode (i), typeof(Node), true);
					GUILayout.BeginHorizontal ();
					GUILayout.FlexibleSpace();
					if ( GUILayout.Button ( "Delete", GUILayout.MinWidth (70f), GUILayout.MaxHeight (18f) ) ) {
						nodeToDelete = i;
					}
					GUILayout.EndHorizontal ();
				}
				if (nodeToDelete != -1) {
					node.DeleteNode (nodeToDelete);
					node.SetLenght (LenghtUpdate());
				}
			}
			else {
				type = MessageType.Info;
				message = "Draw and Drop an other GameObject with the Node Component, then press the button to conect with this.";
				GUILayout.BeginVertical (GUILayout.MaxHeight(10f));
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical ();
			}
		}

		//	Show Feedback
		if(message!="")
			EditorGUILayout.HelpBox(message, type, true);
	}

	//	Show Tangents Settings
	private void TangentsInspector() {

		EditorGUI.BeginChangeCheck();
		Vector3 point = EditorGUILayout.Vector3Field("Position", node.GetTangent(tangentSelected));

		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Move Point");
			EditorUtility.SetDirty(node);
			node.SetTangent(tangentSelected, point);
			node.SetLenght (LenghtUpdate());

		}
		EditorGUI.BeginChangeCheck();
		BezierMode mode = (BezierMode)EditorGUILayout.EnumPopup("Mode", node.GetMode());
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(node, "Change Point Mode");
			node.SetMode(tangentSelected,mode);
			EditorUtility.SetDirty(node);
			node.SetLenght (LenghtUpdate());
		}
	}


	private float LenghtUpdate(){
		float lenght = 0;
		if (node.IsEdge()) {
			
			int steps = 10;
			Vector3[] stepPoints = new Vector3[steps];

			for (int i = 0; i < steps; i++) {
				Node incomingNode = node.GetIncomingNode(0);
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

}