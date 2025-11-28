using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public enum buttonOptions
    {
        quit,
        reload
    }

    public buttonOptions choice;

    public void OnButtonClick()
    {
        if (choice == buttonOptions.quit)
        {
            // note: this code will be ignored in unity's editor mode, works in standalone game
            Debug.Log("This will quit the game.");
            Application.Quit();
        }
        if (choice == buttonOptions.reload)
        {
            //reload the current scene
            Debug.Log("This will reload the current level.");
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
    }
}
