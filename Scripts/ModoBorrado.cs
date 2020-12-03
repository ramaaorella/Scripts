using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModoBorrado : MonoBehaviour
{
    public AñadirObjetoALista blanco;
    public AñadirObjetoALista azul;
    public AñadirObjetoALista rojo;
    public AñadirObjetoALista amarillo;
    public GameObject borrado;

    private bool modoBorrado;

    public void start()
    {
        modoBorrado = false;
    }

    public void ClickModoBorrado()
    {
        if (modoBorrado)
        {
            modoBorrado = false;
            blanco.setModoBorrado(false);
            azul.setModoBorrado(false);
            rojo.setModoBorrado(false);
            amarillo.setModoBorrado(false);
            borrado.GetComponentInChildren<Text>().text = "El modo borrado esta: OFF";
        }
        else
        {
            modoBorrado = true;
            blanco.setModoBorrado(true);
            azul.setModoBorrado(true);
            rojo.setModoBorrado(true);
            amarillo.setModoBorrado(true);
            borrado.GetComponentInChildren<Text>().text = "El modo borrado esta: ON";
        }
    }
}
