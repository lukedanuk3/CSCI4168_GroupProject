using UnityEngine;

public class tempUpdateCounter : MonoBehaviour
{
    public UIManager uiManager;
    private void OnTriggerEnter(Collider other)
    {
        uiManager.increaseCounter();
    }
}
