using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorModelo : MonoBehaviour
{
    public bool spawnHombre;

    // Start is called before the first frame update
    void Start()
    {
        spawnHombre = true;
    }

    public void setSpawnHombre(bool valor)
    {
        spawnHombre = valor;
    }

    public bool getSpawnHombre()
    {
        return spawnHombre;
    }
}
