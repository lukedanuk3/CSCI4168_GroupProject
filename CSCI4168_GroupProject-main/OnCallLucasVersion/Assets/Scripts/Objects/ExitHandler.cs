using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    private bool isLocked = true;
    public UIManager uiManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Interact(){
        if(!isLocked){
            Cursor.lockState = CursorLockMode.None;
            uiManager.activateVictory();
        }
    }
    public void UnlockDoor(){
        isLocked = !isLocked;
    }

    public bool isDoorLocked(){
        return isLocked;    
    }
}
