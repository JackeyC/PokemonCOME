using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class PikachuAI : MonoBehaviour
{
    public AudioClip[] audioClip;
    public Color targetLineColor = Color.green;

    AudioSource audioSource;
    NavMeshAgent agent;
    Animator anim;

    float travelTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        GotoNextDestination();
        PlayAudio();
    }

    void GotoNextDestination()
    {
        Vector3 destination = 10 * Random.insideUnitCircle;
        destination += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(destination, out navMeshHit, 10, 1);
        agent.SetDestination(navMeshHit.position);
        travelTime = Time.time;
        agent.updatePosition = false;
        //agent.updateRotation = true;
        anim.SetInteger("State", 6);
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
        agent.velocity = anim.deltaPosition / Time.deltaTime;
        //anim.SetFloat("Speed", agent.desiredVelocity.magnitude);
        agent.nextPosition = transform.position;
        if (agent.remainingDistance < 0.5f || Time.time - travelTime > 8)
        {
            GotoNextDestination();
        }

        Debug.DrawLine(transform.position, agent.destination, targetLineColor);

        if (Random.Range(0,1000) == 0)
        {
            PlayAudio();
        }
    }
}