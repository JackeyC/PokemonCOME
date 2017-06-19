using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour {
    
    public Transform[] points;
    private int destPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    void Start () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.autoBraking = false;

        //GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
        {
            return;
        }
        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }
	
	void LateUpdate () {
        //agent.SetDestination(target.position);
        if (agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
	}
}
