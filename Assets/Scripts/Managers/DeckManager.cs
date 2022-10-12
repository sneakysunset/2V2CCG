using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    public List<Card> deck;
    public Transform deckTransform;
}

public class DeckManager : MonoBehaviour
{
    public Deck[] decks = new Deck[4];
}
