using UnityEngine;

public class RadioVoiceOver : MonoBehaviour
{
    public static RadioVoiceOver instance;
    public GameObject skipInstructions;
    public AudioSource instructionsSource;
    public AudioSource skipAudio;
    public AudioSource radioStatic;
    public bool hasPlayedBefore;
    
    void Awake(){
        if(instance == null){
            instance = this;
            instructionsSource = GetComponent<AudioSource>();
        }
        else{
            Destroy(gameObject);
        }
        skipInstructions.SetActive(true);
        PlayRadioVoiceOver();
    }

    void Start(){

        if(PlayerPrefs.GetInt("HasPlayed") == 0){
            PlayRadioVoiceOver();
            PlayerPrefs.SetInt("HasPlayed", 1);
        }
    }
    void Update(){
        if(skipAudio.isPlaying || instructionsSource.isPlaying){
            if(!radioStatic.isPlaying){
                radioStatic.Play();
            }
        }
        if(!skipAudio.isPlaying && !instructionsSource.isPlaying){
            radioStatic.Stop();
        }
    }

    //This method will stop the music when called
    public void StopRadioVoiceOver(){
        Debug.Log("Stopping music");
        if(instructionsSource.isPlaying){
            instructionsSource.Stop();
        }
        skipAudio.Play();
        skipInstructions.SetActive(false);
    }

    //This method will play the music when called
    public void PlayRadioVoiceOver(){
        Debug.Log("Starting audio");
        if(!instructionsSource.isPlaying){
            instructionsSource.Play();
        }
    }

}
