using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public PlayerControl playerController;
    private void OnTriggerEnter(Collider other)
    {
        playerController.takeDamage();
    }
}
