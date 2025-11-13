using UnityEngine;

public class InteractionHandler : MonoBehaviour
{

    //Constant types for objects -- each one determines interaction behaviour;
    //Allows for script modularity (same script for tools and other interactables)
    const string TOOL = "TOOL";
    const string HEALTH_BOOST = "HEALTHBOOST";
    //etc, etc...

    public string objectType;

    public void Interact(GameObject player){
        if (objectType == TOOL){
            player.GetComponent<PlayerControl>().EquipTool(gameObject);
        }else if (objectType == HEALTH_BOOST){
            //boost health or something idk lol this is a placeholder example
        }
    }
}
