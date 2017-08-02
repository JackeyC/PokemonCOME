using UnityEngine;
using UnityEngine.UI;
using System;

namespace HoloToolkit.Unity.InputModule
{
    public class HandsThrowingSystem : MonoBehaviour, IFocusable, IInputHandler, ISourceStateHandler
    {
        public event Action StartedDragging;
        public event Action StoppedDragging;

        [Tooltip("Scale by which hand movement in z is multipled to move the dragged object.")]
        public float DistanceScale = 2f;

        public Rigidbody throwObjectPrefab;
        public Transform target;

        private Camera mainCamera;
        private bool isDragging;
        private bool isGazed;
        private Vector3 objRefForward;
        private Vector3 objRefUp;
        private float objRefDistance;
        private Quaternion gazeAngularOffset;
        private float handRefDistance;
        private Vector3 objRefGrabPoint;

        private Vector3 draggingPosition;
        private Quaternion draggingRotation;

        private IInputSource currentInputSource = null;
        private uint currentInputSourceId;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void OnDestroy()
        {
            if (isDragging)
            {
                StopDragging();
            }

            if (isGazed)
            {
                OnFocusExit();
            }
        }

        private void Update()
        {
            if (isDragging)
            {
                UpdateDragging();
            }
        }

        /// <summary>
        /// Starts dragging the object.
        /// </summary>
        public void StartDragging()
        {
            if (isDragging)
            {
                return;
            }

            // Add self as a modal input handler, to get all inputs during the manipulation
            InputManager.Instance.PushModalInputHandler(gameObject);

            isDragging = true;
            //GazeCursor.Instance.SetState(GazeCursor.State.Move);
            //GazeCursor.Instance.SetTargetObject(HostTransform);

           // Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;
            currentInputSource.TryGetPosition(currentInputSourceId, out startHandPosition);
            handPosition = startHandPosition;
            
            // Spawn Pokeball
            pokeballInstance = Instantiate(throwObjectPrefab, startHandPosition, Camera.main.transform.rotation);
            pokeballInstance.GetComponent<Collider>().enabled = false;
            aiming = true;

            //draggingPosition = gazeHitPosition;

            StartedDragging.RaiseEvent();
        }

        /// <summary>
        /// Gets the pivot position for the hand, which is approximated to the base of the neck.
        /// </summary>
        /// <returns>Pivot position for the hand.</returns>
        private Vector3 GetHandPivotPosition()
        {
            Vector3 pivot = Camera.main.transform.position + new Vector3(0, -0.2f, 0) - Camera.main.transform.forward * 0.2f; // a bit lower and behind
            return pivot;
        }

        /// <summary>
        /// Enables or disables dragging.
        /// </summary>
        /// <param name="isEnabled">Indicates whether dragging shoudl be enabled or disabled.</param>
        public void SetDragging(bool isEnabled)
        {
            if (isDragging)
            {
                StopDragging();
            }
        }

        Vector3 startHandPosition, handPosition, newHandPosition, handDeltaMovement;
        bool aiming;
        Rigidbody pokeballInstance;
        float draggingTime;
        public Text debugText;

        /// <summary>
        /// Update the position of the object being dragged.
        /// </summary>
        private void UpdateDragging()
        {

            currentInputSource.TryGetPosition(currentInputSourceId, out newHandPosition);

            pokeballInstance.transform.position = newHandPosition;

            if (aiming)
            {
                if ((newHandPosition - handPosition).magnitude / Time.deltaTime > 0.5f)
                {
                    startHandPosition = handPosition;
                    draggingTime = 0;
                    aiming = false;
                }
                Debug.Log("aiming");
            }
            else
            {
                draggingTime += Time.deltaTime;
                Debug.Log("not aiming");

                if ((newHandPosition - Camera.main.transform.position).magnitude > 0.4f)
                {
                    StopDragging();
                }
            }
            //debugText.text = newHandPosition.ToString("F3");
            handPosition = newHandPosition;
        }

        /// <summary>
        /// Stops dragging the object.
        /// </summary>
        public void StopDragging()
        {
            if (!isDragging)
            {
                return;
            }

            Vector3 ballPosition = newHandPosition;

            // Calculate throwing force
            //float displacementY = target.transform.position.y - ballPosition.y;
            //Vector3 displacementXZ = new Vector3(target.transform.position.x - ballPosition.x, 0, target.transform.position.z - ballPosition.z);
            
            //float hitTime = Vector3.Magnitude(displacementXZ) / velocity;
            //Vector3 velocityY = Vector3.up * displacementY / hitTime - 0.5f * Physics.gravity * hitTime;
            //Vector3 velocityXZ = displacementXZ / hitTime;

            pokeballInstance.isKinematic = false;

            handDeltaMovement = newHandPosition - startHandPosition;
            var scaleProduct = Vector3.Scale(Camera.main.transform.right, handDeltaMovement);
            var force = draggingTime > 0 ? 2 * (handDeltaMovement * (scaleProduct.x + scaleProduct.y + scaleProduct.z) / handDeltaMovement.magnitude + handDeltaMovement) / draggingTime : Vector3.zero;
            pokeballInstance.AddForce(force, ForceMode.Impulse);
            pokeballInstance.GetComponent<Collider>().enabled = true;

            draggingTime = 0;

            debugText.text = force.ToString("F3");

            // Remove self as a modal input handler
            InputManager.Instance.PopModalInputHandler();

            isDragging = false;
            currentInputSource = null;
            StoppedDragging.RaiseEvent();
        }

        public void OnFocusEnter()
        {
            if (isGazed)
            {
                return;
            }

            isGazed = true;
        }

        public void OnFocusExit()
        {
            if (!isGazed)
            {
                return;
            }

            isGazed = false;
        }

        public void OnInputUp(InputEventData eventData)
        {
            if (currentInputSource != null &&
                eventData.SourceId == currentInputSourceId)
            {
                StopDragging();
            }
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (isDragging)
            {
                // We're already handling drag input, so we can't start a new drag operation.
                return;
            }

            if (!eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
            {
                // The input source must provide positional data for this script to be usable
                return;
            }

            currentInputSource = eventData.InputSource;
            currentInputSourceId = eventData.SourceId;
            StartDragging();
        }

        public void OnSourceDetected(SourceStateEventData eventData)
        {
            // Nothing to do
        }

        public void OnSourceLost(SourceStateEventData eventData)
        {
            if (currentInputSource != null && eventData.SourceId == currentInputSourceId)
            {
                StopDragging();
            }
        }
    }
}
