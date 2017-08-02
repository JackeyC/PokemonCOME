using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialScanEffect : MonoBehaviour
{
    public float timeBetweenUpdates = 1.5f;
    public GameObject center;
    public Material mat;
    public Color startPulseColor, startFrameColor;
    
    float radius, radius2;
    Color pulseColor, frameColor;
    float updateTime, elapsedRatio;

    void Start()
    {
        SetCenter();

        mat.SetColor("_PulseColor", startPulseColor);
        mat.SetColor("_WireframeColor", startFrameColor);
        mat.SetFloat("_Radius", radius);

        mat.SetColor("_PulseColor2", startPulseColor);
        mat.SetColor("_WireframeColor2", startFrameColor);
        mat.SetFloat("_Radius2", radius);
    }

    void Update()
    {
        elapsedRatio = (Time.time - updateTime) / timeBetweenUpdates;
        if (elapsedRatio >= 1)
        {
            SetCenter();

            pulseColor = startPulseColor;
            frameColor = startFrameColor;
            mat.SetColor("_PulseColor", pulseColor);
            mat.SetColor("_WireframeColor", frameColor);

            updateTime = Time.time;
        }

        //float radiusRatio = radius2 % 10;
        //if (radiusRatio > 0.8f)
        //{
        //    var lerpRatio = (radiusRatio - 0.8f) * 5;
        //    pulseColor = Color.Lerp(startPulseColor, Color.black, lerpRatio);
        //    frameColor = Color.Lerp(startFrameColor, Color.black, lerpRatio);
        //    mat.SetColor("_PulseColor", pulseColor);
        //    mat.SetColor("_WireframeColor", frameColor);
        //}

        radius += Time.deltaTime;
        radius2 += Time.deltaTime;
        mat.SetFloat("_Radius", radius);
        mat.SetFloat("_Radius2", radius2);
    }

    void SetCenter()
    {
        mat.SetVector("_Center2", mat.GetVector("_Center"));
        mat.SetVector("_Center", center.transform.position);
        
        radius2 = radius;
        radius = 0;
    }
}
