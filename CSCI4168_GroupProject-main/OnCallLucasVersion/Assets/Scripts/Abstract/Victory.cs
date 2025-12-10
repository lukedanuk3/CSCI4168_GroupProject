using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public enum buttonOptions
    {
        quit,
        goBackToHub
    }

    public buttonOptions choice;

    public void OnButtonClick()
    {
        if (choice == buttonOptions.quit)
        {
            // note: this code will be ignored in unity's editor mode, works in standalone game
            PlayerPrefs.SetInt("HasPlayed", 0);
            Debug.Log("This will quit the game.");
            Application.Quit();
        }
        if (choice == buttonOptions.goBackToHub)
        {
            //reload the current scene
            Debug.Log("This will take the user back to the HUB");
            SceneManager.LoadScene("HUB");
        }
    }
}