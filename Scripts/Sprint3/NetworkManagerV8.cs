using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System;

public class NetworkManagerV8 : Mirror.NetworkManager
{
    [AddComponentMenu("")]
    //[SyncVar] static int identificator = 0;
    private List<int> numbers = new List<int>();

    public GameObject bot;

    public GameObject botRama;

    public GameObject ProcessObject;


    List<NetworkConnection> connectedPlayers;

    GameObject ChatObserver;

    GameObject CalendarAdministrator;

    public Transform posicionSpawn;


    public override void OnStartServer()
    {
        base.OnStartServer();
        /* parte del bot de rama*/
        Vector3 startPosition = new Vector3(-4.402f, 0, 18.203f);
        bot = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ScrumBot3"));
        bot.GetComponent<NavMeshAgent>().enabled = false;
        bot.GetComponent<CapsuleCollider>().enabled = false;
        bot.transform.position = startPosition;
        bot.transform.rotation = new Quaternion(0, 180, 0, 0);
        NetworkServer.Spawn(bot);
        bot.GetComponent<NavMeshAgent>().enabled = true;
        bot.GetComponent<CapsuleCollider>().enabled = true;

        bot = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ScrumBot"), new Vector3(4.46f, 0, 30.49f), new Quaternion(0,0,0,0));
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

        Transform startPos = posicionSpawn; //lineas añadidas
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        player.GetComponent<PlayerIdentificatorScript>().idOfPlayer = connectedPlayers.Count;
        player.GetComponent<ChatBehaviour>().ChatObserver = ChatObserver; //linea añadida
        player.GetComponent<DictationEngine>().ChatObserver = ChatObserver; //linea añadida
        //player.GetComponent<SyncPlayerInfo>().playerName = PlayerInfo.playerName; //intentando solucionar la asignacion de nombres a los prefabs, no funciono.
        NetworkServer.AddPlayerForConnection(conn, player);

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
        for (int i = 0; i<connectedPlayers.Count;i++)
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
