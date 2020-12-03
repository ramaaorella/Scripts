using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown("space")) {
                save();
            }
        }
    }

    ListaInfo listaMovs = new ListaInfo();

    public void addMove(string sala) {
        PlayerInfo datos = new PlayerInfo();
        datos.ID = int.Parse(GetComponent<SyncPlayerInfo>().getID());
        datos.sala = sala;
        listaMovs.movimientos.Add(datos);
    }

    public void save() {
        string json = JsonUtility.ToJson(listaMovs);
        Debug.Log("Se va a subir: " + json);
        File.WriteAllText(Application.persistentDataPath + "posPlayer" + GetComponent<SyncPlayerInfo>().getID() + ".json", json);
        listaMovs.movimientos.Clear();
    }

    [System.Serializable]
    private class PlayerInfo
    {
        public string sala;
        public int ID;
        //Dia y hora (DateTime)
    }

    [System.Serializable]
    private class ListaInfo {
        public List<PlayerInfo> movimientos = new List<PlayerInfo>();
    }
}
