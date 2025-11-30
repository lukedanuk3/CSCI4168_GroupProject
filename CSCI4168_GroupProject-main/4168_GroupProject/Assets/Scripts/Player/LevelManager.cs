using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private string levelSelection;
    public PlayerControl player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void SelectLevel(string level){
        player.updateLevelChoice(level);
    }
}
