using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallLogic : MonoBehaviour {

    public Material material1;
    public Material material2;
    public float duration = 2;

    int range;
    Material[] rend = new Material[5];


    void Start()
    {
        range = GetComponent<Renderer>().materials.Length;
        
        for (int i = 0; i < range; i++)
        {
            rend[i] = GetComponent<Renderer>().materials[i];
            rend[i] = material1;
        }
    }
    void Update()
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        for (int i = 0; i < range; i++)
        {
            rend[i].Lerp(material1, material2, lerp);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Pokemon")
        {
            Destroy(col.gameObject, 2);
        }
    }
}
