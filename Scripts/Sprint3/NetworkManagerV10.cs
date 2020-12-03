using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System;

public class NetworkManagerV10 : Mirror.NetworkManager
{
    [AddComponentMenu("")]
    //[SyncVar] static int identificator = 0;
    private List<int> numbers = new List<int>();

    public GameObject prefabHombre;
    public GameObject prefabMujer;
    public bool spawnHombre;
    public Transform posicionSpawn;
    public SelectorModelo selectorModelo;
    public GameObject modeloManager;

    public GameObject bot;

    public GameObject botRama;

    public GameObject ProcessObject;


    List<NetworkConnection> connectedPlayers;

    GameObject ChatObserver;

    GameObject CalendarAdministrator;


    public override void OnStartServer()
    {
        base.OnStartServer();
        /* parte del bot de rama
        Vector3 startPosition = new Vector3(-5.12f, -1.107403f, 13.36f);
        Quaternion startRotation = new Quaternion(0, 145, 0, 0);
        bot = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ScrumBot2"), startPosition, startRotation);
        bot.GetComponent<NavMeshAgent>().enabled = false;
        NetworkServer.Spawn(bot);
        bot.GetComponent<NavMeshAgent>().enabled = true;
        */
        bot = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ScrumBot"));
        NetworkServer.Spawn(bot);

        ChatObserver = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ChatObserver"));
        NetworkServer.Spawn(ChatObserver);

        CalendarAdministrator = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "CalendarAdministrator"));
        NetworkServer.Spawn(CalendarAdministrator);

        SpawnServerOnlyObjects();

        connectedPlayers = new List<NetworkConnection>();
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        /*   LO COMENTE PORQUE AL HACER EL MERGE CON la escena NPC NO PUEDO AHCER QUE EL SCRIPT PLAYER IDENTIFICATOR NO ME TRAIGA UN NETWORK IDENTITY AL OBJETO NETWORK MANAGER
        // add player at correct spawn position
        GameObject player = Instantiate(playerPrefab);
        int newId = GetComponent<PlayerAdministrator>().NewPlayerId();
        player.GetComponent<PlayerIdentificatorScript>().idOfPlayer = newId;
        player.GetComponentInChildren<GetPlayerName>().SetPlayerName(Convert.ToString(newId));
        numbers.Add(newId);
        GetComponent<PlayerAdministrator>().RegisterNewPlayer(player,newId);
        //identificator++;
        NetworkServer.AddPlayerForConnection(conn, player);
        /*print("mostrando id de jugadores conectados");
        foreach (int el in numbers) {
            print(" ");
            print(el);
        }*/

        spawnHombre = selectorModelo.getSpawnHombre();
        if (spawnHombre)
        {
            Transform startPos = posicionSpawn; //lineas añadidas
            GameObject player = startPos != null
                ? Instantiate(prefabHombre, startPos.position, startPos.rotation)
                : Instantiate(prefabHombre);
            player.GetComponent<ChatBehaviour>().ChatObserver = ChatObserver; //linea añadida
            player.GetComponent<DictationEngine>().ChatObserver = ChatObserver; //linea añadida
            NetworkServer.AddPlayerForConnection(conn, player);
        }
        else
        {
            Transform startPos = posicionSpawn; //lineas añadidas
            GameObject player = startPos != null
                ? Instantiate(prefabMujer, startPos.position, startPos.rotation)
                : Instantiate(prefabMujer);
            player.GetComponent<ChatBehaviour>().ChatObserver = ChatObserver; //linea añadida
            player.GetComponent<DictationEngine>().ChatObserver = ChatObserver; //linea añadida
            NetworkServer.AddPlayerForConnection(conn, player);
        }

        modeloManager.SetActive(false);

        /*Transform startPos = GetStartPosition(); //lineas añadidas
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        player.GetComponent<PlayerIdentificatorScript>().idOfPlayer = connectedPlayers.Count;
        player.GetComponent<ChatBehaviour>().ChatObserver = ChatObserver; //linea añadida
        player.GetComponent<DictationEngine>().ChatObserver = ChatObserver; //linea añadida
        player.GetComponent<SyncPlayerInfo>().playerName = PlayerInfo.playerName;
        NetworkServer.AddPlayerForConnection(conn, player);*/

        //base.OnServerAddPlayer(conn); linea comentada //Si no se hiciera lo de las lineas 71 a 78 habria que descomentarlo

        connectedPlayers.Add(conn);

        Debug.Log("Los jugadores conectados son:");
        for (int i = 0; i < connectedPlayers.Count; i++)
            Debug.Log(connectedPlayers[i].identity.gameObject.GetComponent<SyncPlayerInfo>().playerName);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        connectedPlayers.Remove(conn);
        base.OnServerDisconnect(conn);

        Debug.Log("Los jugadores conectados son:");
        for (int i = 0; i < connectedPlayers.Count; i++)
            Debug.Log(connectedPlayers[i].identity.gameObject.GetComponent<PlayerIdentificatorScript>().idOfPlayer);
    }

    [Server]
    public void SpawnServerOnlyObjects()
    {
        ProcessObject = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ProcesosObject"));
        NetworkServer.Spawn(ProcessObject);

        bot.GetComponentInChildren<PruebaChat>().processObject = ProcessObject;
        ProcessObject.GetComponentInChildren<GeneradorLista>().bot = bot;



        ChatObserver.GetComponentInChildren<ChatObserverScript>().CalendarAdministrator = CalendarAdministrator;
    }


}
