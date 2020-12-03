using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System;

public class NetworkManagerV6 : Mirror.NetworkManager
{
    [AddComponentMenu("")]
    //[SyncVar] static int identificator = 0;
    private List<int> numbers = new List<int>();

    public GameObject bot;

    public GameObject botRama;

    public GameObject ProcessObject;

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

        SpawnServerOnlyObjects();
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

        base.OnServerAddPlayer(conn);
            
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }

    [Server]
    public void SpawnServerOnlyObjects ()
    {
        ProcessObject = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ProcesosObject"));
        NetworkServer.Spawn(ProcessObject);

        bot.GetComponentInChildren<PruebaChat>().processObject = ProcessObject;
        ProcessObject.GetComponentInChildren<GeneradorLista>().bot = bot;
    }
}