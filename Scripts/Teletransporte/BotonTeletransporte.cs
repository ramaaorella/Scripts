using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonTeletransporte : MonoBehaviour
{
    public string nombreSpawn;
    public GameObject jugador;
    public GameObject ubicacionTeleport;

    public void Teletransportar()
    {
        //ubicacionTeleport = GameObject.Find("Spawn (Recepcion)");
        ubicacionTeleport = GameObject.Find(nombreSpawn);
        transform.parent.parent.GetComponent<ModifyPlayerPosition>().modifyPlayerPosition(ubicacionTeleport.transform);
        //jugador.transform.position = ubicacionTeleport.transform.position;
        //HAY QUE SIMULAR QUE EL USUARIO APRETA ESCAPE
    }
}
