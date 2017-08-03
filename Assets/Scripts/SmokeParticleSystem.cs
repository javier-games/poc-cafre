using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeParticleSystem : MonoBehaviour {

	[SerializeField]
	private float smooth = 0.5f;
	[SerializeField]
	private ParticleSystem systemLeft;
	[SerializeField]
	private ParticleSystem systemRight;

	private ParticleSystem.MainModule mainLeft;
	private ParticleSystem.MainModule mainRight;

	private Transform follower;
	private Vector3 velocity;

	// Use this for initialization
	void Start () {
		
		follower = new GameObject("SystemParticleFollower").transform;
		mainLeft = systemLeft.main;
		mainRight = systemRight.main;

		follower.position = transform.position;
		mainLeft.customSimulationSpace = follower;
		mainRight.customSimulationSpace = follower;

		systemLeft.Play();
		systemRight.Play ();
	}

	void Update(){
		follower.position = Vector3.SmoothDamp (follower.position,transform.position,ref velocity,smooth);
	}
}
