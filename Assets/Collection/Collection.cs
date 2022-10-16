using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{

    public enum CollectionType { Collection, Deck};
    public CollectionType collectionType;

    public int space = 20;

    public List<GameObject> cards = new List<GameObject>();

    public GameObject collectionSlotPrefab;
    public Transform collectionLayout;
    //private void Update()
    //{
    //    switch (collectionType)
    //    {
    //        case Collection.CollectionType.Collection:
    //            space = cards.Count;
    //            break;
    //        case Collection.CollectionType.Deck:
    //            break;
    //        default:
    //            break;
    //    }
            

    //    if (collectionLayout.childCount > space)
    //    {
    //        for (int i = collectionLayout.childCount; i > space; i--)
    //        {
    //            GameObject child = collectionLayout.transform.GetChild(i - 1).gameObject;
    //            Destroy(child);
    //        }
    //    }
    //    if (collectionLayout.childCount < space)
    //    {
    //        for (int i = collectionLayout.childCount; i < space; i++)
    //        {
    //            Instantiate(collectionSlotPrefab, collectionLayout);
    //        }
    //    }
    //}

    public void Add(GameObject card)
    {
        cards.Add(card);
    }

    public void Remove(GameObject card)
    {
        cards.Remove(card);
    }

}
