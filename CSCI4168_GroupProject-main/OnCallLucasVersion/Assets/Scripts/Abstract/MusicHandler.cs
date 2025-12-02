using UnityEngine;

//This class will be used to manage the background music, that will play throughout the level.
//It's also been given methods that can be used when we reach the game over or victory screens, to end the music, and play their own respective pieces
public class MusicHandler : MonoBehaviour
{

    public static MusicHandler instance;
    private AudioSource audioSource;
    
    void Awake(){
        if(instance == null){
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }
        else{
            Destroy(gameObject);
        }
    }

    //This method will stop the music when called
    public void StopMusic(){
        if(audioSource.isPlaying){
            audioSource.Stop();
        }
    }

    //This method will play the music when called
    public void PlayMusic(){
        if(!audioSource.isPlaying){
            audioSource.Play();
        }
    }
}
