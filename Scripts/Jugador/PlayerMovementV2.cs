﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using Mirror;

public class PlayerMovementV2 : NetworkBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public GameObject prefabHombre;
    public GameObject prefabMujer;
    public bool hombreActivado;
    public KeyCode tecla;
    private Animator anim;

    Vector3 velocity;
    bool isGrounded;
    float x, z;

    // Start is called before the first frame update
    void Start()
    {
        anim = prefabHombre.GetComponent<Animator>();
        hombreActivado = true;
        prefabMujer.SetActive(false);
    }

    // Update is called once per frame
    [Client]
    void Update()
    {
        if (!hasAuthority) { return; }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", z);
        /*
        if (Input.GetKey(tecla) && hombreActivado)
        {
            activarMujer();
        }

        if (Input.GetKey(tecla) && !hombreActivado)
        {
            activarHombre();
        }*/

        if (Input.GetKey(tecla))
        {
            activarMujer();
        }
    }

    [Client]
    public void activarMujer()
    {
        prefabMujer.SetActive(true);
        anim = prefabMujer.GetComponent<Animator>();
        prefabHombre.SetActive(false);
        hombreActivado = false;
        CmdCambiarAvataresEnTodosLosClientes();
    }

    [Command]
    private void CmdCambiarAvataresEnTodosLosClientes()
    {
        RpcCambiarAvatar();
    }

    [ClientRpc]
    private void RpcCambiarAvatar()
    {
        prefabMujer.SetActive(true);
        anim = prefabMujer.GetComponent<Animator>();
        prefabHombre.SetActive(false);
        hombreActivado = false;
    }

    [Client]
    public void activarHombre()
    {
        prefabHombre.SetActive(true);
        anim = prefabHombre.GetComponent<Animator>();
        prefabMujer.SetActive(false);
        hombreActivado = true;
    }
}

