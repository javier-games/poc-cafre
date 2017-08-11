using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RunnerSoundFX : MonoBehaviour {

	[SerializeField]
	private AudioClip startFX;
	[SerializeField]
	private AudioClip engineFX;
	[SerializeField]
	private AudioClip runFX;
	[SerializeField]
	private AudioClip brakeFX;
	[SerializeField]
	private AudioClip explotionFX;
	[SerializeField]
	private AudioClip horizontalFX;
	[SerializeField]
	private AudioClip goldenTossFX;
	[SerializeField]
	private AudioClip silverTossFX;
	[SerializeField]
	private AudioClip takePassengerFX;
	[SerializeField]
	private AudioClip wrongFX;

	private AudioSource soundfx;
	private AudioSource engine;

	// Use this for initialization
	void Start () {
		soundfx = GetComponents <AudioSource>()[1];
		engine = GetComponents <AudioSource>()[0];

		engine.clip = engineFX;
		engine.loop = true;


		GameManager.instance.ChangeStateEvent += GameStateChange;	
	}

	void GameStateChange(){
		switch (GameManager.instance.currentState){
		case GameState.PREPARE:
			engine.PlayOneShot(startFX);
			engine.PlayScheduled (AudioSettings.dspTime - 0.1f + startFX.length);
			break;
		case GameState.PLAYING:
			break;
		case GameState.PAUSE:
			break;
		case GameState.GAME_OVER:
			break;
		}
	}

	public void RunFX(){
		//soundfx.Stop ();
		soundfx.PlayOneShot (runFX);
	}

	public void BrakeFX(){
		//soundfx.Stop ();
		soundfx.PlayOneShot (brakeFX);
	}

	public void HorizontalFX(){
		soundfx.PlayOneShot (horizontalFX);
	}

	public void ExplotionFX(){
		soundfx.PlayOneShot (explotionFX);
	}

	public void GoldenTossFX(){
		soundfx.PlayOneShot (goldenTossFX);
	}

	public void SilverTossFX(){
		soundfx.PlayOneShot (silverTossFX);
	}

	public void TakePassengerFX(){
		soundfx.PlayOneShot (takePassengerFX);
	}

	public void WrongFX(){
		soundfx.PlayOneShot (wrongFX);
	}



}
