/*using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Mirror;
using UnityEngine.EventSystems;

///PARA CONECTAR CON RASA

/// <summary>
/// This class handles all the network requests and serialization/deserialization of data
/// </summary>
/// 
public class PostMessage
{
    public string message;
    public string sender;
}

/// <summary>
/// This class is used to deserialize the resonse json for each
/// individual message.
/// </summary>
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

/// <summary>
/// This class is a wrapper for individual messages sent by the bot.
/// </summary>
[Serializable]
public class RootMessages
{
    public RecieveData[] messages;
}



public class NetworkManager : MonoBehaviour
{

    // reference to BotUI class
    public BotUI botUI;

    public GameObject jugador;

    // the url at which bot's custom connector is hosted
    private const string rasa_url = "https://asistente-comp.herokuapp.com/webhooks/rest/webhook";

    /// <summary>
    /// This method is called when user has entered their message and hits the send button.
    /// It calls the <see cref="NetworkManager.PostRequest(string, string)"> coroutine to send
    /// the user message to the bot and also updates the UI with user message.
    /// </summary>
    public void SendMessageToRasa()
    {
        // get user messasge from input field, create a json object 
        // from user message and then clear input field
        string message = botUI.input.text;
        if (message == "") return;
        EventSystem.current.SetSelectedGameObject(botUI.input.gameObject, null);
        botUI.input.OnPointerClick(new PointerEventData(EventSystem.current));
        botUI.input.text = "";

        PostMessage postMessage = new PostMessage
        {
            //sender = jugador.GetComponent<SyncPlayerInfo>().getID(),
            sender = "viviantjuaneliel@gmail.com",
            message = message
        };
        string jsonBody = JsonUtility.ToJson(postMessage);

        // update UI object with user message
        botUI.UpdateDisplay("user", message, "text");

        // Dependiendo del rol se cambiaría el webhook
        int rolJugador = jugador.GetComponent<SyncPlayerInfo>().rol;
        Debug.Log("rol asignado: " + rolJugador);
        switch (rolJugador)
        {
            case PlayerInfo.ProductOwner:
                // Create a post request with the data to send to Rasa server
                StartCoroutine(PostRequest(rasa_url, jsonBody));
                break;
            case PlayerInfo.ScrumMaster:
                // Create a post request with the data to send to Rasa server
                StartCoroutine(PostRequest(rasa_url, jsonBody));
                break;
            case PlayerInfo.ScrumMember:
                // Create a post request with the data to send to Rasa server
                StartCoroutine(PostRequest(rasa_url, jsonBody));
                break;
        }
    }

    /// <summary>
    /// This is a coroutine to asynchronously send a POST request to the Rasa server with 
    /// the user message. The response is deserialized and rendered on the UI object.
    /// </summary>
    /// <param name="url">the url where Rasa server is hosted</param>
    /// <param name="jsonBody">user message serialized into a json object</param>
    /// <returns></returns>
    private IEnumerator PostRequest(string url, string jsonBody)
    {
        // Create a request to hit the rasa custom connector
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] rawBody = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawBody);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // receive the response
        yield return request.SendWebRequest();

        // Render the response on UI object
        RecieveMessage(request.downloadHandler.text);
    }


    /// <summary>
    /// This method updates the UI object with bot response
    /// </summary>
    /// <param name="response">response json recieved from the bot</param>
    public void RecieveMessage(string response)
    {
        // Deserialize response recieved from the bot
        RootMessages recieveMessages =
            JsonUtility.FromJson<RootMessages>("{\"messages\":" + response + "}");

        // show message based on message type on UI
        foreach (RecieveData message in recieveMessages.messages)
        {
            FieldInfo[] fields = typeof(RecieveData).GetFields();
            foreach (FieldInfo field in fields)
            {
                string data = null;

                // extract data from response in try-catch for handling null exceptions
                try
                {
                    data = field.GetValue(message).ToString();
                }
                catch (NullReferenceException) { }

                // print data
                if (data != null && field.Name != "recipient_id")
                {
                    botUI.UpdateDisplay("bot", data, field.Name);
                }
            }
        }
    }

    /// <summary>
    /// This method gets url resource from link and applies it to the passed texture.
    /// </summary>
    /// <param name="url">url where the image resource is located</param>
    /// <param name="image">RawImage object on which the texture will be applied</param>
    /// <returns></returns>
    public IEnumerator SetImageTextureFromUrl(string url, Image image)
    {
        // Send request to get the image resource
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            // image could not be retrieved
            Debug.Log(request.error);

        else
        {
            // Create Texture2D from Texture object
            Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Texture2D texture2D = texture.ToTexture2D();


            // set max size for image width and height based on chat size limits
            float imageWidth = 0, imageHeight = 0, texWidth = texture2D.width, texHeight = texture2D.height;
            if ((texture2D.width > texture2D.height) && texHeight > 0)
            {
                // Landscape image
                imageWidth = texWidth;
                // Landscape image
                imageWidth = texWidth;
                if (imageWidth > 600) imageWidth = 600;
                float ratio = texWidth / imageWidth;
                imageHeight = texHeight / ratio;
            }
            if ((texture2D.width < texture2D.height) && texWidth > 0)
            {
                // Portrait image
                imageHeight = texHeight;
                if (imageHeight > 600) imageHeight = 600;
                float ratio = texHeight / imageHeight;
                imageWidth = texWidth / ratio;
            }

            // Resize texture to chat size limits and attach to message
            // Image object as sprite
            TextureScale.Bilinear(texture2D, (int)imageWidth, (int)imageHeight);
            image.sprite = Sprite.Create(
                texture2D,
                new Rect(0.0f, 0.0f, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f), 100.0f);
            
            // Resize and reposition all chat bubbles
            StartCoroutine(botUI.RefreshChatBubblePosition());
        }
    }
    /* TEMPORALMENTE DEPRECATED YA QUE SE ADUEÑA DEL ENTER Y ESO NO ESTA CHIDO
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToRasa();
        }
    } */
//}*/

using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// This class handles all the network requests and serialization/deserialization of data
/// </summary>
/// 
public class PostMessage
{
    public string message;
    public string sender;
}

/// <summary>
/// This class is used to deserialize the resonse json for each
/// individual message.
/// </summary>
[Serializable]
public class RecieveData
{
    public string recipient_id;
    public string text;
    public string image;
    public string attachemnt;
    public string buttons;
    public string element;
    public string quick_replie;
}

/// <summary>
/// This class is a wrapper for individual messages sent by the bot.
/// </summary>
[Serializable]
public class RootMessages
{
    public RecieveData[] messages;
}

[Serializable]
public class RecieveDataButtons
{
    public string title;
    public string payload;

}

/// <summary>
/// This class is a wrapper for individual messages sent by the bot.
/// </summary>
[Serializable]
public class RootMessagesButtons
{
    public RecieveDataButtons[] messages;
}

public class NetworkManager : MonoBehaviour
{

    // reference to BotUI class
    public BotUI botUI;
    public int i = 0;
    // the url at which bot's custom connector is hosted
    private const string rasa_url = "https://architecture-chatbot.herokuapp.com/webhooks/rest/webhook";

    /// <summary>
    /// This method is called when user has entered their message and hits the send button.
    /// It calls the <see cref="NetworkManager.PostRequest(string, string)"> coroutine to send
    /// the user message to the bot and also updates the UI with user message.
    /// </summary>
    public void SendMessageToRasa()
    {
        // get user messasge from input field, create a json object 
        // from user message and then clear input field
        string message = botUI.input.text;
        botUI.input.text = "";

        PostMessage postMessage = new PostMessage
        {
            sender = "24",
            message = message
        };
        string jsonBody = JsonUtility.ToJson(postMessage);

        // update UI object with user message
        botUI.UpdateDisplay("user", message, "text");

        // Create a post request with the data to send to Rasa server
        StartCoroutine(PostRequest(rasa_url, jsonBody));
    }

    /// <summary>
    /// This is a coroutine to asynchronously send a POST request to the Rasa server with 
    /// the user message. The response is deserialized and rendered on the UI object.
    /// </summary>
    /// <param name="url">the url where Rasa server is hosted</param>
    /// <param name="jsonBody">user message serialized into a json object</param>
    /// <returns></returns>
    private IEnumerator PostRequest(string url, string jsonBody)
    {
        // Create a request to hit the rasa custom connector
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] rawBody = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawBody);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // receive the response
        yield return request.SendWebRequest();

        // Render the response on UI object
        RecieveMessage(request.downloadHandler.text);
    }


    /// <summary>
    /// This method updates the UI object with bot response
    /// </summary>
    /// <param name="response">response json recieved from the bot</param>
    public void RecieveMessage(string response)
    {
        // Deserialize response recieved from the bot

        RootMessages recieveMessages = JsonUtility.FromJson<RootMessages>("{\"messages\":" + response + "}");
        //Debug.Log(response[response.IndexOf("buttons")+9]);


        // show message based on message type on UI
        foreach (RecieveData message in recieveMessages.messages)
        {

            FieldInfo[] fields = typeof(RecieveData).GetFields();
            foreach (FieldInfo field in fields)
            {
                String data = null;

                // extract data from response in try-catch for handling null exceptions
                try
                {

                    data = field.GetValue(message).ToString();

                }
                catch (NullReferenceException) { }

                // print data
                if (data != null && field.Name != "recipient_id")
                {
                    if (field.Name == "buttons" && response.Contains("buttons"))
                    {
                        int i = response.IndexOf("buttons") + 9;

                        //Debug.Log(response[response.IndexOf("]")]);
                        response = response.Substring(i);
                        int j = response.IndexOf("]") + 1;
                        response = response.Substring(0, j);

                        RootMessagesButtons recieveMessagesButtons = JsonUtility.FromJson<RootMessagesButtons>("{\"messages\":" + response + "}");

                        foreach (RecieveDataButtons msg in recieveMessagesButtons.messages)
                        {
                            FieldInfo[] fields_2 = typeof(RecieveDataButtons).GetFields();
                            data = "";
                            foreach (FieldInfo field_2 in fields_2)
                            {
                                data += field_2.GetValue(msg).ToString();
                            }
                            botUI.UpdateDisplay("bot", data, field.Name);
                        }

                    }
                    else
                    {
                        botUI.UpdateDisplay("bot", data, field.Name);
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method gets url resource from link and applies it to the passed texture.
    /// </summary>
    /// <param name="url">url where the image resource is located</param>
    /// <param name="image">RawImage object on which the texture will be applied</param>
    /// <returns></returns>
    public IEnumerator SetImageTextureFromUrl(string url, Image image)
    {
        // Send request to get the image resource
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            // image could not be retrieved
            Debug.Log(request.error);

        else
        {
            // Create Texture2D from Texture object
            Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Texture2D texture2D = texture.ToTexture2D();


            // set max size for image width and height based on chat size limits
            float imageWidth = 0, imageHeight = 0, texWidth = texture2D.width, texHeight = texture2D.height;
            if ((texture2D.width > texture2D.height) && texHeight > 0)
            {
                // Landscape image
                imageWidth = texWidth;
                // Landscape image
                imageWidth = texWidth;
                if (imageWidth > 600) imageWidth = 600;
                float ratio = texWidth / imageWidth;
                imageHeight = texHeight / ratio;
            }
            if ((texture2D.width < texture2D.height) && texWidth > 0)
            {
                // Portrait image
                imageHeight = texHeight;
                if (imageHeight > 600) imageHeight = 600;
                float ratio = texHeight / imageHeight;
                imageWidth = texWidth / ratio;
            }

            // Resize texture to chat size limits and attach to message
            // Image object as sprite
            TextureScale.Bilinear(texture2D, (int)imageWidth, (int)imageHeight);
            image.sprite = Sprite.Create(
                texture2D,
                new Rect(0.0f, 0.0f, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f), 100.0f);

            // Resize and reposition all chat bubbles
            StartCoroutine(botUI.RefreshChatBubblePosition(false));
        }
    }
    public IEnumerator SetButton(string title, string command, Button button)
    {
        button.GetComponentInChildren<Text>().text = title;
        button.transform.localPosition = new Vector3(70, 0, 0);
        button.onClick.AddListener(delegate () { sendMessageButton(command); });
        StartCoroutine(botUI.RefreshChatBubblePosition(true));
        yield return 0;
    }


    public void sendMessageButton(string command)
    {
        PostMessage postMessage = new PostMessage
        {
            sender = "user",
            message = command

        };
        Debug.Log(command);
        string jsonBody = JsonUtility.ToJson(postMessage);

        // update UI object with user message
        // botUI.UpdateDisplay("user", message, "text");

        // Create a post request with the data to send to Rasa server
        StartCoroutine(PostRequest(rasa_url, jsonBody));

    }
}