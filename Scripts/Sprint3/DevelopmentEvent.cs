using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;


public class DevelopmentEvent : MonoBehaviour
{
    public RasaCommunication rasa;
    string receivedMessageFromObserver;
    bool specialEvent = false;

    GameObject cas;
    string nameOfEvent = "DEFAULT_EVENT_NAME";
    int eventHour = 0;
    int eventMinute = 0;
    string eventDay;
    double durationOfEvent = 0; //Rasa nos lo da en minutos

    bool conversionError = false;


    double countdown = -1.0d;

    bool timerWasScheduled = false;

    bool timerWasSetOff = false;

    bool hasEnded = false;

    public void setEventData(GameObject cas,string eventCommand)
    {
        this.cas = cas;
        retrieveDataFromCommand(eventCommand,ref this.nameOfEvent, ref this.eventDay, ref this.eventHour,ref this.eventMinute,ref this.durationOfEvent);
        if (!conversionError)
            schedule();
        else
            Debug.Log("Hubo un error en la recuperacion de datos del comando de rasa para programar el evento (¿La informacion provista es correcta? " +
                "¿El formato del comando esta bien manejado?)");

    }

    void retrieveDataFromCommand(string eventCommand, ref string nameOfEvent, ref string eventDay, ref int eventHour, ref int eventMinute, ref double durationOfEvent)
    {
        string[] dataArray = new string[4];

        int start = 0;

        int end = 0;

        int currentData = 0;

        int i = 0;

        while (i < eventCommand.Length)
        {
            if (eventCommand[i] == ':')
            {
                start = i;
                while (i < eventCommand.Length && eventCommand[i] != '_')
                {
                    i++;
                }
                end = i;
                //Debug.Log("start" + start);
                //Debug.Log("End" + end);
                if (start + 1 < end - 1)
                    dataArray[currentData] = eventCommand.Substring(start + 1, (end - 1) - (start + 1) + 1);
                else
                    dataArray[currentData] = "none"; //En caso de que el campo no esté especificado, por defecto tenemos el string none

                currentData++;
            }
            else
                i++;
        }

        //Se asignan los valores de las variables para los distintos datos del evento
        //En caso de que el valor no esté definido, el valor por defecto es 0
        //Se verifica que en las conversiones de string a los tipos de datos correspondientes no ocurra ningun error
        //con la variable errorConverting

        if (dataArray[0] == "none")
            nameOfEvent = "evento";
        else
            nameOfEvent = dataArray[0];

        //Debug.Log("Nombre del evento: "+nameOfEvent);

        if (dataArray[1] == "none")
            eventDay = "hoy";
        else
            eventDay = dataArray[1];

        //Debug.Log("Dia del evento: "+eventDay);

        if (dataArray[2] == "none") 
        { 
            eventHour = DateTime.Now.Hour;
            eventMinute = DateTime.Now.Minute; 
        }
        else
        {
            //Debug.Log(dataArray[2].Substring(0, 2));
            //Debug.Log(dataArray[2].Substring(3, 2));
            conversionError = !(Int32.TryParse(dataArray[2].Substring(0,2), out eventHour));
            conversionError = !(Int32.TryParse(dataArray[2].Substring(3,2), out eventMinute));

        }


        //Debug.Log("Hora de activacion del evento: "+eventHour);
        //Debug.Log("Minuto de activacion del evento: " + eventMinute);

        if (dataArray[3] == "none")
            durationOfEvent = 0;
        else
            conversionError = !(Double.TryParse(dataArray[3], out durationOfEvent));


        //Debug.Log("Duracion del evento: "+durationOfEvent);

        if (nameOfEvent=="reunion" || nameOfEvent=="reunión") specialEvent = true;
    }


    void Update()
    {
        if (!hasEnded)
        {
            if (countdown > 0)
                countdown -= Time.deltaTime;
            else
            {
                if (timerWasScheduled && !timerWasSetOff)
                {
                    if (specialEvent)
                        StartCoroutine(specialSetOff());
                    setOff();
                }
                else
                {
                    endEvent();
                }
            }
        }
    }

    public void schedule()
    {

        timerWasScheduled = true;
        timerWasSetOff = false;

        DateTime eventStartDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,eventHour,eventMinute,0);

        countdown = (eventStartDate - DateTime.Now).TotalSeconds;

        cas.GetComponent<CalendarAdministratorScript>().sendMessageToGlobalChat("El evento " + this.nameOfEvent + " fue correctamente programado para el dia: " +
            ""+this.eventDay+" a la hora "+this.eventHour+":"+this.eventMinute+" con duracion "+this.durationOfEvent);
    }

    public void setOff()
    {

        timerWasSetOff = true;
        countdown = durationOfEvent * 60; //Como rasa nos da minutos, tenemos que convertirlo a segundos.
        cas.GetComponent<CalendarAdministratorScript>().sendMessageToGlobalChat("El evento: " + this.nameOfEvent + " esta comenzando con duracion aproximada " + this.durationOfEvent+" minutos.");

    }

    public void endEvent()
    {
        hasEnded = true;
        cas.GetComponent<CalendarAdministratorScript>().sendMessageToGlobalChat("El evento: " + this.nameOfEvent + " acaba de finalizar");
    }

    public void receiveMessage(string message)
    {
        receivedMessageFromObserver = message;
    }

    public IEnumerator specialSetOff()
    {
        //se avisa que esta empezando la reunion
        cas.GetComponent<CalendarAdministratorScript>().sendMessageToGlobalChat("Entro a special set off");

        for (int i=0;i<3;i++)
        {
            Debug.Log("Ciclo del for numero " + i);
            receivedMessageFromObserver = "";
            //Comienza la conversacion y se envia el primer mensaje de control a rasa
            string replyFromRasa = "";
            rasa.sender = "NombreDeUsuario";
            rasa.message = "Permiso para hablar";
            rasa.respuesta = "";
            rasa.SendMessageToRasa();

            //Tengo que esperar a recibir la respuesta de rasa
            yield return new WaitUntil(() => rasa.respuesta != "");
            replyFromRasa = rasa.respuesta;
            rasa.respuesta = "";
            Debug.Log("Respuesta de rasa " + replyFromRasa);
            //Cuando contiene !p significa que es una pregunta que debe ser mostrada en el chat/por voz
            while (replyFromRasa.Contains("!p_"))
            {
                cas.GetComponent<CalendarAdministratorScript>().sendMessageToGlobalChat(replyFromRasa.Substring(3));
                //Tengo que esperar a recibir la respuesta del individuo
                yield return new WaitUntil(() => receivedMessageFromObserver!="");
                rasa.message = receivedMessageFromObserver;
                //tiene que volver a estar vacio para la siguiente respuesta
                receivedMessageFromObserver = "";
                rasa.sender = "NombreDeUsuario";
                rasa.respuesta = "";
                rasa.SendMessageToRasa();
                //Se espera la respuesta de rasa
                yield return new WaitUntil(() => rasa.respuesta != "");
                replyFromRasa = rasa.respuesta;
                rasa.respuesta = "";
            }
            if (replyFromRasa.Contains("!t_"))
                if (i<2)
                    cas.GetComponent<CalendarAdministratorScript>().sendMessageToGlobalChat("Pasamos a charlar con el siguiente participante");
                else
                    endEvent();
                    
        }
    }

    public string GetEventName()
    {
        return nameOfEvent;
    }

    ////Debug.Log("EL CHAT OBSERVER RECIBIO EL MENSAJE: " + message);
    //rasa.sender = "";
    //    rasa.message = message;
    //    rasa.SendMessageToRasa();
    //    //Tengo que esperar a recibir la respuesta de rasa
    //    yield return new WaitUntil(() => rasa.respuesta != "");
    //replyFromRasa = rasa.respuesta;
    //    rasa.respuesta = "";

}