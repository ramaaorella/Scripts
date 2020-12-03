using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public const int ProductOwner = 0;
    public const int ScrumMaster = 1;
    public const int ScrumMember = 2;

    public static string playerName;
    public static int rol;

    public void SetName(string name)
    {
        playerName = name;
    }

    public void SetRol(int playerRol)
    {
        rol = playerRol;
    }

}
