using UnityEngine;
using System.Collections;
//using Boo.Lang;
using UnityEngine.UI;

public class CapturePokemon : MonoBehaviour
{
    bool captured = false;
    int count;
    //private List<Pokemons> PokemonList = new List<Pokemons>();
    float time = 0;

    void Update()
    {
        time += Time.deltaTime;

        if (captured)
        {
            if (time > 3)
            {
                captured = false;
            }
        }
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Pokemon")
        {
            //PokemonList.Add((Pokemons)col.gameObject.GetComponent<Pikachu>());

            Destroy(col.gameObject);
            //gameObject.layer = 64;
            count++;
            captured = true;
        }
    }

    void Choose()
    {

    }


    void OnGUI()
    {
        if (captured)
        {
            string List = string.Format(" {0}", count);
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 1000, 200), "Gotcha!" + List);
        }
        
    }

}

