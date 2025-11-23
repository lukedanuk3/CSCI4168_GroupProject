using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{

    public GameObject doorOpenInstructions;
    public void Interact(){
        Debug.Log("Testing");
        gameObject.transform.Rotate(0,90,0, Space.World);        
        Debug.Log("Interacted with door");
        
        //open or close door...
    }

    private void OnTriggerStay(Collider other){
        if(other.tag == "Door"){
            doorOpenInstructions.SetActive(true);
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
    
}
