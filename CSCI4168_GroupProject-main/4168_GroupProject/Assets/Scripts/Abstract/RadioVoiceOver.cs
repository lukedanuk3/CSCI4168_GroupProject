using UnityEngine;

public class RadioVoiceOver : MonoBehaviour
{
    public static RadioVoiceOver instance;
    public GameObject skipInstructions;
    public AudioSource instructionsSource;
    public AudioSource skipAudio;
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
    }

    //This method will stop the music when called
    public void StopMusic(){
        Debug.Log("Stopping music");
        if(instructionsSource.isPlaying){
            instructionsSource.Stop();
        }
        skipAudio.Play();
        skipInstructions.SetActive(false);
    }

    //This method will play the music when called
    public void PlayMusic(){
        if(!instructionsSource.isPlaying){
            instructionsSource.Play();
        }
    }

}
