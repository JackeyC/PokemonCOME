using UnityEngine;
using UnityEngine.AI;

public class PokeBallLogic : MonoBehaviour {

    public float duration = 2;
    public float despawnTime = 10;
    
    Animator anim;

    AudioSource[] audioSources;
    public AudioClip bounce, capture, struggle, caught;

    public GameObject captureVFX, caughtVFX, ballDisappear;

    Material[] materialOriginal = new Material[5];
    public Material materialGlow;

    Material[] pokemonMaterials;

    int range;
    Material[] rend = new Material[5];
    
    Rigidbody pokeball_rb;
    bool capturing, captured = false;
    bool empty = true;
    Quaternion targetAngle;
    Vector3 targetPosition;
    int collisionCount;

    void Start()
    {
        audioSources = gameObject.GetComponents<AudioSource>();
        Destroy(gameObject, despawnTime);
    }

    void Update()
    {
        if (capturing)
        {
            // Move pokeball into capture post
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, 10 * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, 5 * Time.deltaTime);

            // Change pokemon color
            //float lerp = Mathf.Lerp(Time.time, duration) / duration;
            for (int i = 0; i < range; i++)
            {
                //pokemonMaterials[i].Lerp(materialOriginal[i], materialGlow, Time.deltaTime);

                pokemonMaterials[i].renderQueue = 3000;
            }
        }
        else if (captured)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, 5 * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision pokemon)
    {
        audioSources[0].Play();

        if (empty)
        {
            collisionCount++;
            if (collisionCount < 3)
            {
                if (pokemon.gameObject.tag == "Pokemon")
                {
                    // Change pokemon color

                    pokemonMaterials = pokemon.gameObject.GetComponentInChildren<Renderer>().materials;
                    range = pokemonMaterials.Length;

                    for (int i = 0; i < range; i++)
                    {
                        materialOriginal[i] = pokemonMaterials[i];
                        
                        pokemonMaterials[i] = materialGlow;
                        pokemonMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        pokemonMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        pokemonMaterials[i].SetInt("_ZWrite", 0);
                        pokemonMaterials[i].DisableKeyword("_ALPHATEST_ON");
                        pokemonMaterials[i].EnableKeyword("_ALPHABLEND_ON");
                        pokemonMaterials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");

                        //pokemonMaterials[i] = materialGlow;
                        //Debug.Log(pokemonMaterials[i]);
                        //rend[i] = materialOriginal[i];
                    }
                    

                    // Set capture animation
                    anim = GetComponentInChildren<Animator>();
                    anim.SetInteger("State", 1);

                    // Update regidbody parameters
                    pokeball_rb = GetComponent<Rigidbody>();
                    pokeball_rb.freezeRotation = true;
                    pokeball_rb.mass = 5;
                    pokeball_rb.isKinematic = true;

                    // Disable pokemon components not needed during capture
                    pokemon.gameObject.GetComponent<Animator>().enabled = false;
                    pokemon.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    var script = pokemon.gameObject.GetComponent<PikachuAI>();
                    if (script)
                    {
                        script.enabled = false;
                    }
                    else
                    {
                        pokemon.gameObject.GetComponent<PidgeyAI>().enabled = false;
                    }
                    pokemon.collider.enabled = false;

                    // Play capture sound
                    audioSources[1].clip = capture;
                    audioSources[1].Play();

                    // Move pokeball into capture post
                    Instantiate(captureVFX, transform);
                    targetAngle = Quaternion.LookRotation(pokemon.transform.position - transform.position);
                    targetPosition = transform.position + 0.3f * Vector3.up + 0.2f * new Vector3(transform.position.x - pokemon.transform.position.x, 0 , transform.position.z - pokemon.transform.position.z).normalized;

                    Destroy(pokemon.gameObject, 0.8f);
                    capturing = true;
                    empty = false;
                }
            }
            else
            {
                empty = false;
            }
        }
    }

    public void Fall()
    {
        targetAngle = Quaternion.LookRotation(new Vector3(Camera.main.transform.position.x - transform.position.x, 0, Camera.main.transform.position.z - transform.position.z));
        capturing = false;
        captured = true;
        pokeball_rb.isKinematic = false;
    }

    public void Play_Struggle_SFX()
    {
        audioSources[0].clip = struggle;
        audioSources[0].Play();
    }

    public void Pokemon_Caught_VFX()
    {
        Instantiate(caughtVFX, transform).transform.parent = transform.parent;
        anim.SetInteger("State", 0);
    }

    public void Pokemon_Caught()
    {
        Instantiate(ballDisappear, transform).transform.parent = transform.parent;
        Destroy(gameObject);

    }
}
