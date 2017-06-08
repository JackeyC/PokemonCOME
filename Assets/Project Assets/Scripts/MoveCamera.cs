using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public bool lockCursor = true;
    public float speed = 2.0f;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float yawRad;
    private float pitch = 0.0f;

    bool noClick = true;

    void Start() {
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update() {
        if (Input.GetKey(KeyCode.W)) {
            //transform.position += Camera.main.transform.forward * speed * Time.deltaTime;
            transform.position += Vector3.forward * Mathf.Cos(yawRad) * speed * Time.deltaTime;
            transform.position += Vector3.right * Mathf.Sin(yawRad) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            //transform.position -= Camera.main.transform.right * speed * Time.deltaTime;
            transform.position += Vector3.forward * Mathf.Sin(yawRad) * speed * Time.deltaTime;
            transform.position -= Vector3.right * Mathf.Cos(yawRad) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            //transform.position -= Camera.main.transform.forward * speed * Time.deltaTime;
            transform.position -= Vector3.forward * Mathf.Cos(yawRad) * speed * Time.deltaTime;
            transform.position -= Vector3.right * Mathf.Sin(yawRad) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            //transform.position += Camera.main.transform.right * speed * Time.deltaTime;
            transform.position -= Vector3.forward * Mathf.Sin(yawRad) * speed * Time.deltaTime;
            transform.position += Vector3.right * Mathf.Cos(yawRad) * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space)) {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            transform.localPosition -= Vector3.up * speed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Tab)) {
            noClick = !noClick;
        }
        if (Input.GetMouseButton (0) || noClick) {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -90, 90);
            yawRad = yaw * Mathf.Deg2Rad;

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }
}
