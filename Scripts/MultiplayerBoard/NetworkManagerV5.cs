using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerV5 : Mirror.NetworkManager
{
    [AddComponentMenu("")]

    public GameObject spawnedBoard;
    // Start is called before the first frame update

    public override void OnStartServer()
    {
        base.OnStartServer();

        spawnedBoard = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "MyCanvasToModify"));
        NetworkServer.Spawn(spawnedBoard);
    }
}
