using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour {

	[SerializeField]
	private float initialState = 1;
	[SerializeField]
	private float stopTime;
	[SerializeField]
	private float continueTime;
	[SerializeField]
	private float waitTime;

	private Animator animator;
	private float startTime = 0;

	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator>();
		animator.SetFloat ("Signal",initialState);
	}
	
	void OnTriggerEnter(Collider other){
		switch (animator.GetFloat ("Signal").ToString()) {
		case "1":
			StartCoroutine (ChangeState (continueTime, 0));
			startTime = Time.time;
			break;
		case "0":
			StartCoroutine ( ChangeState(stopTime,-1) );
			startTime = Time.time;
			break;
		case "-1":
			StartCoroutine ( ChangeState(stopTime,1) );
			startTime = Time.time;
			break;
		}
	}
	void OnTriggerStay (Collider other){
		switch (animator.GetFloat ("Signal").ToString()) {
		case "1":
			if(Time.time - startTime > continueTime)
				StartCoroutine ( ChangeState(continueTime,0) );
			break;
		case "0":
			if(Time.time - waitTime > continueTime)
				StartCoroutine ( ChangeState(stopTime,-1) );
			break;
		case "-1":
			if(Time.time - stopTime > continueTime)
				StartCoroutine ( ChangeState(stopTime,1) );
			break;
		}
	}
	void OnTriggerExit(Collider other){
		if (animator.GetFloat ("Signal").ToString () == "-1") {
			Debug.Log ("Multa");
		}
	}

	IEnumerator ChangeState(float waitingTime,float valueState){
		yield return new WaitForSeconds (waitingTime);
		animator.SetFloat ("Signal",valueState);
	}

}
