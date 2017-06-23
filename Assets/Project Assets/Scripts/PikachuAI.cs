using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PikachuAI : MonoBehaviour
{
    public AudioClip[] audioClip;
    AudioSource audioSource;

    UnityEngine.AI.NavMeshAgent agent;

    float travelTime;
    float audioTime;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.autoBraking = false;
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
    }

    void PlayAudio()
    {
        if (audioSource.isPlaying)
        {
            return;
        }

        audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];
        audioSource.Play();
        audioTime = Time.time;
    }

    void Update()
    {
        if (agent.remainingDistance < 0.5f || Time.time - travelTime > 8)
        {
            GotoNextDestination();
        }

        if (Random.Range(0,10) == 0 && Time.time - audioTime > 8)
        {
            PlayAudio();
        }
    }
}