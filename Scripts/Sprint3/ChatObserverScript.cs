using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatObserverScript : MonoBehaviour
{
    public RasaCommunication rasa;

    public GameObject CalendarAdministrator;

    string replyFromRasa;

    public void receiveChatMessage(string message)
    {
        StartCoroutine(receiveChatMessageC(message));
    }

    public IEnumerator receiveChatMessageC(string message)
    {
        CalendarAdministrator.GetComponent<CalendarAdministratorScript>().propagateMessageToEvents(message);
        //Debug.Log("EL CHAT OBSERVER RECIBIO EL MENSAJE: " + message);
        rasa.sender = "";
        rasa.message = message;
        rasa.SendMessageToRasa();
        //Tengo que esperar a recibir la respuesta de rasa
        yield return new WaitUntil(() => rasa.respuesta != "");
        replyFromRasa = rasa.respuesta;
        rasa.respuesta = "";

        Debug.Log("ESTA ES LA RESPUESTA DE RASA: " + replyFromRasa);

        //COMPROBACIONES DE QUE EL STRING DE RASA SEA UN COMANDO

        if (replyFromRasa.Contains("!c_"))
        {
            CalendarAdministrator.GetComponentInChildren<CalendarAdministratorScript>().receiveNewEventData(replyFromRasa);
        }
        //CALENDAR ADMINISTATOR SE ENCARGA DE UTILIZAR LOS CAMPOS COMO CORRESPONDE

    }

    private IEnumerator sendMessageToRasa(string sender, string message)
    {
        return null;
    }


    private string sendMessageToRasa(string message)
    {

        return "DEFAULTSTRING";
    }
}