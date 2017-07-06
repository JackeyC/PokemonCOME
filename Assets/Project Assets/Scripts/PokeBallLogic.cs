using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallLogic : MonoBehaviour {

    public float duration = 2;
    public float despawnTime = 10;
    
    AudioSource audioSource;
    public AudioClip bounce, capture;

    public Material material1;
    public Material material2;

    int range;
    Material[] rend = new Material[5];

    bool empty = true;
    Rigidbody rb;

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
    //void Update()
    //{
    //    float lerp = Mathf.PingPong(Time.time, duration) / duration;
    //    for (int i = 0; i < range; i++)
    //    {
    //        rend[i].Lerp(material1, material2, lerp);
    //    }
    //}

    void OnCollisionEnter(Collision col)
    {
        if (empty)
        {
            audioSource.clip = bounce;
            audioSource.Play();
            if (col.gameObject.tag == "Pokemon")
            {
                Destroy(col.gameObject, 2.5f);
                Destroy(gameObject, 2.5f);
                rb = GetComponent<Rigidbody>();
                rb.AddForce(Vector3.up, ForceMode.Impulse);
                audioSource.clip = capture;
                audioSource.Play();

                empty = false;
            }
        }
    }
}
