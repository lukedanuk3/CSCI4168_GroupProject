using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{

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
        if(gameObject.transform.rotation.y == 0){
            gameObject.transform.Rotate(0,-90,0, Space.World);        
        }
        else{
            gameObject.transform.Rotate(0,90,0, Space.World);        
        }
        StartCoroutine(CloseDoor(gameObject, 2.0f));
    }
    IEnumerator CloseDoor(GameObject door, float delayTime){
        yield return new WaitForSeconds(delayTime);
        if(door.transform.rotation.y != 0){
            door.transform.Rotate(0, 90, 0, Space.World);
        }
    }
}
