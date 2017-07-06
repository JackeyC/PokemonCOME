using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PikachuAI : MonoBehaviour
{
    public AudioClip[] audioClip;
    public Color targetLineColor = Color.green;

    NavMeshAgent agent;
    Animator anim;
    AudioSource audioSource;

    float travelTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        GotoNextDestination();
        PlayAudio();
    }

    void GotoNextDestination()
    {
        Vector3 destination = 10 * Random.onUnitSphere;
        destination += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(destination, out navMeshHit, 10, 1);
        agent.SetDestination(navMeshHit.position);
        travelTime = Time.time;
        agent.updatePosition = false;
    }

    void PlayAudio()
    {
        if (audioSource.isPlaying)
        {
            return;
        }

        audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];
        audioSource.Play();
    }

    void Update()
    {
        //agent.velocity = anim.deltaPosition / Time.deltaTime;
        anim.SetFloat("speedRatio", agent.desiredVelocity.magnitude);
        transform.position = new Vector3(transform.position.x, agent.nextPosition.y, transform.position.z);
        agent.nextPosition = transform.position;
        if (agent.remainingDistance < 0.5f || Time.time - travelTime > 8)
        {
            GotoNextDestination();
        }

        Vector3 targetDistance = agent.destination - transform.position;
        float angle = Vector3.Angle(transform.forward, targetDistance);

        if (angle > 90)
        {
            if (agent.speed > 0.2f)
            {
                agent.speed = Mathf.Lerp(agent.speed, 0.2f, Time.deltaTime);
            }
            else
            {
                agent.speed = 0.2f;
            }
        }
        else if (angle > 40)
        {
            if (agent.speed > 0.5f)
            {
                agent.speed = Mathf.Lerp(agent.speed, 0.5f, Time.deltaTime);
            }
            else
            {
                agent.speed = 0.5f;
            }
        }
        else if (agent.speed < 1)
        {
            agent.speed = Mathf.Lerp(agent.speed, 1, Time.deltaTime);
        }
        else
        {
            agent.speed = 1;
        }

        Debug.DrawLine(transform.position, agent.destination, targetLineColor);

        if (Random.Range(0,1000) == 0)
        {
            PlayAudio();
        }
    }
}