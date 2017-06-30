using UnityEngine;

public class TimeModifier : MonoBehaviour {

    public float timeScale = 1;

	void Start () {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime *= timeScale;
    }
}
