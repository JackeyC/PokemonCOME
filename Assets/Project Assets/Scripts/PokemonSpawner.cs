using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PokemonSpawner : MonoBehaviour {


    public GameObject[] pokemonTypes;
    public int maxPokemon = 8;
    public GameObject cameraPosition;

    float elapsedTime;
    
	void Update () {
        if (transform.childCount <= maxPokemon)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 5)
            {
                Vector3 destination = 5 * Random.insideUnitCircle;
                destination += cameraPosition.transform.position;
                NavMeshHit navMeshHit;
                NavMesh.SamplePosition(destination, out navMeshHit, 5, 1);
                Quaternion spawnRoation = Random.rotation;
                if (navMeshHit.hit)
                {
                    var pokemon = Instantiate(pokemonTypes[Random.Range(0,pokemonTypes.Length)], navMeshHit.position, spawnRoation);
                    pokemon.transform.parent = transform;
                }
                elapsedTime = 0;
            }
        }
    }
}
