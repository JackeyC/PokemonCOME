using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PidgeyAI : MonoBehaviour
{
    public AudioClip[] audioClip;
    public Color targetLineColor = new Color(0, 1, 0);

    AudioSource audioSource;
    NavMeshAgent agent;

    float endAltitude;
    float travelTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
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
        if (agent.remainingDistance < 0.5f || Time.time - travelTime > 8)
        {
            GotoNextDestination();
        }
        if (agent.baseOffset == endAltitude)
        {
            endAltitude = Random.Range(0.5f, 2);
        }
        agent.baseOffset = Mathf.MoveTowards(agent.baseOffset, endAltitude, Time.deltaTime * 0.3f);

        Debug.DrawLine(transform.position, agent.destination, targetLineColor);

        if (Random.Range(0, 1000) == 0)
        {
            PlayAudio();
        }
    }
}