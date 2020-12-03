using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private bool actualizar;

    // Start is called before the first frame update
    void Start()
    {
        actualizar = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && actualizar)
        {
            Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            transform.position = newPos;
            actualizar = false;
        }
    }

    public void setActualizar(bool valor)
    {
        actualizar = valor;

        if (valor == true && transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject);
    }
}
