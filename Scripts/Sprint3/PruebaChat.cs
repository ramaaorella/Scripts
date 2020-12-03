using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PruebaChat : MonoBehaviour
{
    public ChatBehaviour chat;
    private float timeRemaining = 30.0f;
    private float timeToNotifyAllPlayers = 30.0f;
    public bool allPlayersNotified = false;
    public RasaCommunication rasa;
    public GameObject processObject;

    private IEnumerator coroutine;

    private string message;

    string[] arrnombres = new string[6];

    int jugadorQueHabla = 0;

    bool hasEnded = false;

    List<string> messageList = new List<string>();

    List<Proceso> listaDeProcesos = new List<Proceso>();

    List<double> listOfTimes = new List<double>();

    int currentTask = 0;

    double currentTimer = 0.0d;

    bool thereIsAnAgenda = false;

    void Start()
    {
        messageList = new List<string>();

        arrnombres[0] = "Juan";
        arrnombres[1] = "Joaquin";
        arrnombres[2] = "Ezequiel";

        //coroutine = waitForResponse();
        //StartCoroutine(coroutine);

        //meetingWarningTimer();
    }

    public void updateList()
    {

        List<Proceso> otraListaDeProcesos = processObject.GetComponentInChildren<GeneradorLista>().getListaDeProcesos();

        listaDeProcesos.Clear();

        for (int i = 0; i < otraListaDeProcesos.Count; i++)
        {
            listaDeProcesos.Add(otraListaDeProcesos[i]);
        }

        

        for (int i = 0; i<listaDeProcesos.Count;i++)
        {
            Debug.Log("ESTE MENSAJE VIENE DE PRUEBA CHAT OJITO " +listaDeProcesos[i].tarea);
        }

        Debug.Log(listaDeProcesos.Count);

        for (int i = 0; i < listaDeProcesos.Count; i++)
        {
            double currentTaskTime = 0.0d;

            switch (listaDeProcesos[i].duracion)
            {
                case "15 minutos":
                    currentTaskTime = 15.0d;
                    break;
                case "30 minutos":
                    currentTaskTime = 30.0d;
                    break;
                case "1 horas":
                    currentTaskTime = 60.0d;
                    break;
                case "2 horas":
                    currentTaskTime = 120.0d;
                    break;
                case "4 horas":
                    currentTaskTime = 240.0d;
                    break;
                default:
                    currentTaskTime = 5.0d;
                    break;
            }
            listOfTimes.Add(currentTaskTime);
        }

        thereIsAnAgenda = true;

    }

    void Update()
    {
        if (thereIsAnAgenda)
        {
            if (currentTimer < 0.0d)
            {
                Debug.Log(currentTask);
                Debug.Log(listaDeProcesos.Count);
                if (currentTask < listaDeProcesos.Count)
                {
                    ////////////////////////////////////////////////////////////
                    if (listaDeProcesos[currentTask].tarea == "Daily Meeting")
                    {
                        dailyMeeting();
                        //chat.RpcHandleMessage("[ScrumMaster]: Prueba");
                        currentTimer = listOfTimes[currentTask];
                        currentTask++;
                    }
                    ///////////////////////////////////////////////////////////
                    else
                    {
                        chat.RpcHandleMessage("[Aviso]: Comienza la tarea " + listaDeProcesos[currentTask].tarea + " de duración estimada " + listaDeProcesos[currentTask].duracion + ".");
                        currentTimer = listOfTimes[currentTask];
                        currentTask++;
                    }
                }
                else
                {
                    thereIsAnAgenda = false;
                    chat.RpcHandleMessage("[Aviso]: No hay mas tareas en la agenda, fin del dia");
                }
            }
            else
                currentTimer = currentTimer - Time.deltaTime;
        }
        
       
    }

    private IEnumerator waitForResponse(string sender, string message)
    {
        Debug.Log("waitforresponse");
        rasa.sender = sender;
        rasa.message = message;
        rasa.SendMessageToRasa();
        while (rasa.respuesta == "")
        {
            yield return null;
        }
        //chat.RpcHandleMessage("[Scrumbot]: Hola! la daily meeting está por empezar. Porfavor entren a la sala de conferencias.");
        chat.RpcHandleMessage("[Aviso]:" + rasa.respuesta + "\n");
        gameObject.GetComponent<PopUpText>().SetAnswer(rasa.respuesta);
        //messageList.Add(rasa.respuesta);
        rasa.respuesta = "";
    }

    void dailyMeeting()
    {
        chat.RpcHandleMessage("[Aviso]: Hola! la daily meeting está por empezar. Porfavor entren a la sala de conferencias." + "\n");
        coroutine1 = wait();
        StartCoroutine(coroutine1);
        //wait();
        chat.RpcHandleMessage("[Aviso]: Le toca hablar a : Juan");
        coroutine2 = wait_1minuto();
        StartCoroutine(coroutine2);
        //wait_1minuto();
        chat.RpcHandleMessage("[Aviso]: Termino tu tiempo");
        StartCoroutine(coroutine1);
        //wait();
        chat.RpcHandleMessage("[Aviso]: Le toca hablar a : Joaquin");
        StartCoroutine(coroutine2);
        //wait_1minuto();
        chat.RpcHandleMessage("[Aviso]: Termino tu tiempo");
        StartCoroutine(coroutine1);
        //wait();
        chat.RpcHandleMessage("[Aviso]: Le toca hablar a : Ezequiel");
        StartCoroutine(coroutine2);
        //wait_1minuto();
        chat.RpcHandleMessage("[Aviso]: Termino tu tiempo");
        StartCoroutine(coroutine1);
        //wait();
        chat.RpcHandleMessage("[Aviso]:" + "Termino la reunion, sigan adelante." + "\n");
    }

    private IEnumerator coroutine1;
    private IEnumerator coroutine2;

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(5f);
    }

    private IEnumerator wait_1minuto()
    {
        yield return new WaitForSeconds(60f);
    }

}

    /*
    private IEnumerator wait() { 
        yield return new WaitForSeconds(5f);
    }

    private void meetingWarningTimer()
    {
        System.Timers.Timer aTimer = new System.Timers.Timer();
        aTimer.Elapsed += new ElapsedEventHandler(meetingWarning);
        aTimer.Interval = 1000;
        aTimer.Enabled = true;

    }

    void meetingWarning(object source, ElapsedEventArgs e)
    {
        Debug.Log("meetingwarning");
        message = "Reunion 5";
        coroutine = waitForResponse("", message);
        StartCoroutine(coroutine);
    } */


//public ChatBehaviour chat;
//private float timeRemaining = 30.0f;
//private float timeToNotifyAllPlayers = 30.0f;
//public bool allPlayersNotified = false;
//public RasaCommunication rasa;

//private IEnumerator coroutine;

//private string message;

//string[] arrnombres = new string[6];

//int jugadorQueHabla = 0;

//bool hasEnded = false;

//List<string> messageList;

//void Start()
//{
//    messageList = new List<string>();

//    arrnombres[0] = "Juan";
//    arrnombres[1] = "Joaquin";
//    arrnombres[2] = "Ezequiel";

//    //coroutine = waitForResponse();
//    //StartCoroutine(coroutine);

//    //meetingWarningTimer();
//}

//private IEnumerator waitForResponse(string sender, string message)
//{
//    Debug.Log("waitforresponse");
//    rasa.sender = sender;
//    rasa.message = message;
//    rasa.SendMessageToRasa();
//    while (rasa.respuesta == "")
//    {
//        yield return null;
//    }
//    //chat.RpcHandleMessage("[Scrumbot]: Hola! la daily meeting está por empezar. Porfavor entren a la sala de conferencias.");
//    chat.RpcHandleMessage("[ScrumMaster]:" + rasa.respuesta + "\n");
//    gameObject.GetComponent<PopUpText>().SetAnswer(rasa.respuesta);
//    //messageList.Add(rasa.respuesta);
//    rasa.respuesta = "";
//}

///*
//private IEnumerator wait() { 
//    yield return new WaitForSeconds(5f);
//}

//private void meetingWarningTimer()
//{
//    System.Timers.Timer aTimer = new System.Timers.Timer();
//    aTimer.Elapsed += new ElapsedEventHandler(meetingWarning);
//    aTimer.Interval = 1000;
//    aTimer.Enabled = true;

//}

//void meetingWarning(object source, ElapsedEventArgs e)
//{
//    Debug.Log("meetingwarning");
//    message = "Reunion 5";
//    coroutine = waitForResponse("", message);
//    StartCoroutine(coroutine);
//} */
//void Update()
//{

//    if (!allPlayersNotified)
//    {
//        if (timeToNotifyAllPlayers > 0)
//        {
//            timeToNotifyAllPlayers = timeToNotifyAllPlayers - Time.deltaTime;
//        }
//        else
//        {

//            message = "Reunion 1";
//            coroutine = waitForResponse("", message);
//            StartCoroutine(coroutine);
//            //rasa.SendMessageToRasa();

//            //chat.RpcHandleMessage("[Scrumbot]: Hola! la daily meeting está por empezar. Porfavor entren a la sala de conferencias.");
//            //chat.RpcHandleMessage(rasa.respuesta);
//            allPlayersNotified = true;
//        }
//    }
//    if (allPlayersNotified && !hasEnded)
//    {
//        if (timeRemaining > 0)
//        {
//            timeRemaining = timeRemaining - Time.deltaTime;
//        }
//        else
//        {
//            if (jugadorQueHabla != 0)
//            {
//                //wait();
//                //message = "Termino tu tiempo";
//                //coroutine = waitForResponse(arrnombres[jugadorQueHabla-1], message);
//                //StartCoroutine(coroutine);
//                //wait();
//            }

//            if (jugadorQueHabla == 3)
//            {
//                //wait();
//                chat.RpcHandleMessage("[ScrumMaster]:" + "Termino la reunion, sigan adelante." + "\n");
//                hasEnded = true;
//            }
//            else
//            {
//                //wait();
//                message = "Permitido hablar 1 minuto";
//                coroutine = waitForResponse(arrnombres[jugadorQueHabla], message);
//                StartCoroutine(coroutine);
//                //wait();
//                jugadorQueHabla++;
//            }
//            //chat.RpcHandleMessage("[Scrumbot]: Comenzando la daily meeting. ");
//            //chat.RpcHandleMessage("[Scrumbot]: Le toca hablar a : Juan ");
//            timeRemaining = 30.0f;
//        }
//    }
//}