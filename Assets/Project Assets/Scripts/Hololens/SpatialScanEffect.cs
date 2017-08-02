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
    float elapsedTime, elapsedRatio, updateRatio;

    void Start()
    {
        SetCenter();

        mat.SetColor("_PulseColor", startPulseColor);
        mat.SetColor("_WireframeColor", startFrameColor);
        mat.SetFloat("_Radius", radius);

        mat.SetColor("_PulseColor2", startPulseColor);
        mat.SetColor("_WireframeColor2", startFrameColor);
        mat.SetFloat("_Radius2", radius);

        updateRatio = 1 / timeBetweenUpdates;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        elapsedRatio = elapsedTime < timeBetweenUpdates ? elapsedTime * updateRatio : 1;

        radius = elapsedTime;
        radius2 += Time.deltaTime;

        mat.SetFloat("_Radius", radius);
        mat.SetFloat("_Radius2", radius2);

        pulseColor = Color.Lerp(startPulseColor, Color.black, elapsedRatio);
        frameColor = Color.Lerp(startFrameColor, Color.black, elapsedRatio);
        mat.SetColor("_PulseColor2", pulseColor);
        mat.SetColor("_WireframeColor2", frameColor);

        if (elapsedRatio == 1)
        {
            SetCenter();

            elapsedTime = 0;
        }
    }

    void SetCenter()
    {
        radius2 = radius;
        mat.SetVector("_Center2", mat.GetVector("_Center"));
        mat.SetFloat("_Radius2", radius2);
        mat.SetColor("_PulseColor2", startPulseColor);
        mat.SetColor("_WireframeColor2", startFrameColor);

        radius = 0;
        mat.SetFloat("_Radius", radius);
        mat.SetVector("_Center", center.transform.position);
    }
}
