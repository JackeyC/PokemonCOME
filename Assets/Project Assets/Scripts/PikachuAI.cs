using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PikachuAI : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;

    float travelTime;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.autoBraking = false;

        GotoNextDestination();
    }

    void GotoNextDestination()
    {
        Vector3 destination = 10 * Random.insideUnitCircle;
        destination += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(destination, out navMeshHit, 10, 1);
        agent.SetDestination(navMeshHit.position);
        travelTime = Time.time;
    }

    void LateUpdate()
    {
        if (agent.remainingDistance < 0.5f || Time.time - travelTime > 8)
        {
            GotoNextDestination();
        }
    }
}