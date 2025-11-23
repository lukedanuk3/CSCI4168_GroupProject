using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public DaniellePlayerControl playerController;
    private void OnTriggerEnter(Collider other)
    {
        playerController.takeDamage();
    }
}
