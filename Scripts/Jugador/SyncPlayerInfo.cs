using UnityEngine;
using Mirror;
using TMPro;

public class SyncPlayerInfo : NetworkBehaviour
{
    [SyncVar] public string playerName;
    [SyncVar] public int rol;
    
    private Transform labelHolder;

    private void Awake()
    {
        labelHolder = transform.Find("LabelHolder");
    }

    public override void OnStartAuthority()
    {
        if (!hasAuthority) { return; }
        SetPlayerInfo();
    }

    void SetPlayerInfo()
    {
        this.playerName = PlayerInfo.playerName;
        this.rol = PlayerInfo.rol;

        CmdSendRolToServer(this.playerName, this.rol);

        // Se añade a la lista de jugadores conectados
        CurrentPlayers.playersList.Add(gameObject);
    }

    [Command(ignoreAuthority = true)]
    void CmdSendRolToServer(string playerName, int rol)
    {       
        foreach (GameObject player in CurrentPlayers.playersList)
        {
            SyncPlayerInfo playerInfo = player.GetComponent<SyncPlayerInfo>();
            player.GetComponent<SyncPlayerInfo>().RcpSetPlayerRol(playerInfo.playerName, playerInfo.rol);
        }

        RcpSetPlayerRol(playerName, rol);
    }

    [ClientRpc]
    void RcpSetPlayerRol(string playerName, int rol)
    {
        labelHolder.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = playerName;
        string rolName = "";
        switch(rol)
        {
            case PlayerInfo.ProductOwner:
                rolName = "Product Owner";
                break;
            case PlayerInfo.ScrumMaster:
                rolName = "Scrum Master";
                break;
            case PlayerInfo.ScrumMember:
                rolName = "Scrum Member";
                break;
        }
        labelHolder.Find("RolName").GetComponent<TextMeshProUGUI>().text = rolName;
    }

    // Se mantiene funcionalidad para Rasa    
    private int cantJugadores = 0;
    public string getID()
    {
        cantJugadores++;
        return cantJugadores + "";
    }

}
