using UnityEngine;
using Mirror;
using TMPro;
using System.Collections;

public class PopUpText : NetworkBehaviour
{
    [SyncVar]public string answer;

    private Transform popUp;

    private void Awake()
    {
        popUp = transform.Find("PopUpText");
    }

    IEnumerator Waiter(float duration)
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(duration);

        popUp.gameObject.SetActive(false);
        transform.Find("LabelHolder").gameObject.SetActive(true);
    }

    public void SetAnswer(string answer)
    {
        this.answer = answer;
        CmdSendAnswerToServer(this.answer);
        Debug.Log("set answer called: "+answer);
    }

    [Command(ignoreAuthority = true)]
    void CmdSendAnswerToServer(string answer)
    {
        RcpSetPlayerRol(answer);
    }

    [ClientRpc]
    void RcpSetPlayerRol(string answer)
    {
        popUp.GetChild(0).GetComponent<TextMeshProUGUI>().text = answer;
        popUp.gameObject.SetActive(true);
        transform.Find("LabelHolder").gameObject.SetActive(false);
        StartCoroutine(Waiter(4));
    }
}
