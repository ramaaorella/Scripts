using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EraseDrop : MonoBehaviour , IDropHandler
{
    public FollowCursor userStoryAnchor;

    [SerializeField] private GameObject userStorieHandler;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag.tag == "USER_STORIE")
            if (eventData.pointerDrag != null)
            {
                if (eventData.pointerDrag.GetComponent<DragDrop>().getIsFirstDrop())
                    userStorieHandler.GetComponent<UserStorieHandler>().initiateNewUserStorie();

                //GameObject listcontent = transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;

                /*if (eventData.pointerDrag.transform.parent == userStorieHandler.transform)
                    userStorieHandler.GetComponent<UserStorieHandler>().initiateNewUserStorie();*/

                Destroy(eventData.pointerDrag);

                userStoryAnchor.setActualizar(true);

            }
    }
}
