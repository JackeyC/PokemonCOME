using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

    public float Seconds = 10;

	void Start () {
        Destroy(gameObject, Seconds);
    }

}
