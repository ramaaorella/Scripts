using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInicialToMenuROles : MonoBehaviour
{


    public void changeScene(string nombreEscena)
    {
        Application.LoadLevel(nombreEscena);
    }
}
