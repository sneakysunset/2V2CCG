using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerData : MonoBehaviour, IPointerMoveHandler
{

    public static PointerData instance;
    private void Awake()
    {
        instance = this;
    }

    public PointerEventData pointerData;
    public void OnPointerMove(PointerEventData eventData)
    {
        Debug.Log("Pipi");
        pointerData = eventData;
    }
}
