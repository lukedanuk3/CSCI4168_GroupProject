using UnityEngine;

public class tempGameplayUIswap : MonoBehaviour
{
    public bool active;
    public UIManager uiManager;

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            uiManager.setGameplayUIActive();
        }
        if (!active)
        {
            uiManager.setGameplayUIInactive();
        }
    }
}
