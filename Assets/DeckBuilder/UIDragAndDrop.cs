using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragAndDrop : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]public CanvasGroup canvasGroup;
    GameObject[] dropAreasCanvasGroup;
    Vector3 offset;
    Canvas canvas;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvas = GameObject.FindObjectOfType<Canvas>();
        dropAreasCanvasGroup = GameObject.FindGameObjectsWithTag("dropArea");
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RaycastResult raycastResult = eventData.pointerCurrentRaycast;
        if(raycastResult.gameObject.tag == "dropArea")
        {
            raycastResult.gameObject.GetComponent<DropArea>().Add(gameObject);
        }

        canvasGroup.blocksRaycasts = true;
        foreach (var dropAreaCanvasGroup in dropAreasCanvasGroup)
        {
            dropAreaCanvasGroup.SetActive(false);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
        offset = transform.position - Input.mousePosition;

        canvasGroup.blocksRaycasts = false;

        foreach (var dropAreaCanvasGroup in dropAreasCanvasGroup)
        {
            dropAreaCanvasGroup.SetActive(true);
        }
    }
}
