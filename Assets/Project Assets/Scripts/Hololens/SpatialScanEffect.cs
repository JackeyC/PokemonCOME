using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace HoloToolkit.Unity.SpatialMapping
//{
    public class SpatialScanEffect : MonoBehaviour
    {
    //SpatialMappingObserver spatialMappingObserver;
        float timeBetweenUpdates = 3.5f;

        public GameObject center;
        public Material mat;

        float radius;
        float updateTime;

        //void Start()
        //{
        //    spatialMappingObserver = GetComponent<SpatialMappingObserver>();
        //}

        void Update()
        {
            //if ((Time.unscaledTime - updateTime) >= timeBetweenUpdates)
            //{
            //    mat.SetVector("_Center", center.transform.position);
            //    radius = 0;
            //    updateTime = Time.unscaledTime;
            //}

            radius += Time.deltaTime;
            radius %= 10;
            mat.SetFloat("_Radius", radius);
        }
    }
//}
