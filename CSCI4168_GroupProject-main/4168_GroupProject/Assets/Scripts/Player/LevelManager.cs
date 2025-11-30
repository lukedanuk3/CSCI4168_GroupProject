using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{

    private string levelSelection;
    public PlayerControl player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SelectLevel(string level){
        player.updateLevelChoice(level);
    }

    public void EndGame(){
        Application.Quit();
    }
}
