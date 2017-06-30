using UnityEngine;
using System;
using UnityEditor;

public class Node : MonoBehaviour {


	[SerializeField]
	private Vector3[]	tangent;
	[SerializeField]
	private BezierMode	mode;

	[SerializeField]
	private Node[] 		incomingNodes;
	[SerializeField]
	private bool isEdge = false;
	[SerializeField]
	private float lenght = 0;

	private float indexTime = 0;

	[SerializeField]
	private Node leftNode;
	[SerializeField]
	private Node rightNode;



	public void Reset () {
		tangent = new Vector3[] {
			new Vector3(0f, 0f, -1f),	//	Tangent to the back node
			new Vector3(0f, 0f, 1f),	//	Tangent to the incoming node
		};
		mode = BezierMode.Mirrored;
	}
		


	//	Constraint Tangents

	private void EnforceMode (int index) {

		if (mode == BezierMode.Free)
			return;

		int fixedIndex;
		int enforcedIndex;

		if ( index < 1 ) {
			fixedIndex = 0;
			enforcedIndex = 1;
		}
		else {
			fixedIndex = 1;
			enforcedIndex = 0;
		}

		Vector3 middle = Vector3.zero;
		Vector3 enforcedTangent = middle - tangent[fixedIndex];
		if (mode == BezierMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, tangent[enforcedIndex]);
		}
		tangent[enforcedIndex] = middle + enforcedTangent;
	}



	//	Get a Bezier Point

	public Vector3 BezierPoint(Vector3 p0, Vector3 p3, Vector3 p1, Vector3 p2, float t){
		//t = Mathf.Clamp01(t);
		float OneMinusT = 1f - t;
		return
			OneMinusT * OneMinusT * OneMinusT * p0 +
			3f * OneMinusT * OneMinusT * t * p1 +
			3f * OneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	public Vector3 GetPoint(float speed,out bool ended){
		indexTime += (Time.deltaTime*speed) / lenght;
		Node incomingNode = incomingNodes [0];
		Vector3 point = BezierPoint (
			transform.position,
			incomingNode.transform.position,
			transform.TransformPoint (tangent [1]),
			incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
			indexTime
		);
		ended = false;
		if (indexTime >= 1) {
			ended = true;
			indexTime = 0;
		}
		return point;
	}

	public Quaternion GetRotation(int index){
		return Quaternion.Lerp (transform.rotation,incomingNodes[index].transform.rotation,indexTime);
	}



	//	Get Methods

	public Vector3 GetTangent (int index) {
		return tangent[index];
	}
	public BezierMode GetMode () {
		return mode;
	}
	public bool IsEdge(){
		return isEdge;
	}
	public Node GetIncomingNode(int index){
		return incomingNodes [index].GetComponent<Node>();
	}
	public int IncomingNodesCount(){
		return incomingNodes.Length;
	}
	public Node[] GetIncomingNodes(){
		return incomingNodes;
	}
	public float GetLenght(){
		return lenght;
	}



	//	Set Methods

	public void SetTangent (int index, Vector3 tangent) {
		this.tangent [index] = tangent;
		EnforceMode(index);
	}
	public void SetMode (int index, BezierMode mode) {
		this.mode = mode;
		EnforceMode(index);
	}
	public void SetIncomingNode(int index,Node node){
		incomingNodes [index] = node;
	}
	public void SetToEdge(){
		if (incomingNodes.Length >= 1) {
			if (incomingNodes [0]) {
				isEdge = true;
			} else {
				isEdge = false;
			}
		}
	}
	public void SetToNoEdge(){
		incomingNodes = new Node[] {};
		isEdge = false;
	}
	public void AddNode(Node node){
		Array.Resize (ref incomingNodes, incomingNodes.Length+1);
		incomingNodes [incomingNodes.Length - 1] = node;
	}
	public void DeleteNode(int index){
		Node[] newIncomingNodes = new Node[incomingNodes.Length - 1];

		int i = 0;
		int j = 0;
		while (i < incomingNodes.Length)
		{
			if (i != index)
			{
				newIncomingNodes[j] = incomingNodes[i];
				j++;
			}
			i++;
		}
		incomingNodes = newIncomingNodes;
		if (incomingNodes.Length <= 0)
			isEdge = false;
	}
	public void SetLenght(float lenght){
		this.lenght = lenght;
	}
	public void ResetIndex(){
		indexTime = 0;
	}



	// Comparation Methods

	public bool Exist (Node node){
		bool exist = false;
		for (int i = 0; i < incomingNodes.Length; i++) {
			if(node.transform.name == incomingNodes[i].transform.name ){
				exist = true;
			}
		}
		return exist;
	}



	//	Draw Methods

	private void OnDrawGizmos(){
		if (isEdge) {
			for (int i = 0; i < incomingNodes.Length; i++) {
				Node incomingNode = incomingNodes [i];
				Handles.DrawBezier (
					transform.position,
					incomingNode.transform.position,
					transform.TransformPoint (tangent [1]),
					incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
					Color.gray,
					null,
					2f
				);
			}
		}
	}	


}

public enum BezierMode {
	Free,
	Aligned,
	Mirrored
}
