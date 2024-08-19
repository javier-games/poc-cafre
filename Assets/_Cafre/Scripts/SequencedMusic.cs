using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeOfTrack{ SAMPLE, LOOP,START, END , LOOPSTART}
public class SequencedMusic : MonoBehaviour {


	[SerializeField]
	private AudioClip intro;
	[SerializeField]
	private AudioClip song;

	private AudioSource[] source;

	void Start(){
		source = GetComponents<AudioSource> ();
		source [0].clip = intro;
		source [1].clip = song;

		source [0].Play ();
		GameManager.instance.ChangeStateEvent += SetSong;
	}

	public void SetSong(){
		switch (GameManager.instance.currentState) {
		case GameState.PLAYING:
			source [0].Stop ();
			source [1].Play ();
			break;
		case GameState.PAUSE:
			source [1].Stop ();
			source [0].Play ();
			break;
		}
	}







	/*
	[System.Serializable]
	public struct SequencedTrack{
		public AudioClip clip;
		public TypeOfTrack type;
		public int[] incomingTracks;
	};

	[SerializeField]
	private SequencedTrack[] tracks;
	[SerializeField]
	private float latency = -0.02f;

	private AudioSource[] source;
	private int clipIndex;
	private bool flagSource = false;

	void Start(){
		source = GetComponents<AudioSource> ();
		source [0].clip = tracks [0].clip;
		source [1].clip = tracks [1].clip;

		source [0].PlayScheduled (AudioSettings.dspTime );
		source [1].PlayScheduled (AudioSettings.dspTime + latency + tracks[0].clip.length);

		clipIndex = 2;
		//StartCoroutine (Alternate(tracks[clipIndex-1].clip.length-0.25f));
	}

	void Update(){
		if (!source [0].isPlaying && !flagSource) {
			source [0].clip = tracks [clipIndex].clip;
			source [0].PlayScheduled (AudioSettings.dspTime + latency + tracks[clipIndex-1].clip.length);
			clipIndex++;
			flagSource = true;
		}
		if (!source [1].isPlaying && flagSource) {
			source [1].clip = tracks [clipIndex].clip;
			source [1].PlayScheduled (AudioSettings.dspTime + latency + tracks[clipIndex-1].clip.length);
			clipIndex++;
			flagSource = false;

		}


	}

	IEnumerator Alternate(float timeToWait){
		yield return new WaitForSecondsRealtime(timeToWait);

		if (!flagSource) {
			source [0].PlayOneShot (tracks [clipIndex].clip);
			clipIndex++;
			flagSource = true;
			StartCoroutine (Alternate(tracks[clipIndex-1].clip.length-0.1f));
		}
		else if(flagSource){
			source [1].PlayOneShot (tracks [clipIndex].clip);
			clipIndex++;
			flagSource = false;
			StartCoroutine (Alternate(tracks[clipIndex-1].clip.length-0.25f));
		}
	}
*/

}
