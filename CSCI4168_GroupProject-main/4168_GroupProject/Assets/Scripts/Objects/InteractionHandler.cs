using UnityEngine;

public class InteractionHandler : MonoBehaviour
{

    //Constant types for objects -- each one determines interaction behaviour;
    //Allows for script modularity (same script for tools and other interactables)
    const string TOOL = "TOOL";
    const string DOOR = "DOOR";
    const string BUTTON = "BUTTON";
    const string TOOLSELECTOR = "TOOLS";
    const string LEVELSELECTOR = "LEVEL";

    public string objectType;

    public void Interact(GameObject player){
        if (objectType == TOOL){
            player.GetComponent<PlayerControl>().PickUpTool(gameObject);
        }else if (objectType == DOOR){
            gameObject.GetComponent<DoorHandler>().Interact();
        }else if (objectType == BUTTON){
            //TODO: buttons?
        }
        else if(objectType == TOOLSELECTOR){
            Debug.Log("Selecting tools");
            player.GetComponent<PlayerControl>().selectTools();

        }
        else if(objectType == LEVELSELECTOR){
            player.GetComponent<PlayerControl>().selectLevel();
        }
    }

}