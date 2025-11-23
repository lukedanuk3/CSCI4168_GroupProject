using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SimpleEnemyAI : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float stoppingDistance = 1.5f;   // how close it gets before "attacking"
    public float damagePerSecond = 10f;

    Transform target;        // player
    PlayerHealth playerHealth;

    void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }

        // Make sure our collider is set as trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void Update()
    {
        if (target == null) return;

        // Move toward player on XZ plane
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > stoppingDistance)
        {
            direction.Normalize();
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.forward = direction;  // face player
        }
        else
        {
            // in attack range – damage over time
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
