using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragAndDrop : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [HideInInspector]public CanvasGroup canvasGroup;
    public GameObject[] dropAreasCanvasGroup;
    Vector3 offset;
    Canvas canvas;
    public GameObject clone;
    public bool isOriginal = true;
    public bool isDrag;
    private void Awake()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        dropAreasCanvasGroup = GameObject.FindGameObjectsWithTag("dropArea");

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.GetComponent<CardLogic>().isActive == false) return;
        if (isOriginal) return;

        transform.position = Input.mousePosition + offset;
    }
    private void Update()
    {
        if(isDrag && !isOriginal)
        {
            Dragging();
            if (Input.GetMouseButtonUp(0))
            {
                isDrag = false;
                Up();
            }
        }
           
    }

    void Dragging()
    {
        transform.position = Input.mousePosition + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (gameObject.GetComponent<CardLogic>().isActive == false) return;
        if (isOriginal) return;
        Debug.Log("UP");

        Up();

    }

    PointerEventData pointerData;

    public void Up()
    {
        if (!isOriginal)
            isDrag = false;

        RaycastResult raycastResult = pointerData.pointerCurrentRaycast;
        if (raycastResult.gameObject.tag == "dropArea")
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
        if (gameObject.GetComponent<CardLogic>().isActive == false) return;
        if (clone == null && gameObject.GetComponent<UIDragAndDrop>().isOriginal == true)
        {
            clone = Instantiate(gameObject, canvas.transform);
            clone.GetComponent<UIDragAndDrop>().isOriginal = false;
            clone.GetComponent<UIDragAndDrop>().isDrag = true;
            gameObject.GetComponent<CardLogic>().isActive = false;
        }

        if (!isOriginal)
            isDrag = true;

        offset = transform.position - Input.mousePosition;
        

        canvasGroup.blocksRaycasts = false;

        foreach (var dropAreaCanvasGroup in dropAreasCanvasGroup)
        {
            dropAreaCanvasGroup.SetActive(true);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        pointerData = eventData;
    }
}
