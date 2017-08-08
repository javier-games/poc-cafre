using UnityEngine;
using System.Collections;

public class AnnoyingCanvas : MonoBehaviour {

	public static AnnoyingCanvas instance;
	
	[SerializeField] private Transform target;
	[SerializeField] private Vector3 offset;
	[SerializeField] private float smooth = 0.4f;
	[SerializeField] private float mesaggeDuration = 2f;
	[SerializeField] private Transform firstMessage;

	private bool first = true;
	private Vector3 velocity;

	void Awake(){
		instance = this;
	}

	void Start(){
		firstMessage.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void LateUpdate () {
		transform.position = Vector3.SmoothDamp (transform.position, target.position + offset, ref velocity, smooth);
		transform.LookAt (Camera.main.transform.forward + target.position + offset);
	}

	public void SendAMessage(){
		if (first) {
			firstMessage.gameObject.SetActive (true);
			StartCoroutine (ShotMessage (firstMessage.GetSiblingIndex()));
			first = false;
		} else {
			for (int i = 0; i < transform.childCount; i++) {
				if (!transform.GetChild (i).gameObject.activeInHierarchy) {
					transform.GetChild (i).gameObject.SetActive (true);
					StartCoroutine (ShotMessage (i));
					break;
				}
			}
		}
	}

	IEnumerator ShotMessage(int childIndex){
		yield return new WaitForSeconds(mesaggeDuration);
		transform.GetChild (childIndex).gameObject.SetActive (false);
		if (transform.GetChild (childIndex) == firstMessage) {
			first = true;
		}
	}


}