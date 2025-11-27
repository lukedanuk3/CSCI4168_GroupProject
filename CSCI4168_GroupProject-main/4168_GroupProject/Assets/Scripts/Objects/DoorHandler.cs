using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{

    private IEnumerator coroutineInstance;
    private float originalRotation;
    private float newRotation;
    private float currentRotation;

    private void Start(){
        originalRotation = gameObject.transform.localEulerAngles.y;
    }
    public GameObject doorOpenInstructions;
    public void Interact(){
        Open();
        
        //open or close door...
    }

    private void OnTriggerStay(Collider other){
        if(other.tag == "Door"){
            if(Input.GetKeyDown(KeyCode.E)){
                Interact();
                other.transform.Rotate(0,90,0, Space.World);
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.tag == "Door"){
            doorOpenInstructions.SetActive(false);
            other.transform.Rotate(0,-90,0, Space.World);
        }
    }
    
    public void Open(){
        Animator anim = gameObject.GetComponent<Animator>();
        Debug.Log("Current Rotation: " + gameObject.transform.localEulerAngles.y);
        Debug.Log("Original Rotation: " + originalRotation);
        if(gameObject.transform.localEulerAngles.y == originalRotation){
            if(anim.GetBool("HasBeenOpened") == false){
                anim.SetBool("HasBeenOpened", true);
            }
            Debug.Log("Opening Door");
            anim.SetBool("IsClosed", false);
            anim.SetTrigger("OpenClose");

            // gameObject.transform.Rotate(0,-90,0, Space.World); 
            coroutineInstance = CloseDoor(anim, gameObject, 2.0f);    
            StartCoroutine(coroutineInstance);
   
        }
        else{
            if(coroutineInstance != null){
                StopCoroutine(coroutineInstance);
                anim.SetBool("IsClosed", true);  
                anim.SetTrigger("OpenClose");
            }
            Debug.Log("Door hates ya");

        }
        // anim.SetTrigger("OpenClose");     
    }
    IEnumerator CloseDoor(Animator anim, GameObject door, float delayTime){
        yield return new WaitForSeconds(delayTime);
        if(door.transform.localEulerAngles.y != 0){
            if(anim.GetBool("IsClosed") == false){
                Debug.Log("Door is opened");
                anim.SetBool("IsClosed", true);
                anim.SetTrigger("OpenClose");  
            }

            // door.transform.Rotate(0, 90, 0, Space.World);
        }
    }
}
