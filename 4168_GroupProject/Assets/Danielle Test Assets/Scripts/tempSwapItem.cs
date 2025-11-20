using UnityEngine;

public class tempSwapItem : MonoBehaviour
{
    public UIManager uiManager;
    public bool tool1;

    private void OnTriggerEnter(Collider other)
    {
        if (tool1)
        {
            uiManager.tool1Active();
        } 
        else
        {
            uiManager.tool2Active();
        }
    }
}
