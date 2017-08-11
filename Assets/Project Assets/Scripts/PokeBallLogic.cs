using UnityEngine;
using UnityEngine.AI;

public class PokeballLogic : MonoBehaviour {
    
    public float despawnTime = 10;

    public bool isFirstBall = false;
    public GameObject myPokemon, unitIndicator;
    
    Animator anim;

    AudioSource[] audioSources;
    public AudioClip bounce, struggle, caught;

    public GameObject captureVFX, caughtVFX, ballDisappear;

    Material[] pokemonMaterials;
    bool firstStage = true;
    float lerp;
    int range;
    Material[] rend = new Material[5];
    Vector3 emissionColor = Vector3.zero;
    Vector4 color = Vector4.one;
    
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
            if (firstStage)
            {
                lerp += Time.deltaTime;
                emissionColor = Vector3.Lerp(Vector3.zero, Vector3.one, lerp);
                for (int i = 0; i < range; i++)
                {
                    pokemonMaterials[i].SetVector("_EmissionColor", emissionColor);
                }
                if (lerp > 1)
                {
                    firstStage = false;
                    for (int i = 0; i < range; i++)
                    {
                        pokemonMaterials[i].SetVector("_Color", Vector4.zero);
                        //pokemonMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        pokemonMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        pokemonMaterials[i].SetInt("_ZWrite", 0);
                        pokemonMaterials[i].DisableKeyword("_ALPHATEST_ON");
                        pokemonMaterials[i].EnableKeyword("_ALPHABLEND_ON");
                        pokemonMaterials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        pokemonMaterials[i].renderQueue = 3000;
                    }
                }
            }
            else
            {
                lerp -= Time.deltaTime * 0.6f;
                emissionColor = Vector3.Lerp(Vector3.zero, Vector3.one, lerp);
                //color = Vector4.Lerp(Vector4.zero, new Vector4(0, 0, 0, 1), lerp);
                for (int i = 0; i < range; i++)
                {
                    pokemonMaterials[i].SetVector("_EmissionColor", emissionColor);
                }
            }
        }

        else if (captured)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, 5 * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision pokemon)
    {
        if (isFirstBall)
        {

            var myPokemonInstant = Instantiate(myPokemon, transform.position, Quaternion.identity);
            myPokemonInstant.GetComponent<PikachuAI>().isWild = false;
            myPokemonInstant.tag = "MyPokemon";
            Instantiate(unitIndicator, myPokemonInstant.transform);
            Destroy(gameObject);
            isFirstBall = false;
        }

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

                    // Set pokeball's capture post
                    Instantiate(captureVFX, transform);
                    targetPosition = transform.position + 0.5f * Vector3.up + 0.5f * new Vector3(transform.position.x - pokemon.transform.position.x, 0 , transform.position.z - pokemon.transform.position.z).normalized;
                    targetAngle = Quaternion.LookRotation(pokemon.transform.position - targetPosition);

                    Destroy(pokemon.gameObject, 3);
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
        anim.SetInteger("State", 2);
    }

    public void Pokemon_Caught_VFX()
    {
        Instantiate(caughtVFX, transform).transform.parent = transform.parent;
    }

    public void Pokemon_Caught()
    {
        Instantiate(ballDisappear, transform).transform.parent = transform.parent;
        anim.SetInteger("State", 3);
        Destroy(gameObject);
    }
}
