using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class NetworkManagerBot : Mirror.NetworkManager
{
    public GameObject bot;

    public override void OnStartServer()
    {
        base.OnStartServer();

        Vector3 startPosition = new Vector3(-5.12f, -1.107403f, 13.36f);
        Quaternion startRotation = new Quaternion(0, 145, 0, 0);
        bot = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "ScrumBot2"), startPosition, startRotation);
        bot.GetComponent<NavMeshAgent>().enabled = false;
        NetworkServer.Spawn(bot);
        bot.GetComponent<NavMeshAgent>().enabled = true;
    }
}
