using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

	[SerializeField]
	private AudioMixer mainAudioMixer;
	[SerializeField]
	private AudioClip tap;
	[SerializeField]
	private AudioClip[] readyCount;
	[SerializeField]
	private AudioClip[] annoyingVoices;
	[SerializeField]
	private AudioClip crash;

    
	void Awake(){
        instance = this;        
    }

    void Start (){
        GameManager.instance.ChangeStateEvent += GameStateChange;	
	}

    void GameStateChange(){
        switch (GameManager.instance.currentState){
            case GameState.PLAYING: 
                break;
            case GameState.PAUSE:
                break;
            case GameState.CONTINUE:
                break;
            case GameState.GAME_OVER:
                break;
        }
    }


	//	One Shot Methods
	public void Tap(AudioSource source){
		source.PlayOneShot (tap);
	}
	public void ReadyCount(int index,AudioSource source){
		if (index > 3)
			source.PlayOneShot (readyCount [1]);
		else
			source.PlayOneShot (readyCount [0]);
	}
	public void PlayAnnoingVoice(int index, AudioSource source){
		if(!source.isPlaying)
			source.PlayOneShot (annoyingVoices [index]);
	}
	public void Crash(AudioSource source){
		if (!source.isPlaying)
			source.PlayOneShot (crash);
	}


	/*

    public void SetSfxGroupVolume(float volume){
        mainAudioMixer.SetFloat("SFXVolume", volume);
    }
    public void SetMusicGroupVolume(float volume){
        mainAudioMixer.SetFloat("MusicVolume", volume);
    }*/
}
