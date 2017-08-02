﻿using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public class ThrowBall : MonoBehaviour, IInputClickHandler
{
    public float cooldownPeriod = 1;
    public float velocity = 2;
    public Rigidbody pokeballPrefab;
    public Transform target;
    public Vector3 ballPositionOffset;

    private IInputSource currentInputSource = null;
    private uint currentInputSourceId;
    Vector3 handPosition;

    Vector3 targetDistance;
    float upSpeed;
    float forwardSpeed;

    float lastThrowTime;
    public Text debugText;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnThrowBall();
    }

    void OnThrowBall()
    {
        if ((lastThrowTime + cooldownPeriod) < Time.time )
        {
            lastThrowTime = Time.time;

            Vector3 ballPosition = Camera.main.transform.position + Camera.main.transform.forward * ballPositionOffset.x + Camera.main.transform.up * ballPositionOffset.y + Camera.main.transform.right * ballPositionOffset.z;

            // Calculate throwing force
            float displacementY = target.transform.position.y - ballPosition.y;
            Vector3 displacementXZ = new Vector3(target.transform.position.x - ballPosition.x, 0, target.transform.position.z - ballPosition.z);

            float hitTime;

            hitTime = Vector3.Magnitude(displacementXZ) / velocity;
            Vector3 velocityY = Vector3.up * displacementY / hitTime - 0.5f * Physics.gravity * hitTime;
            Vector3 velocityXZ = displacementXZ / hitTime;

            // Spawn Pokeball
            Rigidbody pokeballInstance = Instantiate(pokeballPrefab, ballPosition, Camera.main.transform.rotation);
            pokeballInstance.isKinematic = false;
            pokeballInstance.AddForce(pokeballInstance.mass * (velocityXZ + velocityY), ForceMode.Impulse);
        }
    }
}
