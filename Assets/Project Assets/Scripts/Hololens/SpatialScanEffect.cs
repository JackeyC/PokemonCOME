using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialScanEffect : MonoBehaviour
{
    public float timeBetweenUpdates = 3.5f;
    public GameObject center;
    public Material mat;

    float radius;
    float updateTime;

    void Start()
    {
        SetCenter();
    }

    void Update()
    {
        if ((Time.time - updateTime) >= timeBetweenUpdates)
        {
            SetCenter();
        }

        radius += Time.deltaTime;
        radius %= 10;
        mat.SetFloat("_Radius", radius);
    }

    void SetCenter()
    {
        mat.SetVector("_Center", center.transform.position);
        radius = 0;
        updateTime = Time.time;
    }
}
