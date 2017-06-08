using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConstantRotate : MonoBehaviour {

    public float X_axis;
    public float Y_axis;
    public float Z_axis;
    public float rotateSpeed = 10f;

    void Update () {
        transform.Rotate(new Vector3(X_axis, Y_axis, Z_axis), rotateSpeed * Time.deltaTime);
    }
}
