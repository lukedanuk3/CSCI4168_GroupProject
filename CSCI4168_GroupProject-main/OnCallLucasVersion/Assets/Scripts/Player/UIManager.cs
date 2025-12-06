using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image health1;
    public Image health2;
    public Image health3;

    public Image tool1;
    public Image tool2;

    public GameObject gameplayUI;
    public GameObject selectionUI;
    public GameObject gameOverUI;
    public GameObject victoryUI;

    public TMP_Text lifeText;

    public void UpdateHealth(int health)
    {
        //Debug.Log("updating health");
        //Debug.Log(health);
        if (health == 3)
        {
            Debug.Log("enabling");
            health1.gameObject.SetActive(false);
            health2.gameObject.SetActive(false);
            health3.gameObject.SetActive(false);
        }
        if (health == 2)
        {
            Debug.Log("enabling");
            health1.gameObject.SetActive(true);
            health2.gameObject.SetActive(false);
            health3.gameObject.SetActive(false);
        }
        if (health == 1)
        {
            health1.gameObject.SetActive(true);
            health2.gameObject.SetActive(true);
            health3.gameObject.SetActive(false);
        }
        if (health == 0)
        {
            health1.gameObject.SetActive(true);
            health2.gameObject.SetActive(true);
            health3.gameObject.SetActive(true);
        }
    }

    public void tool1Active()
    {
        Debug.Log("UI MANAGER SLOT 1 HAS: " + tool1.name);
        tool1.GetComponent<Outline>().enabled = true;
        tool2.GetComponent<Outline>().enabled = false;
    }

    public void tool2Active()
    {
        tool1.GetComponent<Outline>().enabled = false;
        tool2.GetComponent<Outline>().enabled = true;
    }

    public Image GetTool1(){
        return tool1;
    }

    public Image GetTool2(){
        return tool2;
    }

    public void setGameplayUIActive()
    {
        gameplayUI.SetActive(true);
    }
    public void setGameplayUIInactive()
    {
        gameplayUI.SetActive(false);
    }

    public void setSelectionUIActive()
    {
        selectionUI.SetActive(true);
    }

    public void setSelectionUIInactive()
    {
        selectionUI.SetActive(false);
    }
    public void activateGameOver()
    {
        // disable gameplay UI
        setGameplayUIInactive();
        Cursor.lockState = CursorLockMode.None;

        // enable game over screen
        gameOverUI.SetActive(true);
    }

    public void activateVictory(){
        setGameplayUIInactive();
        setSelectionUIInactive();
        Cursor.lockState = CursorLockMode.None;
        victoryUI.SetActive(true);
    }
    public void increaseCounter()
    {
        int curr = int.Parse(lifeText.text);
        //Debug.Log(curr);
        int newCurr = curr + 1;
        //Debug.Log(newCurr);
        if (newCurr > 5)
        {
            newCurr = 5;
        }
        lifeText.text = newCurr + "";
    }

}