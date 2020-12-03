using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuscarPadre : MonoBehaviour
{
    public bool encontrado;

    // Start is called before the first frame update
    void Start()
    {
        this.encontrado = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!encontrado)
        {
            var jugador = GameObject.Find("PrefabJugador(Clone)");

            if (jugador != null)
            {
                this.transform.parent = jugador.transform;
                this.encontrado = true;
                jugador.name = "PrefabJugador(Clone) - Voice Asignado";
                //this.transform = jugador.transform;                
            }
        }
    }
}
