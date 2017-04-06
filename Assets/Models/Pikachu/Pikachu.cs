
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pikachu : MonoBehaviour
{

    private Animator anim;
    string moveState;
    private float totalTime = 0;
    bool moving = false;
    //private Vector3 direction;
    //private Vector3 movement;
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 randomPoint;
    private int rand;
    NavMeshHit navHit;
    private Vector3 Pos, Pos2;
    Vector3 destination;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        randomPoint = Random.insideUnitCircle * 30;
        randomPoint += transform.position;
        NavMesh.SamplePosition(randomPoint, out navHit, 30, 1);
        agent.SetDestination(navHit.position);

    }

    void Update()
    {

        totalTime += Time.deltaTime;


        Move();

    }


    void Move()
    {

        //Pos2 = Pos;
        Pos = transform.position;


        if
            //(
          //  (
            ((System.Math.Abs(Pos.x - navHit.position.x) < 0.5f) && (System.Math.Abs(Pos.z - navHit.position.z) < 0.5f))
        //||(System.Math.Abs(Pos2.x - Pos.x) < 0.00001) && (System.Math.Abs(Pos2.z - Pos.z) < 0.00001)) //or old = new
        //&& ((string.Compare(moveState, "Move04") != 0) || ((string.Compare(moveState, "Move05") != 0)))
        //)
        {
            agent.Stop();
            anim.Play("Move05");
            //totalTime = 11;

        }


        if (totalTime > 10)
        {
            /*
            rand = Random.Range((int)1, (int)10);
            if (rand < 4)
            {
                agent.Stop();
                inPlaceAnim();
            }
            else moveAnim();
            */
            moveAnim();
        }

    }
    /*
    void inPlaceAnim()
    {
        //pick state
        moveState = string.Format("Move0{0}", Random.Range((int)4, (int)6));
        //play animation
        anim.Play(moveState);

    }
    */
    void moveAnim()
    {

        //pick state
        moveState = string.Format("Move0{0}", Random.Range((int)2, (int)4));
        //change speed depending on state
        if (string.Compare(moveState, "Move03") == 0)
        {
            agent.speed = 0.3f;
        }
        else agent.speed = 0.8f;


        totalTime = 0;
        randomPoint = Random.insideUnitCircle * 10;
        Vector3 finaldest = new Vector3(randomPoint.x, 0.0f, randomPoint.y);
        randomPoint += transform.position;
        NavMesh.SamplePosition(finaldest, out navHit, 20, 1);
        agent.SetDestination(navHit.position);
        Debug.DrawRay(navHit.position, Vector3.up, Color.blue, 1.0f);

        agent.Resume();

        //transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 8);
        //agent.SetDestination(direction);
        //play animation

        anim.Play(moveState);

    }

    //Vector3 PickRandomPoint()
    //{
    //    return randomPoint;
    //}

}


//Targets

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikachu : MonoBehaviour
{

    //public Transform target;
    public Transform[] points;
    private int destPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    private int rand;
    private string moveState;
    private Animator anim;
    private float time;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = gameObject.GetComponentInChildren<Animator>();
        agent.autoBraking = false;

        GotoNextPoint();
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

    void Update()
    {

        time += Time.deltaTime;

        if (agent.remainingDistance < 0.5f)
        {
            time = 0;
            
            rand = Random.Range((int)4, (int)10);

            if (rand < 3)
            {
                inPlaceAnim();
            }
            else
            {
                moveAnim();


                GotoNextPoint();
            }
            
            
        }

    }


    //picks which animation

    void inPlaceAnim()
    {
        //pick state
        moveState = string.Format("Move0{0}", Random.Range((int)4, (int)6));
        //play animation
        anim.Play(moveState);

    }

    void moveAnim()
    {
        //pick state
        moveState = string.Format("Move0{0}", Random.Range((int)2, (int)4));
        //change speed depending on state
        if (string.Compare(moveState, "Move03") == 0)
        {
            agent.speed = 0.3f;
        }
        else agent.speed = 0.8f;

        //var rotate = Quaternion.LookRotation(direction);
        //rotate.y = 0;
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * 8);
        //agent.SetDestination(direction);
        //play animation

        anim.Play(moveState);

    }
}
*/

