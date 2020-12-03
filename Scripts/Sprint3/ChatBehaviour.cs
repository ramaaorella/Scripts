//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;

public class ChatBehaviour : NetworkBehaviour
{
    [SerializeField] private GameObject canvasChat = null;
    [SerializeField] private TMP_Text textoChat = null;
    [SerializeField] private TMP_InputField inputField = null;

    private static event Action<string> OnMessage;

    public GameObject ChatObserver;

    public override void OnStartAuthority()
    {
        canvasChat.SetActive(true);
        OnMessage += HandleNewMessage;

        //obtener el observador de chat de la escena (es uno solo para todos).
        //ChatObserver = GameObject.Find("ChatObserver(Clone)"); //linea comentada


    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority) { return; }
        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message)
    {
        textoChat.text += message;
    }

    [Client]
    public void Send(string message)
    {
        if (!Input.GetKeyDown(KeyCode.Return)) { return; }
        if (string.IsNullOrWhiteSpace(message)) { return; }
        CmdSendMessage(message);
        inputField.text = string.Empty;
    }

    [Command]
    public void CmdSendMessage(string message)
    {
        //Cada mensaje que se envie enviarlo al observador de chat
        ChatObserver.GetComponent<ChatObserverScript>().receiveChatMessage(message);

        //Debug.Log("Llego al CmdSendMessage " + message);
        RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
    }

    [ClientRpc]
    public void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
}
