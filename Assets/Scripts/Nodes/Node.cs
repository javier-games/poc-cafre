using UnityEngine;

/*
 * This structure help to make a path builded by Nodes 
 * that an endless runner character can follows
*/

public class Node : MonoBehaviour {

	//	Variables

	//	Variables of Single Node and Node-Edges
	[SerializeField]
	private Vector3[]	tangent;					//	Couple of Points to make a Bezier Curve 
	[SerializeField]
	private BezierMode	mode;						//	Mode of use of the Bezier Tangent Points

	//	Variables for Node that are Edges only
	//	A node change to edge if it has an incomingNode
	[SerializeField]
	private Node		incomingNode;				//	Node that follows after this one
	[SerializeField]
	private Node		leftNode;					//	Node to the left
	[SerializeField]
	private Node		rightNode;					//	Node to the right
	[SerializeField]
	private float		lenght		= 0;			//	Lenght of the Bezier Curve of the edge
	[SerializeField]
	private bool		isEdge		= false;		//	Flag to know if the Node is also an Edge
	private float		indexTime	= 0;			//	Indicates the proportion of the edge traveled



	//	Reset Methods

	public void Reset () {
		tangent = new Vector3[] {
			new Vector3(0f, 0f, -1f),	//	Tangent to the behind node
			new Vector3(0f, 0f, 1f),	//	Tangent to the incoming node
		};
		mode = BezierMode.Aligned;
	}



	//	Get Methods

	public Vector3 GetTangent (int index) {
		return tangent[index];
	}
	public BezierMode GetMode () {
		return mode;
	}
	public Node GetIncomingNode(){
		return incomingNode;
	}
	public Node GetLeftNode(){
		return leftNode;
	}
	public Node GetRightNode(){
		return rightNode;
	}
	public float GetLenght(){
		return lenght;
	}
	public float GetIndexTime(){
		return indexTime;
	}
	public bool IsEdge(){
		SetToEdge ();
		return isEdge;
	}



	//	Set Methods

	public void SetTangent (int index, Vector3 tangent) {
		this.tangent [index] = tangent;
	}
	public void SetMode ( BezierMode mode ) {
		this.mode = mode;
	}
	public void SetIncomingNode( Node incomingNode ){
		this.incomingNode = incomingNode;
		SetToEdge ();
	}
	public void SetLeftNode( Node leftNode ){
		this.leftNode = leftNode;
	}
	public void SetRightNode( Node rightNode ){
		this.rightNode = rightNode;
	}
	public void SetToEdge(){
		isEdge = incomingNode ? true : false;
	}
	public void SetLenght(float lenght){
		this.lenght = lenght;
	}
	public void SetIndexTime(float indexTime){
		this.indexTime = indexTime;
	}
	public void ResetIndexTime(){
		indexTime = 0;
	}



	//	Psition Methods

	//	Obtaining a point in a Bezier Curve
	public Vector3 BezierPoint(Vector3 p0, Vector3 p3, Vector3 p1, Vector3 p2, float t){
		float OneMinusT = 1f - t;
		//	Applying the formula
		return
			OneMinusT * OneMinusT * OneMinusT * p0 +
			3f * OneMinusT * OneMinusT * t * p1 +
			3f * OneMinusT * t * t * p2 +
			t * t * t * p3;
	}
	//	Method to update the position of the indexTime	
	public Vector3 GetPosition(float speed,out bool changeNode){
		//	Acumulate the proportion of length traveled acording to de speed and deltaTime
		indexTime += (Time.deltaTime * speed) / lenght;
		changeNode = false;
		//	If the index is grather than the total length return a true value by reference
		if (indexTime >= 1) {
			changeNode = true;
		}
		//	Returning the new position
		return BezierPoint (
				transform.position,
				incomingNode.transform.position,
				transform.TransformPoint (tangent [1]),
				incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
				indexTime
		);
	}

}

//	Mode of use of the Bezier Tangent Points
public enum BezierMode {
	Free,					//	The tangent points can be moved in a free way
	Aligned,				//	The tangent points are aligned
	Mirrored				//	The tangent points has the same distance to 
							//	node.transform.position and are aligned
}