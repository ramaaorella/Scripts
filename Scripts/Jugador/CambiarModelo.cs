using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarModelo : MonoBehaviour
{
    public string controllerHombre;
    public Avatar avatarHombre;
    public string controllerMujer;
    public Avatar avatarMujer;
    public GameObject jugador;
    public Animator anim;
    public KeyCode teclaModelo = KeyCode.M;
    public bool hombreActivado;

    // Start is called before the first frame update
    void Start()
    {
        anim = jugador.GetComponent<Animator>();
        hombreActivado = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(teclaModelo))
            {
                if (hombreActivado)
                {
                    anim.runtimeAnimatorController = Resources.Load(controllerMujer) as RuntimeAnimatorController;
                    anim.avatar = avatarMujer;
                    hombreActivado = false;
                }
                else
                {
                    anim.runtimeAnimatorController = Resources.Load(controllerHombre) as RuntimeAnimatorController;
                    anim.avatar = avatarHombre;
                    hombreActivado = true;
                }
            }
        }
    }
}
