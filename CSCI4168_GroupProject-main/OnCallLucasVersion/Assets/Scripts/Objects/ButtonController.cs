
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public enum buttonOptions
    {
        quit,
        level1
    }

    public buttonOptions option;

    public void OnButtonClick()
    {
        if (option == buttonOptions.quit)
        {
            // note: this code will be ignored in unity's editor mode, works in standalone game
            Debug.Log("This will quit the game.");
            Application.Quit();
        }
        if (option == buttonOptions.level1)
        {
            // load the first scene in the build index (TODO: decomment when build if further along)
            // SceneManager.LoadScene(1);
            Debug.Log("This will load the first level.");
        }
    }
}
