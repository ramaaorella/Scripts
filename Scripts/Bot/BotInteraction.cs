using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BotInteraction : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    private Camera playerCamera;

    [SerializeField] private GameObject chatCanvas;
    public GameObject pizarracanvas;
    private InputField inputField;

    void Start()
    {
        if (playerCameraTransform.childCount != 0)
            playerCamera = playerCameraTransform.GetChild(0).GetComponent<Camera>();
        inputField = pizarracanvas.transform.Find("InputField").GetComponent<InputField>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "NPC")
                {
                    //hit.collider.gameObject.GetComponent<PopUpText>().SetAnswer("La sala de reuniones");

                    chatCanvas.SetActive(false);
                    pizarracanvas.SetActive(true);
                    gameObject.GetComponent<MouseLook>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    gameObject.GetComponent<CharacterController>().enabled = false;
                    EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
                    inputField.OnPointerClick(new PointerEventData(EventSystem.current));
                }

            }

        }

        if (Input.GetKeyDown(KeyCode.Escape)) CloseChat();
    }

    public void CloseChat()
    {
        chatCanvas.SetActive(true);
        pizarracanvas.SetActive(false);
        gameObject.GetComponent<MouseLook>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }
}