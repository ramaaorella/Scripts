using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;
using TMPro;

public class ReceiveDrop : NetworkBehaviour, IDropHandler
{
    public FollowCursor userStoryAnchor;

    [SerializeField] private GameObject userStorieHandler;
    [SerializeField] private GameObject listcontent;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "USER_STORIE")
        {

            //GameObject listcontent = transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;

            bool isFirstDrop = eventData.pointerDrag.GetComponent<DragDrop>().getIsFirstDrop();

            if (isFirstDrop)
                userStorieHandler.GetComponent<UserStorieHandler>().initiateNewUserStorie();

            /*if (eventData.pointerDrag.transform.parent==userStorieHandler.transform)
            userStorieHandler.GetComponent<UserStorieHandler>().initiateNewUserStorie();

            eventData.pointerDrag.transform.SetParent(listcontent.transform,false);*/          

            string userStorieText = eventData.pointerDrag.GetComponentInChildren<TextMeshProUGUI>().text;
            Color32 userStorieColor = eventData.pointerDrag.GetComponent<Image>().color;
            Transform oldListContent = eventData.pointerDrag.GetComponent<DragDrop>().listContent;

            CmdSendDropUserStorieEventToServer(isFirstDrop, userStorieText, userStorieColor, oldListContent);
        }
    }

    [Command(ignoreAuthority = true)]
    void CmdSendDropUserStorieEventToServer(bool isFirstDrop, string userStorieText, Color32 userStorieColor, Transform oldListContent)
    {
        RcpDropUserStorieEvent(isFirstDrop, userStorieText, userStorieColor, oldListContent);
    }

    [ClientRpc]
    void RcpDropUserStorieEvent(bool isFirstDrop, string userStorieText, Color32 userStorieColor, Transform oldListContent)
    {
        userStorieHandler.GetComponent<UserStorieHandler>().instantiateUserStory(userStorieText, userStorieColor, listcontent.transform, oldListContent);

        userStoryAnchor.setActualizar(true);
    }
}
