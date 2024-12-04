using Photon.Realtime;
using UnityEngine.AI;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    private float updateInterval = 0.5f; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("UpdatePath", 0f, updateInterval);
    }

    void UpdatePath()
    {
        if (target != null && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
    }
}
