using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField] private GameObject father;

	[SerializeField] private GameObject handler;

	[SerializeField] private GameObject canvas;

	private GameObject anchor;
	private RectTransform rectTransform;
	private CanvasGroup canvasGroup;

	private bool isFirstDrop = true;

	public Transform listContent;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("OnBeginDrag");
		//canvasGroup.alpha = .6f; para que se vea faded al arrastrar
		canvasGroup.blocksRaycasts = false;

		//transform.SetParent(father.transform, false);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Debug.Log("OnDrag");
		transform.SetParent(anchor.transform, false);
		rectTransform.anchoredPosition += eventData.delta / canvas.GetComponent<Canvas>().scaleFactor;
		//rectTransform.anchoredPosition += eventData.delta/* / canvas.GetComponent<Canvas>().scaleFactor*/;
		canvasGroup.blocksRaycasts = false;

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("OnEndDrag");
		//canvasGroup.alpha = 1f;para que deje de verse faded al arrastrar
		canvasGroup.blocksRaycasts = true;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("On pointer down");
	}

	public void setCanvas(GameObject canvas)
    {
		this.canvas = canvas;
    }

	public bool getIsFirstDrop()
    {
		if (isFirstDrop)
        {
			this.isFirstDrop = false;
			return true;
        }
		return isFirstDrop;
    }

	public void setAnchor(GameObject anchor)
    {
		this.anchor = anchor;
    }

}
