using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public partial class HandsThrowingSystem : Singleton<HandsTrackingManager>
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }

        public GameObject TrackingObject;

        private HashSet<uint> trackedHands = new HashSet<uint>();
        private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();

        new void Awake()
        {
            InteractionManager.SourceDetected += InteractionManager_SourceDetected;
            InteractionManager.SourceLost += InteractionManager_SourceLost;
            InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
        }

        public Rigidbody pokeballPrefab;
        Vector3 handPosStart, handPosLast;

        public Text textDisplay;

        public void OnInputClicked(InputClickedEventData eventData)
        {
            // Spawn Pokeball
            //Rigidbody pokeballInstance;
            //pokeballInstance = Instantiate(pokeballPrefab, handPosLast, Camera.main.transform.rotation);
            //pokeballInstance.isKinematic = false;
            //pokeballInstance.AddForce(handPosLast - handPosStart, ForceMode.Impulse);
            //Debug.Log("Tapped");
            textDisplay.text = "Tapped";
        }

        public void OnHoldStarted(HoldEventData eventData)
        {
            textDisplay.text = "Holding";
        }

        public void OnHoldCompleted(HoldEventData eventData)
        {
            textDisplay.text = "Completed";
        }

        public void OnHoldCanceled(HoldEventData eventData)
        {
            textDisplay.text = "Canceled";
        }

        private void InteractionManager_SourceUpdated(InteractionSourceState state)
        {
            uint id = state.source.id;
            Vector3 pos;

            if (state.source.kind == InteractionSourceKind.Hand)
            {
                if (trackingObject.ContainsKey(state.source.id))
                {
                    if (state.properties.location.TryGetPosition(out pos))
                    {
                        //trackingObject[state.source.id].transform.position = pos;
                        //handPosLast = pos;
                    }
                }
            }

        }

        private void InteractionManager_SourceDetected(InteractionSourceState state)
        {
            Debug.Log("Source detected!");
            // Check to see that the source is a hand.
            //if (state.source.kind != InteractionSourceKind.Hand)
            //{
            //    return;
            //}
            //trackedHands.Add(state.source.id);

            //var obj = Instantiate(TrackingObject) as GameObject;
            //Vector3 pos;
            //if (state.properties.location.TryGetPosition(out pos))
            //{
            //    obj.transform.position = pos;
            //    handPosStart = pos;
            //}

            //trackingObject.Add(state.source.id, obj);
        }

        private void InteractionManager_SourceLost(InteractionSourceState state)
        {
            Debug.Log("Source lost!");
            // Check to see that the source is a hand.
            //if (state.source.kind != InteractionSourceKind.Hand)
            //{
            //    return;
            //}

            //if (trackedHands.Contains(state.source.id))
            //{
            //    trackedHands.Remove(state.source.id);
            //}

            //if (trackingObject.ContainsKey(state.source.id))
            //{
            //    var obj = trackingObject[state.source.id];
            //    trackingObject.Remove(state.source.id);
            //    Destroy(obj, 0.5f);
            //}
        }

        new void OnDestroy()
        {
            InteractionManager.SourceDetected -= InteractionManager_SourceDetected;
            InteractionManager.SourceLost -= InteractionManager_SourceLost;
            InteractionManager.SourceUpdated -= InteractionManager_SourceUpdated;
        }
    }
}