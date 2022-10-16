using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    public Transform gridLayout;
    private void Awake()
    {
        gameObject.tag = "dropArea";
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Add(GameObject card)
    {
        card.GetComponent<UIDragAndDrop>().canvasGroup.blocksRaycasts = true;
        Instantiate(card, gridLayout);
        gridLayout.GetComponent<Collection>().Add(card);
        
        Destroy(card);
    }
}
