using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragAndDrop : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]public CanvasGroup canvasGroup;
    public GameObject dropAreaCanvasGroup;
    Vector3 offset;
    Canvas canvas;
    public GameObject clone;
    public bool isOriginal = true;
    public bool isDrag;
    public GameObject master;
    private void Awake()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        dropAreaCanvasGroup = GameObject.FindGameObjectWithTag("dropArea");
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameObject.GetComponent<CardLogic>().isActive == false) return;
        if (isOriginal) return;

        transform.position = Input.mousePosition + offset;
    }
    private void Update()
    {
        if(dropAreaCanvasGroup == null) dropAreaCanvasGroup = GameObject.FindGameObjectWithTag("dropArea");

        if (isDrag && !isOriginal)
        {
            Dragging();
            if (Input.GetMouseButtonUp(0))
            {
                isDrag = false;
                Up(PointerData.instance.pointerData);
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

        Up(PointerData.instance.pointerData);

    }

    public void Up(PointerEventData eventData)
    {
        Debug.Log("UP CACA");
        if (!isOriginal)
            isDrag = false;

        RaycastResult raycastResult = eventData.pointerCurrentRaycast;
        Debug.Log(raycastResult.gameObject);
        if (raycastResult.gameObject.tag == "dropArea")
        {
            Debug.Log("Drop");
            dropAreaCanvasGroup.GetComponent<DropArea>().Remove(gameObject);
            raycastResult.gameObject.GetComponent<DropArea>().Add(gameObject);
        }
        else
        {
            Destroy(gameObject);
            master.GetComponent<CardLogic>().isActive = true;
            master.GetComponent<UIDragAndDrop>().canvasGroup.blocksRaycasts = true;
            dropAreaCanvasGroup.GetComponent<DropArea>().Remove(gameObject);
        }

        canvasGroup.blocksRaycasts = true;
        dropAreaCanvasGroup.SetActive(false);
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
            clone.GetComponent<UIDragAndDrop>().canvasGroup.blocksRaycasts = false;
            clone.GetComponent<UIDragAndDrop>().master = gameObject;
        }

        if (!isOriginal)
            isDrag = true;

        offset = transform.position - Input.mousePosition;
        canvasGroup.blocksRaycasts = false;

        dropAreaCanvasGroup.SetActive(true);

    }

    
}
