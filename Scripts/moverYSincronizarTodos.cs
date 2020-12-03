using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moverYSincronizarTodos : NetworkBehaviour
{
   public void modifYSincronizarTodosFunc(GameObject elcuboencuestion)
   {
        
        if (elcuboencuestion != null)
        {
            print("moverYsincronziar");
            CmdAgregarCanvas(elcuboencuestion);

        }

    }

   [ClientRpc]
   void RpcMoveCube(GameObject obj)
   {
        obj.GetComponentInChildren<AñadirObjetoALista>().ClickBotonAñadir();
   }

   [Command]
   void CmdAgregarCanvas(GameObject obj)
   {
        print("CmdAgregarCanvasFUERA");
        GameObject theCubeFromTheScene = GameObject.Find("MyCanvasToModify(Clone)");
        theCubeFromTheScene.GetComponentInChildren<AñadirObjetoALista>().ClickBotonAñadir();
        /*if (obj != null)
        {
            print("CmdAgregarCanvas");
            NetworkIdentity objNetId;
            objNetId = obj.GetComponent<NetworkIdentity>();        // get the object's network ID
            objNetId.AssignClientAuthority(connectionToClient);    // assign authority to the player who is changing the color
            //RpcMoveCube(obj);                                    // usse a Client RPC function to "paint" the object on all clients
                // remove the authority from the player who changed the color
            obj.GetComponentInChildren<AñadirObjetoALista>().ClickBotonAñadir();
            objNetId.RemoveClientAuthority();
        }
       */
    }
   
}
