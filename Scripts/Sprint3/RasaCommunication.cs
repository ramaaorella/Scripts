using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Mirror;
using UnityEngine.EventSystems;

/*public class PostMessage
{
    public string message;
    public string sender;
}

[Serializable]
public class RecieveData
{
    public string recipient_id;
    public string text;
    public string image;
    public string attachemnt;
    public string button;
    public string element;
    public string quick_replie;
}

[Serializable]
public class RootMessages
{
    public RecieveData[] messages;
}*/

public class RasaCommunication : MonoBehaviour
{
    public string message;
    public string respuesta;
    public string sender; 

    private const string rasa_url = "https://asistente-comp.herokuapp.com/webhooks/rest/webhook";

    public void SendMessageToRasa()
    {
        PostMessage postMessage = new PostMessage
        {
            sender = sender,                              //NOMBRE
            message = message
        };
        string jsonBody = JsonUtility.ToJson(postMessage);
        StartCoroutine(PostRequest(rasa_url, jsonBody));
    }


    private IEnumerator PostRequest(string url, string jsonBody)
    {
        // Create a request to hit the rasa custom connector
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] rawBody = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawBody);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        RecieveMessage(request.downloadHandler.text);
    }

    public void RecieveMessage(string response)
    {
        RootMessages recieveMessages = JsonUtility.FromJson<RootMessages>("{\"messages\":" + response + "}");
        foreach (RecieveData message in recieveMessages.messages)
        {
            FieldInfo[] fields = typeof(RecieveData).GetFields();
            foreach (FieldInfo field in fields)
            {
                string data = null;
                try
                {
                    data = field.GetValue(message).ToString();           //ESTA ES LA RESPUESTA
                }
                catch (NullReferenceException) { }
                if (data != null && field.Name != "recipient_id")
                {
                    respuesta = data;
                }
            }
        }
    }
}

