using UnityEngine;

public class DaniellePlayerControl : MonoBehaviour
{
    public int health = 3;
    private string[] items = new string[2]; // two items held 

    // the ui manager paints the UI 
    public UIManager uiManager;

    public void takeDamage()
    {
        health--;
        uiManager.UpdateHealth(health);
    }

    public void heal()
    {
        health++;
        uiManager.UpdateHealth(health);
    }

    public int returnHealth()
    {
        return health;
    }

    public bool updateItems(string s, int index)
    {
        if (index < 0 || index > 1)
        {
            return false;
        }
        items[index] = s;
        return true;
    }

    public string[] getItems()
    {
        return items;
    }
}
