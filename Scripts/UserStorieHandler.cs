using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserStorieHandler : MonoBehaviour
{
    [SerializeField] private GameObject userStoriePrefab;

    [SerializeField] private GameObject anchor;

    private GameObject userStorie;
    // Start is called before the first frame update
    void Start()
    {
        userStorie = Instantiate(userStoriePrefab);
        userStorie.transform.SetParent(this.transform,false);
        //userStorie.transform.SetParent(transform, false);
        userStorie.GetComponent<DragDrop>().setAnchor(anchor);
        userStorie.transform.position = transform.position;
        userStorie.GetComponent<DragDrop>().setCanvas(transform.parent.gameObject);
        userStorie.GetComponent<DragDrop>().listContent = null;
        print("instancia de user storie");
    }

    public void initiateNewUserStorie()
    {
            Start();

    }

    public void instantiateUserStory(string texto, Color32 color, Transform parent, Transform oldListContent)
    {
        if(oldListContent != null)
        {
            Debug.LogError(oldListContent.name);
            oldListContent = oldListContent.Find("Viewport").GetChild(0);
            for (int i = 0; i < oldListContent.childCount; i++)
            {
                GameObject child = oldListContent.GetChild(i).gameObject;
                if (child.GetComponentInChildren<TextMeshProUGUI>().text.Equals(texto) && child.GetComponent<Image>().color == color)
                {
                    Destroy(child);
                    break;
                }
            }   
        }

        Start();
        userStorie.GetComponentInChildren<TextMeshProUGUI>().text = texto;
        userStorie.GetComponent<Image>().color = color;
        userStorie.transform.SetParent(parent, false);
        userStorie.GetComponent<DragDrop>().listContent = parent.parent.parent;

        if(transform.childCount > 1)
            Destroy(transform.GetChild(0).gameObject);
    }
}
