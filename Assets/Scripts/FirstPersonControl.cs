using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControl : MonoBehaviour {

    public float speed = 2.0f;

    Transform cameraT;
    float pitch;

    void Start() {
        cameraT = Camera.main.transform;
    }

    void Update () {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * speed);
        pitch += Input.GetAxis("Mouse Y") * speed;
        pitch = Mathf.Clamp(pitch, -90, 90);
        cameraT.localEulerAngles = Vector3.left * pitch;

        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition += Vector3.back * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += Vector3.right * speed * Time.deltaTime;
        }
    }
}
