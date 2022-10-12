using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDraw : MonoBehaviour
{
    public DeckManager deckManager;
    public HandManager handManager;
    public void DrawCard(int playerIndex)
    {
        Vector3 handSlotTarget = Vector3.zero;
        int handSlotIndex = 0;
        for (int i = 0; i <= handManager.Hands[playerIndex].handSlots.Count; i++)
        {
            if (i == handManager.Hands[playerIndex].handSlots.Count) return;

            if (!handManager.Hands[playerIndex].occupied[i])
            {
                handSlotTarget = handManager.Hands[playerIndex].handSlots[i].position;
                handManager.Hands[playerIndex].occupied[i] = true;
                handSlotIndex = i;
                break;
            }
        }
        Card card = Instantiate(deckManager.decks[playerIndex].deck[deckManager.decks[playerIndex].deck.Count - 1].gameObject, deckManager.decks[playerIndex].deckTransform.position, Quaternion.identity).GetComponent<Card>();
        card.AnchoringPosition = handSlotTarget;
        card.handSlotIndex = handSlotIndex;
        card.playerNum = handManager.Hands[playerIndex].pNum;
        card.teamNum = handManager.Hands[playerIndex].tNum;
        card.playerIndex = playerIndex;
        if(card.teamNum == HandManager.teamNum.T2)
        {
            card.cardFrontComp.SetActive(false);
            card.cardFaceRevealed = false;
        }
        deckManager.decks[playerIndex].deck.RemoveAt(deckManager.decks[playerIndex].deck.Count - 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DrawCard(1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DrawCard(2);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawCard(3);
        }
    }
}
