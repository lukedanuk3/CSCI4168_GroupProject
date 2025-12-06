using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{

    private IEnumerator coroutineInstance;
    private float originalRotation;
    private float newRotation;
    private float currentRotation;
    public AudioSource openSound;
    public AudioSource closeSound;
    Animator anim;

    private void Start(){
        anim = gameObject.GetComponent<Animator>();
        originalRotation = gameObject.transform.localEulerAngles.y;
        anim.SetBool("IsClosed", true);
    }
    public void Interact(){
        Open();
        
    }
    
    public void Open(){
        Debug.Log(anim.GetBool("IsClosed"));
        if(anim.GetBool("IsClosed") == true){
            if(anim.GetBool("HasBeenOpened") == false){
                Debug.Log("Door has never been opened before");
                anim.SetBool("HasBeenOpened", true);
            }
            Debug.Log("Opening Door");
            // openSound.Play();
            anim.SetBool("IsClosed", false);
            anim.SetTrigger("OpenClose");

            coroutineInstance = CloseDoor(anim, gameObject, 2.0f);    
            StartCoroutine(coroutineInstance);
   
        }
        else{
            if(coroutineInstance != null){
                StopCoroutine(coroutineInstance);

                if(openSound.isPlaying){
                    openSound.Stop();
                }
                // closeSound.Play();
                anim.SetBool("IsClosed", true);  
                anim.SetTrigger("OpenClose");
            }
            Debug.Log("Door hates ya");

        }
    }
    IEnumerator CloseDoor(Animator anim, GameObject door, float delayTime){
        yield return new WaitForSeconds(delayTime);
        if(anim.GetBool("IsClosed") == false){
            Debug.Log("Door is opened");
            // if(openSound.isPlaying){
            //     openSound.Stop();
            // }
            // closeSound.Play();
            anim.SetBool("IsClosed", true);
            anim.SetTrigger("OpenClose");  
            }

    }
}
