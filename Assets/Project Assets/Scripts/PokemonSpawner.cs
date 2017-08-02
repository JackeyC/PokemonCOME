using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PokemonSpawner : MonoBehaviour {


    public GameObject[] pokemonTypes;
    public int maxPokemon = 8;
    //public GameObject cameraPosition;

    float elapsedTime;
    
	void Update () {
        if (transform.childCount <= maxPokemon)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 5)
            {
                float radian = 2 * Random.Range(0, Mathf.PI);
                Vector3 spawnPoint = Random.Range(5,1) * new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian));
                spawnPoint += Camera.main.transform.position;
                NavMeshHit navMeshHit;
                NavMesh.SamplePosition(spawnPoint, out navMeshHit, 1, 1);
                
                if (navMeshHit.hit)
                {
                    Quaternion spawnRoation = Random.rotation;
                    var pokemon = Instantiate(pokemonTypes[Random.Range(0,pokemonTypes.Length)], navMeshHit.position, spawnRoation);
                    pokemon.transform.parent = transform;
                }
                elapsedTime = 0;
            }
        }
    }
}
