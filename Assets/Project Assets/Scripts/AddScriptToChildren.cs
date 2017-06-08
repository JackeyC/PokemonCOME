using UnityEngine;

public class AddScriptToChildren : MonoBehaviour {
    
	void Update () {
		foreach (Transform child in transform)
        {
            if(!child.gameObject.GetComponent<NavMeshSourceTag>())
            {
                child.gameObject.AddComponent<NavMeshSourceTag>();
            }
        }
	}
}
