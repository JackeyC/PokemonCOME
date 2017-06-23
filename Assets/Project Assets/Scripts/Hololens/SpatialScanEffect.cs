using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialScanEffect : MonoBehaviour {

    public Material mat;

    float radius;

	void Update () {

        radius += 0.5f * Time.deltaTime;
        radius %= 10;
        mat.SetFloat("_Radius", radius);
	}
}
