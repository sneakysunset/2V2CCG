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
        GameObject cardclone = Instantiate(card, gridLayout);
        if(gridLayout.GetComponent<Collection>().collectionType == Collection.CollectionType.Deck && !gridLayout.GetComponent<Collection>().cards.Contains(card))
            gridLayout.GetComponent<Collection>().Add(cardclone);
        
        Destroy(card);
    }
    public void Remove(GameObject card)
    {
        if (gridLayout.GetComponent<Collection>().collectionType == Collection.CollectionType.Deck && gridLayout.GetComponent<Collection>().cards.Contains(card))
            gridLayout.GetComponent<Collection>().Remove(card);
    }
}
