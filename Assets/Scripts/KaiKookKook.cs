using UnityEngine;
using System.Collections.Generic;

public class KaiKookKook : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    private UnityEngine.AI.NavMeshAgent agent;
    private int currentWaypointIndex;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        currentWaypointIndex = 0;
        SetNextWaypoint();
    }

    private void Update()
    {
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex++;
    }
}
