using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muteo : MonoBehaviour
{
    [SerializeField] private GameObject onIcon;
    [SerializeField] private GameObject offIcon;
    private bool on = true;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            if(on)
            {
                offIcon.SetActive(true);
                onIcon.SetActive(false);
                on = false;
            }
            else
            {
                onIcon.SetActive(true);
                offIcon.SetActive(false);
                on = true;
            }
        }

    }
}
