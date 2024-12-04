using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }
}
