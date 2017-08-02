using UnityEngine;

public class PokeballEventHandler : MonoBehaviour {

	public void Fall()
    {
        transform.parent.GetComponent<PokeBallLogic>().Fall();
    }

    public void Play_Struggle_SFX()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void Pokemon_Caught_VFX()
    {
        transform.parent.GetComponent<PokeBallLogic>().Pokemon_Caught_VFX();
    }

    public void Pokemon_Caught()
    {
        transform.parent.GetComponent<PokeBallLogic>().Pokemon_Caught();
    }
}
