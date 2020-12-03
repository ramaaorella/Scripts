using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class AñadirObjetoALista : MonoBehaviour
{
    public GameObject itemTemplate;
    public GameObject content;
    public GameObject contentInProgress;
    public GameObject contentDone;

    //private bool chequeoDone;
    private bool modoBorrado;
    private bool toDo;
    private bool inProgress;
    private bool done;

    int index = 0;

    public void Start()
    {
        //chequeoDone = false;
        modoBorrado = false;
        toDo = true;
        inProgress = false;
        done = false;
    }

    public void ClickBotonAñadir()
    {

        GetCubeFromScene scriptDelCubo = GetComponentInParent<GetCubeFromScene>();
        if (scriptDelCubo != null) 
        { 
            GameObject elcuboencuestion = scriptDelCubo.theCubeFromTheScene;
            print("henlo");
            moverYSincronizarTodos var = GetComponentInParent<moverYSincronizarTodos>();
            if (var!=null)
            {
                print("henlo2");
                var.modifYSincronizarTodosFunc(elcuboencuestion);
            }                
            
        }
        
        var copia = Instantiate(itemTemplate);
        //SI SE QUISIERA AÑADIR EN OTRO LADO HACER LOS IFs ACA
        //copia.transform.parent = content.transform;
        copia.transform.SetParent(content.transform, false);
        //copia.GetComponentInChildren<Text>().text = texto + " " + (index + 1).ToString();

        int copiaIndex = index;
        copia.GetComponent<Button>().onClick.AddListener(

            () =>
            {
                //ACA VA EL CODIGO PARA LO QUE SE QUIERA HACER AL APRETAR UN BOTON DE LA LISTA
                //Debug.Log("Index = " + copiaIndex);
                //Destroy(copia);
                //copia.transform.parent = contentInProgress.transform;

                if (modoBorrado)
                {
                    Destroy(copia);
                    //chequeoDone = false;
                }
                else
                {
                    if (toDo)
                    {
                        copia.transform.parent = content.transform;
                    }
                    else
                    {
                        if (inProgress)
                        {
                            copia.transform.parent = contentInProgress.transform;
                        }
                        else
                        {
                            copia.transform.parent = contentDone.transform;
                        }
                    }
                    /*if (!chequeoDone)
                    {
                        copia.transform.parent = contentInProgress.transform;
                        chequeoDone = true;
                    }
                    else
                    {
                        copia.transform.parent = contentDone.transform;
                    }*/
                }
            }
        );

        index++;
    }

    public void setModoBorrado(bool modoBorrado)
    {
        this.modoBorrado = modoBorrado;
    }

    public void setToDo(bool valor)
    {
        this.toDo = valor;
    }

    public void setInProgress(bool valor)
    {
        this.inProgress = valor;
    }

    public void setDone(bool valor)
    {
        this.done = valor;
    }

   
}
