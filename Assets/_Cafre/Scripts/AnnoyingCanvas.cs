using UnityEngine;
using System.Collections;

public class AnnoyingCanvas : MonoBehaviour {

	public static AnnoyingCanvas instance;

	[SerializeField] private Transform target;
	[SerializeField] private Vector3 offset;
	[SerializeField] private float smooth = 0.4f;
	[SerializeField] private float mesaggeDuration = 2f;
	[SerializeField] private Transform filcherMessage;
	[SerializeField] private int firstMessage;
	[SerializeField] private Transform[] messages;

	private bool first = true;
	private Vector3 velocity;

	void Awake(){
		instance = this;
	}

	void Start(){
		messages[firstMessage].gameObject.SetActive (false);
	}

	// Update is called once per frame
	void LateUpdate () {
		transform.position = Vector3.SmoothDamp (transform.position, target.position + offset, ref velocity, smooth);
		transform.LookAt (Camera.main.transform.forward + target.position + offset);
	}

	public void Assault(bool state){
		filcherMessage.gameObject.SetActive (state);
	}

	public void SendAMessage(){
		if (first) {
			messages[firstMessage].gameObject.SetActive (true);
			StartCoroutine (ShotMessage (firstMessage));
			first = false;
		} else {
			for(int i = 0; i<messages.Length; i++){
				int j = Random.Range (i,messages.Length);
				if (!messages [j].gameObject.activeInHierarchy) {
					messages [j].gameObject.SetActive (true);
					StartCoroutine (ShotMessage (j));
					break;
				}
			}

		}
	}

	IEnumerator ShotMessage(int childIndex){
		yield return new WaitForSeconds(mesaggeDuration);
		messages[childIndex].gameObject.SetActive (false);
		if (childIndex == firstMessage) {
			first = true;
		}
	}

}
