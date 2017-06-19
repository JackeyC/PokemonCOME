using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PokemonSpawner : MonoBehaviour {


    public GameObject[] numberOfPokemon;
    public GameObject cameraPosition;

    float elapsedTime;
    
	void Update () {
        elapsedTime += Time.deltaTime;
        if (transform.childCount < 10)
        {
            if (elapsedTime > 5)
            {
                Vector3 destination = 5 * Random.insideUnitCircle;
                destination += cameraPosition.transform.position;
                NavMeshHit navMeshHit;
                NavMesh.SamplePosition(destination, out navMeshHit, 5, 1);
                Quaternion spawnRoation = Random.rotation;
                if (navMeshHit.hit)
                {
                    var pokemon = Instantiate(numberOfPokemon[Random.Range(0,numberOfPokemon.Length)], navMeshHit.position, spawnRoation);
                    pokemon.transform.parent = transform;
                }
                //else
                //{
                //    Debug.Log("No Navmesh Near Spawn Point");
                //}
                elapsedTime = 0;
            }
        }
    }
}
