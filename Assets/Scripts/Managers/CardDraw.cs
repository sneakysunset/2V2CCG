using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardDraw : MonoBehaviour
{
    public DeckManager deckManager;
    public HandManager handManager;
    public PriorityHandler priorityHandler;
    public TutorialManager tutorialManager;
    [HideInInspector]public UnityEvent cardAnimEnd;
    int i;
    int playerIndexProxy, drawNumberProxy;
    private void Awake()
    {
        this.cardAnimEnd.AddListener(() => this.CardAnimEnd());
    }
    public void DrawCard(int playerIndex, int drawNumber)
    {
        playerIndexProxy = playerIndex;
        drawNumberProxy = drawNumber;
        //print(i);

        Vector3 handSlotTarget = Vector3.zero;
        int handSlotIndex = 0;
        for (int j = 0; j <= handManager.Hands[playerIndex].handSlots.Count; j++)
        {
            if (j == handManager.Hands[playerIndex].handSlots.Count) return;

            if (!handManager.Hands[playerIndex].occupied[j])
            {
                handSlotTarget = handManager.Hands[playerIndex].handSlots[j].position;
                handManager.Hands[playerIndex].occupied[j] = true;
                handSlotIndex = j;
                break;
            }
        }
        Card card = Instantiate(deckManager.decks[playerIndex].deck[0].gameObject, deckManager.decks[playerIndex].deckTransform.position, Quaternion.identity).GetComponent<Card>();
        handManager.Hands[playerIndex].cardList[handSlotIndex] = card;
        card.AnchoringPosition = handSlotTarget;
        card.handSlotIndex = handSlotIndex;
        card.playerNum = handManager.Hands[playerIndex].pNum;
        card.teamNum = handManager.Hands[playerIndex].tNum;
        card.playerIndex = playerIndex;
        if (card.teamNum == HandManager.teamNum.T2)
        {
            card.cardFrontComp.SetActive(false);
            card.cardFaceRevealed = false;
        }
        else
        {
            card.cardBack.SetActive(false);
        }
        deckManager.decks[playerIndex].deck.Add(deckManager.decks[playerIndex].deck[0]);
        deckManager.decks[playerIndex].deck.RemoveAt(0);
        
        
        StartCoroutine(MoveAnimations.LerpToAnchor(card.transform.position, card.AnchoringPosition, card.anchoringAnimCurve, card.transform, .5f, cardAnimEnd));
    }

    void CardAnimEnd()
    {
        i++;
        if (i < drawNumberProxy)
        {
            DrawCard(playerIndexProxy, drawNumberProxy);
        }
        else
        {
            playerIndexProxy = 0;
            drawNumberProxy = 0;
            i = 0;
            if (priorityHandler.currentPriority != 0)
            {
                tutorialManager.tutorialIndex++;
                tutorialManager.tutorialPlaying = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DrawCard(1, 1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DrawCard(2, 1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawCard(3, 1);
        }
    }
}
