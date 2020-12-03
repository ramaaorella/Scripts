using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Teleportation : MonoBehaviour
{
    public string salaAnterior;
    public Transform ubicacionTeleport;



    void OnTriggerEnter(Collider other)
    {
        print("TeleportActivado");
        other.transform.position = ubicacionTeleport.transform.position;
        other.transform.rotation = ubicacionTeleport.transform.rotation;
        //Guardar datos en Base
        other.GetComponent<RoomSave>().addMove(ubicacionTeleport.name);

        //Almacenamiento de datos en la base

        //aca obtengo el nombre de la sala
        string sala = ubicacionTeleport.name;
        sala = sala.Split('(')[1];
        sala = sala.Split(')')[0];

        //obtengo el nombre del jugador
        string name = other.GetComponent<SyncPlayerInfo>().playerName;
        name = "Joaquin";

        //si la sala es alguna de estas, es que sali de una sala y entre al pasillo
        //por lo que es una salida de sala
        if (sala == "Pasillo" || sala == "Pasillo 3" || sala == "Pasillo 2")
        {
            //aca creo un objeto de MyClass el cual lo paso a formato json con la sala de la que sali
            MyClass miClase = new MyClass();
            miClase.oficina = salaAnterior;
            miClase.usuario = name;
            string jsonData = JsonUtility.ToJson(miClase);

            StartCoroutine(Post("https://diseno2020.herokuapp.com/api/actorOficina/salir", jsonData));

            print("Salio de " + salaAnterior + " " + name);

        }
        //aqui entra si es que entre a una sala
        else
        {
            //aca creo un objeto de MyClass el cual lo paso a formato json con la sala a la que entro
            MyClass miClase = new MyClass();
            miClase.oficina = sala;
            miClase.usuario = name;
            string jsonData = JsonUtility.ToJson(miClase);

            //StartCoroutine(Post("https://diseno2020.herokuapp.com/api/actorOficina", jsonData));

            print("Entro a " + sala + " " + name);
        }
    }

        //corrutina para hacer el post en la base
        public IEnumerator Post(string url, string envio)
        {
            Debug.Log(url);
            using (UnityWebRequest www = UnityWebRequest.Post(url, envio))
            {
                www.SetRequestHeader("content-type", "application/json");
                www.uploadHandler.contentType = "application/json";
                www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(envio));
                yield return www.SendWebRequest();

                if (www.isNetworkError)
                    Debug.LogError(string.Format("{0}: {1}", www.url, www.error));
                else
                    Debug.Log(string.Format("Response: {0}", www.downloadHandler.text));
            }
        }

        public class MyClass
        {   
            public string usuario;
            public string oficina;
        }


}


