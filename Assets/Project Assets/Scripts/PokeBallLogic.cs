using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallLogic : MonoBehaviour {

    public float duration = 2;
    public float despawnTime = 10;
    
    Animator anim;

    AudioSource audioSource;
    public AudioClip bounce, capture, struggle, caught;

    public GameObject caughtVFX;

    public Material material1;
    public Material material2;

    int range;
    Material[] rend = new Material[5];

    bool capturing = false;
    bool empty = true;
    Rigidbody rb;
    int collisionCount;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        Destroy(gameObject, despawnTime);

        //range = GetComponent<Renderer>().materials.Length;
        
        //for (int i = 0; i < range; i++)
        //{
        //    rend[i] = GetComponent<Renderer>().materials[i];
        //    rend[i] = material1;
        //}
    }

    void Update()
    {
        if (capturing)
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.up, 5 * Time.deltaTime);
        }
    //    float lerp = Mathf.PingPong(Time.time, duration) / duration;
    //    for (int i = 0; i < range; i++)
    //    {
    //        rend[i].Lerp(material1, material2, lerp);
    //    }
    }

    void OnCollisionEnter(Collision pokemon)
    {
        audioSource.clip = bounce;
        audioSource.Play();
        if (empty)
        {
            collisionCount++;
            if (collisionCount < 3)
            {
                if (pokemon.gameObject.tag == "Pokemon")
                {
                    anim = GetComponentInChildren<Animator>();
                    anim.SetInteger("State", 1);
                    Destroy(pokemon.gameObject, 0.8f);
                    rb = GetComponent<Rigidbody>();
                    rb.AddForce(Vector3.up, ForceMode.Impulse);
                    rb.freezeRotation = true;
                    audioSource.clip = capture;
                    audioSource.Play();

                    rb.mass = 5;
                    rb.isKinematic = true;
                    capturing = true;
                    empty = false;
                }
            }
            else
            {
                empty = false;
            }
        }
    }

    public void Fall()
    {
        capturing = false;
        rb.isKinematic = false;
    }

    public void Play_Struggle_SFX()
    {
        audioSource.clip = struggle;
        audioSource.Play();
    }

    public void Pokemon_Caught_VFX()
    {
        Instantiate(caughtVFX, transform);
    }

    public void Pokemon_Caught()
    {
        anim.SetInteger("State", 0);
        //audioSource.clip = caught;
        //audioSource.Play();
        
    }
}
