using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int health = 3;
    private string[] items = new string[2]; // two items held 

    // the ui manager paints the UI 
    public UIManager uiManager;

    float timer = 0;
    public float healTime = 10;

    public void takeDamage()
    {
        health--;
        if (health < 0)
        {
            uiManager.activateGameOver();
        }
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

    private void Update()
    {
        if (health < 3)
        {
            timer = timer + Time.deltaTime;
            if (timer > healTime)
            {
                heal();
                //Debug.Log("Healed! Current health: " + health);
                timer = 0;
            }
        }
    }
}
