using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToTarget : MonoBehaviour {

    public Transform targetTransform;
	
	void Update () {
        transform.position = targetTransform.position;
	}
}
