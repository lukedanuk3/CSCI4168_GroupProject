using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public enum buttonOptions
    {
        quit,
        HUB,
        controls,
        credits,
        mainMenu
    }

    public buttonOptions option;

    public GameObject titleSection;
    public GameObject controlsSection;
    public GameObject creditsSection;

    public void OnButtonClick()
    {
        if (option == buttonOptions.quit)
        {
            // note: this code will be ignored in unity's editor mode, works in standalone game
            Debug.Log("This will quit the game.");
            Application.Quit();
        }
        if (option == buttonOptions.HUB)
        {
            SceneManager.LoadScene("HUB");
            Debug.Log("This will load the first level.");
        }
        if (option == buttonOptions.controls)
        {
            titleSection.SetActive(false);
            controlsSection.SetActive(true);
        }
        if (option == buttonOptions.credits)
        {
            titleSection.SetActive(false);
            creditsSection.SetActive(true);
        }
        if (option == buttonOptions.mainMenu)
        {
            controlsSection.SetActive(false);
            creditsSection.SetActive(false);
            titleSection.SetActive(true);
        }
    }
}