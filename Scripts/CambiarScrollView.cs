using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CambiarScrollView : MonoBehaviour
{
    public GameObject activar;
    public GameObject desactivar1;
    public GameObject desactivar2;

    public void activarScrollView()
    {
        activar.SetActive(true);
        desactivar1.SetActive(false);
        desactivar2.SetActive(false);
    }
}
