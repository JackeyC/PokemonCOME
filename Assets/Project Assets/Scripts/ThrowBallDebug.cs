using System.Collections;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ThrowBallDebug : MonoBehaviour, IInputClickHandler
{

    public float thrust = 5;
    public float cooldownPeriod = 1;
    public float hitTime = 0.5f;
    public float velocity = 2;
    public float velocity2 = 2;
    public Rigidbody pokeballPrefab;
    public Transform target;
    public Vector3 ballPositionOffset;
    public bool continuousThrow;
    public float timeScale = 1;

    public bool secondPath;

    Vector3 targetDistance;
    float upSpeed;
    float forwardSpeed;

    float lastThrowTime;
    float lastThrowTime2;

    public void Start()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime *= timeScale;
    }

    public void Update()
    {
        if (continuousThrow)
        {
            OnThrowBall();
            if (secondPath)
            {
                OnThrowBall2();
            }
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        OnThrowBall();
    }

    void OnThrowBall()
    {
        if ((lastThrowTime + cooldownPeriod) < Time.time)
        {
            lastThrowTime = Time.time;

            Vector3 ballPosition = Camera.main.transform.position + Camera.main.transform.forward * ballPositionOffset.x + Camera.main.transform.up * ballPositionOffset.y + Camera.main.transform.right * ballPositionOffset.z;

            // Calculate throwing force
            float displacementY = target.transform.position.y - ballPosition.y;
            Vector3 displacementXZ = new Vector3(target.transform.position.x - ballPosition.x, 0, target.transform.position.z - ballPosition.z);

            hitTime = Vector3.Magnitude(displacementXZ) / velocity;
            Vector3 velocityY = Vector3.up * displacementY / hitTime - 0.5f * Physics.gravity * hitTime;
            Vector3 velocityXZ = displacementXZ / hitTime;

            // Spawn Pokeball
            Rigidbody pokeballInstance;
            pokeballInstance = Instantiate(pokeballPrefab, ballPosition, Camera.main.transform.rotation);

            pokeballInstance.AddForce(pokeballInstance.mass * (velocityXZ + velocityY), ForceMode.Impulse);
        }
    }
    void OnThrowBall2()
    {
        if ((lastThrowTime2 + cooldownPeriod) < Time.time)
        {
            lastThrowTime2 = Time.time;

            Vector3 ballPosition = Camera.main.transform.position + Camera.main.transform.forward * ballPositionOffset.x + Camera.main.transform.up * ballPositionOffset.y + Camera.main.transform.right * ballPositionOffset.z;

            // Calculate throwing force
            //float displacementY = target.transform.position.y - ballPosition.y;
            Vector3 displacementXZ = new Vector3(target.transform.position.x - ballPosition.x, 0, target.transform.position.z - ballPosition.z);

            hitTime = Vector3.Magnitude(displacementXZ) / velocity;
            //Vector3 velocityY = Vector3.up * displacementY / hitTime - 0.5f * Physics.gravity * hitTime;
            Vector3 velocityY = Vector3.up * velocity2;
            Vector3 velocityXZ = displacementXZ / hitTime;
            //velocityY = Vector3.ClampMagnitude(velocityY, velocityXZ.magnitude);

            // Spawn Pokeball
            Rigidbody pokeballInstance;
            pokeballInstance = Instantiate(pokeballPrefab, ballPosition, Camera.main.transform.rotation);

            pokeballInstance.AddForce(pokeballInstance.mass * (velocityXZ + velocityY), ForceMode.Impulse);
        }
    }
}
