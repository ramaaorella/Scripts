using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInteraction : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    private Camera playerCamera;

    private Transform canvas;

    void Start()
    {
        if (playerCameraTransform.childCount != 0)
            playerCamera = playerCameraTransform.GetChild(0).GetComponent<Camera>();

        canvas = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Canvas" && (hit.collider.gameObject.name == "Cork" || hit.collider.gameObject.name == "MonitorUsuariosInputField"))
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    canvas = hit.collider.transform.parent;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            canvas = null;
        }

        // Si el input field está activado, se impide el movimiento del player para impedir que se mueva mientras escriba
        if(canvas != null && canvas.GetComponentInChildren<InputField>().isFocused)
            gameObject.GetComponent<CharacterController>().enabled = false;

        if (canvas != null && !canvas.GetComponentInChildren<InputField>().isFocused)
            gameObject.GetComponent<CharacterController>().enabled = true;
    }

    void UnlockCharacterController()
    {
        gameObject.GetComponent<CharacterController>().enabled = true;
    }


    public void OpenCanvas(RaycastHit hit)
    {
        //gameObject.GetComponent<CharacterController>().enabled = false;
        //gameObject.GetComponent<MouseLook>().canRotate = false;
        //gameObject.transform.position = new Vector3(1.31f, 0, 2.29f);
        Cursor.lockState = CursorLockMode.Confined;
        //gameObject.transform.eulerAngles = new Vector3(0, 45.7f, 0);
    }

    public void CloseCanvas(RaycastHit hit)
    {
        //gameObject.GetComponent<CharacterController>().enabled = true;
        //gameObject.GetComponent<MouseLook>().canRotate = true;
        Cursor.lockState = CursorLockMode.Locked;

        
    }
}
