using UnityEngine;

public class tempSelectUIswap : MonoBehaviour
{
    public bool active;
    public UIManager uiManager;

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            uiManager.setSelectionUIActive();
        }
        if (!active)
        {
            uiManager.setSelectionUIInactive();
        }
    }
}
