using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PikachuAI : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;

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
    }

    void LateUpdate()
    {
        if (agent.remainingDistance < 0.5f)
        {
            GotoNextDestination();
        }
    }
}