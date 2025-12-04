using UnityEngine;
using System.Collections.Generic;

public class ToolList : MonoBehaviour
{
    public Dictionary<string, GameObject> toolDictionary = new Dictionary<string, GameObject>();
    public GameObject crowbar;
    public GameObject wireCutters;
    public GameObject boltCutters;

    public GameObject player;

    void Awake(){
        toolDictionary.Add("Crowbar", crowbar);
        toolDictionary.Add("Wire Cutters", wireCutters);
        toolDictionary.Add("Bolt Cutters", boltCutters);
    }

}
