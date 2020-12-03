using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferirTexto : MonoBehaviour
{
    public string texto;
    public GameObject inputField;
    public GameObject displayBlanco;
    public GameObject displayAzul;
    public GameObject displayRojo;
    public GameObject displayAmarillo;
    //HABRIA QUE HACER UNA LISTA MAS QUE PASAR CADA UNO

    public void cargarTexto()
    {
        texto = inputField.GetComponent<Text>().text;
        displayBlanco.GetComponentInChildren<Text>().text = texto;
        displayAzul.GetComponentInChildren<Text>().text = texto;
        displayRojo.GetComponentInChildren<Text>().text = texto;
        displayAmarillo.GetComponentInChildren<Text>().text = texto;
    }
}
