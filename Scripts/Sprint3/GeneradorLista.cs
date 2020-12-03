using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Proceso : MonoBehaviour
{
    public string tarea;                      //LAS HICE PUBLICAS PARA PODER DEBUGGUEAR
    public string duracion;                   //LAS HICE PUBLICAS PARA PODER DEBUGGUEAR




    public Proceso(string x, string y)
    {
        this.tarea = x;
        this.duracion = y;
        //Debug.Log("Proceso: " + tarea + " - " + duracion);
    }
}

public class GeneradorLista : MonoBehaviour
{
    public GameObject bot;
    public GameObject content;
    
    List<Proceso> listaProcesos = new List<Proceso>();

    private Transform tarea;
    private Transform duracion;

    public void generarLista()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            tarea = content.transform.GetChild(i);
            duracion = tarea.transform.GetChild(1);
            añadirProceso(tarea.GetComponentInChildren<TMP_Text>().text, duracion.GetComponentInChildren<TMP_Text>().text);
        }
        imprimirLista();
        bot.GetComponentInChildren<PruebaChat>().updateList();
    }

    public List<Proceso> getListaDeProcesos()
    {
        return listaProcesos;
    }

    void añadirProceso(string tarea, string duracion)
    {
        Debug.Log("Estoy agregando la tarea " + tarea + " cuya duracion es de " + duracion);
        Proceso elemento = new Proceso(tarea, duracion);
        listaProcesos.Add(elemento);
    }

    void imprimirLista()
    {
        for (int i = 0; i < listaProcesos.Count; i++)
        {
            Debug.Log("Lista de procesos: " + listaProcesos[i].tarea + " - " + listaProcesos[i].duracion);
        }
    }
}
