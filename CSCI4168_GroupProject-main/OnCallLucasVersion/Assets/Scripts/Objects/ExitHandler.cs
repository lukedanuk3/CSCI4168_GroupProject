using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    private bool isLocked = true;
    public UIManager uiManager;
    public GameObject light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light.SetActive(false);
    }

    // Update is called once per frame
    public void Interact(){
        if(!isLocked){
            uiManager.activateVictory();
        }
    }
    public void UnlockDoor(){
        isLocked = !isLocked;
        light.SetActive(true);
    }

    public bool isDoorLocked(){
        return isLocked;    
    }
}
