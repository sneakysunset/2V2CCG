using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public CardDraw cardDrawer;
    public DeckManager deckManager;
    public HandManager handManager;
    private void Start()
    {
        handManager.Hands[0].actionTokenNumber = 2;
    }

    void Tuto1FirstDraw()
    {

    }
}
