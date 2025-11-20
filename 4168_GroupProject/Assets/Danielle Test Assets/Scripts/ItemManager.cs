using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{

    private string tool1;
    private string tool2;

    public GameObject slot1;
    public GameObject slot2;

    public Sprite crowbar;
    public Sprite boltcutter;
    public Sprite wirecutter;

    public PlayerControl playerController;

    public void Start()
    {
        slot1.SetActive(false);
        slot2.SetActive(false);
    }

    public void updateItems(string s)
    {
        if (s == "CROWBAR") 
        {
            if (tool1 != "crowbar" && tool2 != "crowbar")
            {
                if (tool1 == null)
                {
                    tool1 = "crowbar";
                    slot1.SetActive(true);
                    updateUI();
                    return;
                }
                if (tool2 == null)
                {
                    tool2 = "crowbar";
                    slot2.SetActive(true);
                    updateUI();
                    return;
                }
                tool2 = tool1;
                tool1 = "crowbar";
                updateUI();
                return;
            } 
        }
        if (s == "BOLTCUTTER")
        {
            if (tool1 != "boltcutter" && tool2 != "boltcutter")
            {
                if (tool1 == null)
                {
                    tool1 = "boltcutter";
                    slot1.SetActive(true);
                    updateUI();
                    return;
                }
                if (tool2 == null)
                {
                    tool2 = "boltcutter";
                    slot2.SetActive(true);
                    updateUI();
                    return;
                }
                tool2 = tool1;
                tool1 = "boltcutter";
                updateUI();
                return;
            }
        }
        if (s == "WIRECUTTER")
        {
            if (tool1 != "wirecutter" && tool2 != "wirecutter")
            {
                if (tool1 == null)
                {
                    tool1 = "wirecutter";
                    slot1.SetActive(true);
                    updateUI();
                    return;
                }
                if (tool2 == null)
                {
                    tool2 = "wirecutter";
                    slot2.SetActive(true);
                    updateUI();
                    return;
                }
                tool2 = tool1;
                tool1 = "wirecutter";
                updateUI();
                return;
            }
        }
    }

    private void updateUI()
    {
        if (tool1 == "crowbar")
        {
            slot1.GetComponent<Image>().sprite = crowbar;
        }
        if (tool2 == "crowbar")
        {
            slot2.GetComponent<Image>().sprite = crowbar;
        }
        if (tool1 == "wirecutter")
        {
            slot1.GetComponent<Image>().sprite = wirecutter;
        }
        if (tool2 == "wirecutter")
        {
            slot2.GetComponent<Image>().sprite = wirecutter;
        }
        if (tool1 == "boltcutter")
        {
            slot1.GetComponent<Image>().sprite = boltcutter;
        }
        if (tool2 == "boltcutter")
        {
            slot2.GetComponent<Image>().sprite = boltcutter;
        }

        playerController.updateItems(tool1, 0);
        playerController.updateItems(tool2, 1);

        string[] items = playerController.getItems();
        Debug.Log("Debug test player controller items: 1: " + items[0] + " 2: " + items[1]);

    }

    private void debugPrint()
    {
        Debug.Log("tool1: " + tool1 + " tool2: " + tool2);
    }

    
}
