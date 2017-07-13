using UnityEngine;

public class Node : MonoBehaviour {

	[SerializeField]
	private Vector3[]	tangent;
	[SerializeField]
	private BezierMode	mode;

	[SerializeField]
	private Node		incomingNode;
	[SerializeField]
	private Node		leftNode;
	[SerializeField]
	private Node		rightNode;
	[SerializeField]
	private float		lenght		= 0;
	[SerializeField]
	private bool		isEdge		= false;
	private float		indexTime	= 0;



	/*	RESET METHOD	*/

	public void Reset () {
		tangent = new Vector3[] {
			new Vector3(0f, 0f, -1f),	//	Tangent to the back node
			new Vector3(0f, 0f, 1f),	//	Tangent to the incoming node
		};
		mode = BezierMode.Aligned;
	}



	/*	GET METHODS	*/

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



	/*	SET METHODS	*/

	public void SetTangent (int index, Vector3 tangent) {
		this.tangent [index] = tangent;
		EnforceMode(index);
	}
	public void SetMode (int index, BezierMode mode) {
		this.mode = mode;
		EnforceMode(index);
	}
	public void SetIncomingNode( Node incomingNode){
		this.incomingNode = incomingNode;
		SetToEdge ();
	}
	public void SetLeftNode( Node leftNode){
		this.leftNode = leftNode;
	}
	public void SetRightNode( Node rightNode){
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
	public void ResetIndex(){
		indexTime = 0;
	}
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



	/*	BEZIER METHODS	*/

	public Vector3 BezierPoint(Vector3 p0, Vector3 p3, Vector3 p1, Vector3 p2, float t){
		//t = Mathf.Clamp01(t);
		float OneMinusT = 1f - t;
		return
			OneMinusT * OneMinusT * OneMinusT * p0 +
			3f * OneMinusT * OneMinusT * t * p1 +
			3f * OneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	public Vector3 GetPosition(float speed,out bool ended){
		Vector3 position;
		ended = false;
		if (isEdge) {
			indexTime += (Time.deltaTime * speed) / lenght;
			position = BezierPoint (
				transform.position,
				incomingNode.transform.position,
				transform.TransformPoint (tangent [1]),
				incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
				indexTime
			);
			if (indexTime >= 1) {
				ended = true;
				indexTime = 0;
			}
		} else {
			position = transform.position;
		}
		return position;
	}
	public Vector3 GetPosition(float index){
		Vector3 position;
		if (isEdge) {
			position = BezierPoint (
				transform.position,
				incomingNode.transform.position,
				transform.TransformPoint (tangent [1]),
				incomingNode.transform.TransformPoint (incomingNode.GetTangent (0)),
				index
			);
		} else {
			position = transform.position;
		}
		return position;
	}

	public Quaternion GetRotation(){
		return Quaternion.Lerp (transform.rotation,incomingNode.transform.rotation,indexTime);
	}

}

public enum BezierMode {
	Free,
	Aligned,
	Mirrored
}