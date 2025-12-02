using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
 
public class ItemManager : MonoBehaviour
{
 
    public static ItemManager instance;
 
    private string[] tools = new string[2];
 
    public GameObject slot1_selection;
    public GameObject slot2_selection;
    
 
    public GameObject slot1_gameplay;
    public GameObject slot2_gameplay;
    [Space]
 
    public Sprite crowbar;
    public GameObject crowbarObject;
    [Space]
    public Sprite boltcutter;
    public GameObject boltcutterObject;
    [Space]
 
    public Sprite wirecutter;
    public GameObject wirecutterObject;
    [Space]
 
    public PlayerControl playerController;

    public bool shouldLoadMemory;
 
    void Awake(){

        if (shouldLoadMemory){
            string[] toolMemory = PlayerPrefs.GetString("UIMemory").Split("/");
            tools[0] = toolMemory[0];
            tools[1] = toolMemory[1];
        }

        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        slot1_selection.SetActive(false);
        slot2_selection.SetActive(false);
    }
 
    public void updateItems(string s){
        s = s.ToLower();

        if (tools[0] == s || tools[1] == s)
            return;

        if (tools[0] == null)
        {
            tools[0] = s;
            AddToolToSlot(0, s);
            return;
        }

        if (tools[1] == null)
        {
            tools[1] = s;
            AddToolToSlot(1, s);
            return;
        }

        tools[1] = tools[0];
        tools[0] = s;
        AddToolToSlot(0, s);
        AddToolToSlot(1, tools[1]);
    }
 
    // public void PlaySelectSound(AudioSource pickUpSound){
    //     pickUpSound.Play();
    // }

    private void AddToolToSlot(int index, string tool)
{
        if (index == 0)
        {
            slot1_selection.SetActive(true);
            slot1_gameplay.SetActive(true);
        }
        else
        {
            slot2_selection.SetActive(true);
            slot2_gameplay.SetActive(true);
        }

        GameObject toolPrefab = GetToolPrefabByName(tool);

        if (toolPrefab != null)
        {
            playerController.PickUpTool(toolPrefab);
            playerController.ChangeCurrentSlot();
        }
        else
        {
            Debug.LogWarning("ItemManager.AddToolToSlot: no prefab found for tool: " + tool);
        }

        UpdateUI();
    }

    private GameObject GetToolPrefabByName(string tool){
        switch (tool)
        {
            case "crowbar":   
                return crowbarObject;
            case "boltcutter":
                return boltcutterObject;
            case "wirecutter":
                return wirecutterObject;
            default: return null;
        }
    }

 
    private void UpdateUI()
    {
        UpdateSlotUI(0, tools[0]);
        UpdateSlotUI(1, tools[1]);

        playerController.updateItems(tools[0], 0);
        playerController.updateItems(tools[1], 1);
 
        //string[] items = playerController.getItems();
        //Debug.Log("Debug test player controller items: 1: " + items[0] + " 2: " + items[1]);
 
    }

    private void UpdateSlotUI(int index, string tool)
    {
        GameObject selection = (index == 0) ? slot1_selection : slot2_selection;
        GameObject gameplay  = (index == 0) ? slot1_gameplay  : slot2_gameplay;

        if (tool == null)
        {
            selection.SetActive(false);
            gameplay.SetActive(false);
            return;
        }

        selection.SetActive(true);
        gameplay.SetActive(true);

        Sprite sprite = null;

        switch (tool)
        {
            case "crowbar": sprite = crowbar; break;
            case "boltcutter": sprite = boltcutter; break;
            case "wirecutter": sprite = wirecutter; break;
        }

        selection.GetComponent<Image>().sprite = sprite;
        gameplay.GetComponent<Image>().sprite = sprite;
    }

 
    private void debugPrint()
    {
        Debug.Log("tool1: " + tools[0] + " tool2: " + tools[1]);
    }
 
    public void confirmSelection(){
        UpdateUIMemory();
        playerController.UpdateGlobalInventory();
        playerController.finishSelectingTools();
    }

    void UpdateUIMemory(){
        PlayerPrefs.SetString("UIMemory", tools[0] + "/" + tools[1]);
    }
    
}
 