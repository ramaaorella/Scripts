using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CambiarTexto : MonoBehaviour
{
    public GameObject imagen;
    public GameObject inputField;

    private Transform userStorie;
    private string texto;
    private Transform duracion;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (imagen.transform.childCount > 0)
        {
            userStorie = imagen.transform.GetChild(0);
            texto = inputField.GetComponent<Text>().text;
            userStorie.GetComponentInChildren<TMP_Text>().text = texto;
        }

        
    }

    /*public void cambiarTexto()
    {
        userStorie = imagen.transform.GetChild(0);
        texto = inputField.GetComponent<Text>().text;
        userStorie.GetComponentInChildren<TMP_Text>().text = texto;
    }*/

    public void cambiarDuracion(string texto)
    {
        if (imagen.transform.childCount > 0)
        {
            userStorie = imagen.transform.GetChild(0);
            duracion  = userStorie.transform.GetChild(1);
            duracion.GetComponentInChildren<TMP_Text>().text = texto;
        }
    }
}
