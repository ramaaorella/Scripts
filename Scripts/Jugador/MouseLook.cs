using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using Mirror;

public class MouseLook : NetworkBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    public Transform playerCamera;//

    float xRotation = 0f;

    public bool canRotate = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!hasAuthority) { return; }
        Cursor.lockState = CursorLockMode.Locked;
    }
    [Client]
    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) { return; }

        if(canRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            if(Cursor.lockState == CursorLockMode.Confined)
            {
                mouseX *= 3.5f;
                mouseY *= 3.5f;
            }

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);//
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
