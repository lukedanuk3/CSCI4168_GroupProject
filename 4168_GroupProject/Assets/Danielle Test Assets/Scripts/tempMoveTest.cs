using UnityEngine;
using UnityEngine.AI;

public class tempMoveTest : MonoBehaviour
{
    [SerializeField] public GameObject goal; 
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null && goal != null)
        {
            agent.SetDestination(goal.transform.position);
        }
    }
}
