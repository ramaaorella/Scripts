using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientApi : MonoBehaviour
{
    public string url;

    void Start()
    {
        StartCoroutine(Get(url));
    }

    public IEnumerator Get(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(result);
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }

    public IEnumerator Post(string url, JsonObject envio)
    {
        var jsonData = JsonUtility.ToJson(envio);
        Debug.Log(jsonData);

        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();
        }
    }


    public abstract class JsonObject
    {

    }

    [System.Serializable]
    public class UserStory : JsonObject{
        public int id;
        public string titulo;
        public char ubicacion;
        public char prevUbicacion;
        public char etiqueta;
    }

    [System.Serializable]
    public class Actor : JsonObject
    {
        string firstName ="Ezequiel";
        string surname ="Carbajo";
        string email ="test@test";
        string password = "222";
    }
}
