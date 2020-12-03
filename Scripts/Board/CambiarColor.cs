using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CambiarColor : MonoBehaviour
{
    public byte rojo;
    public byte verde;
    public byte azul;
    public GameObject imagen;

    private Transform userStorie;

    public void cambiarColor()
    {
        userStorie = imagen.transform.GetChild(0);
        //userStorie = imagen.transform.Find("User Storie(Clone)")
        userStorie.GetComponent<Image>().color = new Color32(rojo, verde, azul, 255);
    }


}
