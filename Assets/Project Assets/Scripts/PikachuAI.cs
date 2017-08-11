using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PikachuAI : MonoBehaviour
{
    public bool isWild = true;
    public AudioClip meleeSFX, SpecialSFX, hitSFX;
    public AudioClip[] audioClip;
    public Color targetLineColor = Color.green;
    public Transform moveTarget;
    public GameObject meleeVFX;
    public GameObject specialVFX;
    public GameObject lightningHitVFX;

    NavMeshAgent agent;
    Animator anim;
    AudioSource audioSource;

    float travelTime;

    bool isFollowing = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        GotoNextDestination();
        PlayAudio();
    }

    void Update()
    {
        //agent.velocity = anim.deltaPosition / Time.deltaTime;
        if (!anim.GetBool("Melee") && !anim.GetBool("Special"))
        {
            anim.SetFloat("speedRatio", agent.desiredVelocity.magnitude);
        }
        transform.position = new Vector3(transform.position.x, agent.nextPosition.y, transform.position.z);
        agent.nextPosition = transform.position;
        if (isWild)
        {
            if (agent.remainingDistance < 0.5f || Time.time - travelTime > 8)
            {
                GotoNextDestination();
            }
        }
        else if (isFollowing)
        {
            if ((agent.destination - Camera.main.transform.position).magnitude > 2)
            {
                agent.SetDestination(Camera.main.transform.position);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position, Vector3.up), 100 * Time.deltaTime);
            }
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

        if (Random.Range(0, 1000) == 0)
        {
            PlayAudio();
        }
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

        audioSource.clip = audioClip[Random.Range(1, audioClip.Length)];
        audioSource.Play();
    }

    void CommandReceived()
    {
        if (audioSource.isPlaying)
        {
            return;
        }

        audioSource.clip = audioClip[0];
        audioSource.Play();
    }

    public void FollowMe()
    {
        CommandReceived();
        Debug.Log("Follow Me");
        isFollowing = true;
    }

    public void MoveHere()
    {
        CommandReceived();
        Debug.Log("Move Here");
        agent.SetDestination(moveTarget.position);
        isFollowing = false;
    }

    public void MeleeAttack()
    {
        CommandReceived();
        Debug.Log("Iron Tail");
        anim.SetFloat("speedRatio", 0);
        anim.SetTrigger("Melee");
    }

    public void SpecialAttack()
    {
        Debug.Log("Thundershock");
        anim.SetFloat("speedRatio", 0);
        anim.SetTrigger("Special");

        audioSource.clip = SpecialSFX;
        audioSource.Play();
    }

    public void Melee_VFX()
    {
        var VFXInstance = Instantiate(meleeVFX, transform);
        Destroy(VFXInstance, 3);
    }

    public void Special_VFX()
    {
        var VFXInstance = Instantiate(specialVFX, transform);
        Destroy(VFXInstance, 3);
    }

    public void Lightning_Hit_VFX()
    {
        var VFXInstance = Instantiate(lightningHitVFX, transform);
        Destroy(VFXInstance, 3);
    }
}