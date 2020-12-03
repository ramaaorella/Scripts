using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class AbrirCanvas : MonoBehaviour
{
    public string teclaOn;
    public string teclaOff;
    public GameObject canvas;

    private Boolean activo;

    // Start is called before the first frame update
    void Start()
    {
        activo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(teclaOn))
        {
            if (!activo)
            {
                canvas.SetActive(true);
                activo = true;
            }
        }
        if (Input.GetKeyDown(teclaOff))
        {
            if (activo)
            {
                canvas.SetActive(false);
                activo = false;
            }
        }
    }
}