using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirConTecla : MonoBehaviour
{
    public KeyCode pizarrateclaOn;
    public KeyCode tpteclaOn = KeyCode.LeftAlt;
    public KeyCode teclaOff = KeyCode.Escape;
    public KeyCode teclaAsistente = KeyCode.Z;
    public KeyCode teclaChat = KeyCode.X;
    public GameObject pizarracanvas;
    public GameObject tpcanvas;
    public GameObject asistenteCanvas;
    public GameObject chat;
    public GameObject avisoChat;

    public GameObject jugador;

    public Boolean pizarraactivo;
    private Boolean tpactivo;
    private Boolean asistenteactivo;
    private Boolean chatactivo;

    private CharacterController control;

    // Start is called before the first frame update
    void Start()
    {
        pizarraactivo = false;
        tpactivo = false;
        asistenteactivo = false;
        chatactivo = false;

        control = jugador.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(pizarrateclaOn) || Input.GetKeyDown(tpteclaOn) || Input.GetKeyDown(teclaAsistente) || Input.GetKeyDown(teclaChat))
            {
                if (Input.GetKeyDown(pizarrateclaOn))
                {
                    if (!pizarraactivo)
                    {
                        pizarracanvas.SetActive(true);
                        pizarraactivo = true;
                        jugador.GetComponent<MouseLook>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        control.enabled = false;
                    }
                }
                else if (Input.GetKeyDown(tpteclaOn))
                {
                    if (!tpactivo)
                    {
                        tpcanvas.SetActive(true);
                        tpactivo = true;
                        jugador.GetComponent<MouseLook>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        control.enabled = false;
                    }
                }
                else if (Input.GetKeyDown(teclaAsistente))
                {
                    asistenteCanvas.SetActive(true);
                    asistenteactivo = true;
                    jugador.GetComponent<MouseLook>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    control.enabled = false;
                }
                else if (Input.GetKeyDown(teclaChat))
                {
                    avisoChat.SetActive(false);
                    chat.SetActive(true);
                    chatactivo = true;
                    jugador.GetComponent<MouseLook>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    control.enabled = false;
                }
            }
        }
        else if (Input.GetKeyDown(teclaOff))
        {
            if (tpactivo)
            {
                tpcanvas.SetActive(false);
                tpactivo = false;
                jugador.GetComponent<MouseLook>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                control.enabled = true;
            }
            else if (pizarraactivo)
            {
                pizarracanvas.SetActive(false);
                pizarraactivo = false;
                jugador.GetComponent<MouseLook>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                control.enabled = true;
            }
            else if (asistenteactivo) {
                asistenteCanvas.SetActive(false);
                asistenteactivo = false;
                jugador.GetComponent<MouseLook>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                control.enabled = true;
            }
            else if (chatactivo)
            {
                chat.SetActive(false);
                avisoChat.SetActive(true);
                chatactivo = false;
                jugador.GetComponent<MouseLook>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                control.enabled = true;
            }
        }
    }
}


