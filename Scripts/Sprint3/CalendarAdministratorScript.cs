using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;


public class CalendarAdministratorScript : MonoBehaviour
{

    public ChatBehaviour chat;

    public GameObject eventPrefab;

    public List<GameObject> eventList;

    public CalendarAdministratorScript()
    {

    }

    public void Start()
    {
        eventList = new List<GameObject>();
    }

    public void sendMessageToGlobalChat(string message)
    {
        chat.RpcHandleMessage("[Aviso]:" + message + "\n");
        registerMessage();
    }

    private void registerMessage()
    {
        //Funcion para registrar toda la conversación por chat
    }



    public void receiveNewEventData(string command)
    {
        string eventCommand = command.Substring(2);

        GameObject newEvent = Instantiate(eventPrefab);

        newEvent.GetComponent<DevelopmentEvent>().setEventData(this.gameObject, eventCommand);

        newEvent.transform.parent = this.gameObject.transform;

        eventList.Add(newEvent);
    }

    public void propagateMessageToEvents(string message)
    {
        for (int i = 0; i < eventList.Count; i++)
            if (eventList[i].GetComponent<DevelopmentEvent>().GetEventName()=="reunion")
                eventList[i].GetComponent<DevelopmentEvent>().receiveMessage(message);
    }

}

