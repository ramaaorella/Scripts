using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotChat : MonoBehaviour
{
    [SerializeField] private Transform InputFieldCanvas;
    
    public void EnviarMensaje()
    {
        InputFieldCanvas.Find("InputField").GetComponent<InputField>().text = "";
        InputFieldCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnviarMensaje();
            InputFieldCanvas.parent.gameObject.GetComponent<BotInteraction>().CloseChat();
        }
    }
}
