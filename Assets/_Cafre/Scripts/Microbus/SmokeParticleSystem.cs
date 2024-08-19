using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticleSystem : MonoBehaviour {

	[SerializeField]
	private GameObject systemPrefab;
	[SerializeField]
	private float smooth = 0.5f;
	[SerializeField]
	private Vector3 leftSystemPosition;
	[SerializeField]
	private Vector3 rightSystemPosition;

	private GameObject leftSystem;
	private GameObject rightSystem;

	private ParticleSystem.MainModule mainLeft;
	private ParticleSystem.MainModule mainRight;

	private Transform follower;
	private Vector3 velocity;

	// Use this for initialization
	void Start () {

		follower = new GameObject("System Particle Reference").transform;
		follower.position = transform.position;

		leftSystem = Instantiate (systemPrefab, leftSystemPosition, Quaternion.LookRotation (-transform.forward));
		rightSystem = Instantiate (systemPrefab, rightSystemPosition, Quaternion.LookRotation(-transform.forward));

		leftSystem.transform.parent = transform;
		rightSystem.transform.parent = transform;

		leftSystem.transform.position = transform.TransformPoint(leftSystemPosition);
		rightSystem.transform.position = transform.TransformPoint(rightSystemPosition);

		mainLeft = leftSystem.transform.GetComponent<ParticleSystem> ().main;
		mainRight = rightSystem.transform.GetComponent<ParticleSystem> ().main;

		mainLeft.customSimulationSpace = follower;
		mainRight.customSimulationSpace = follower;

	}

	void Update(){
		follower.position = Vector3.SmoothDamp (follower.position,transform.position,ref velocity,smooth);
	}
}
