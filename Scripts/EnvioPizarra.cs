using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvioPizarra : MonoBehaviour
{
    public AñadirObjetoALista blanco;
    public AñadirObjetoALista azul;
    public AñadirObjetoALista rojo;
    public AñadirObjetoALista amarillo;
    public GameObject envio;

    public void ClickToDo()
    {
        blanco.setToDo(true);
        azul.setToDo(true);
        rojo.setToDo(true);
        amarillo.setToDo(true);
        blanco.setInProgress(false);
        azul.setInProgress(false);
        rojo.setInProgress(false);
        amarillo.setInProgress(false);
        blanco.setDone(false);
        azul.setDone(false);
        rojo.setDone(false);
        amarillo.setDone(false);
        envio.GetComponentInChildren<Text>().text = "To do";
    }

    public void ClickInProgress()
    {
        blanco.setToDo(false);
        azul.setToDo(false);
        rojo.setToDo(false);
        amarillo.setToDo(false);
        blanco.setInProgress(true);
        azul.setInProgress(true);
        rojo.setInProgress(true);
        amarillo.setInProgress(true);
        blanco.setDone(false);
        azul.setDone(false);
        rojo.setDone(false);
        amarillo.setDone(false);
        envio.GetComponentInChildren<Text>().text = "In progress";
    }

    public void ClickDone()
    {
        blanco.setToDo(false);
        azul.setToDo(false);
        rojo.setToDo(false);
        amarillo.setToDo(false);
        blanco.setInProgress(false);
        azul.setInProgress(false);
        rojo.setInProgress(false);
        amarillo.setInProgress(false);
        blanco.setDone(true);
        azul.setDone(true);
        rojo.setDone(true);
        amarillo.setDone(true);
        envio.GetComponentInChildren<Text>().text = "Done";
    }
}
