using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Card_Equipment : Card
{
    #region variables
    public Transform card;
    public TextMesh attackTextComp, defenseTextComp;
    public Transform currentBoardSlot;
    int currentHoveredBoardSlot = -1;
    BoardSlotsManager boardSlotsManager;
    public int attack = 1;
    public int defense = 1;
    [HideInInspector] public UnityEvent endCoroutineEffect;
    int IAtargetIndex;
    #endregion

    public override void Start()
    {
        this.endCoroutineEffect.AddListener(() => this.EndCoroutineEffect());
        boardSlotsManager = FindObjectOfType<BoardSlotsManager>();
        base.Start();
        attackTextComp.text = "+" + attack;
        defenseTextComp.text = "+" + defense;
    }

    #region PlayerAction
    public void OnMouseEnter()
    {
        if (!cardManager.dragging)
            base.OnMouseEnterEvent();
    }

    public void OnMouseExit()
    {
        base.OnMouseExitEvent();
    }

    public void OnMouseDrag()
    {
        if (teamNum == HandManager.teamNum.T1 && playerNum == HandManager.playerNum.J1 && handManager.Hands[0].actionTokenNumber > 0 && priorityHandler.currentPriority == 0 && tutorialManager.canPlay && !tutorialManager.pause && lvlCondition >= level)
        {
            if (handSlotIndex != -1)
            {
                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, LayerMask.GetMask("Slot"));

                if (rayHit)
                {
                    string[] collName = rayHit.transform.name.Split(" : ");
                    currentHoveredBoardSlot = int.Parse(collName[0]);
                    if(boardSlotsManager.boardSlot[currentHoveredBoardSlot] != null)
                    {
                        bool condition = collName[1] == "BoardSlot" && collName[2] == teamNum.ToString() && (collName[3] == playerNum.ToString() || collName[3] == "J");
                        if (condition)
                        {
                            transform.position = rayHit.transform.position;
                            return;
                        }
                        
                    }
                }

                currentHoveredBoardSlot = -1;
                currentBoardSlot = null;
                base.OnMouseDragEvent();
            }
        }
    }

    void DropCard()
    {
        if (currentHoveredBoardSlot != -1 && boardSlotsManager.boardSlot[currentHoveredBoardSlot] != null)
        {
            boardSlotsManager.boardSlot[currentHoveredBoardSlot].GetComponent<Card_Unit>().ChangeStat(attack, defense);
            tutorialManager.tutorialPlaying = false;
            base.CardUsed();
            Destroy(this.gameObject);
        }
        else
        {
            StartCoroutine(MoveAnimations.LerpToAnchor(transform.position, AnchoringPosition, anchoringAnimCurve, transform, .1f));
        }
        dragged = false;
        cardManager.dragging = false;
    }

    private void OnMouseUp()
    {
        if (dragged)
        {
            DropCard();
        }
    }
    #endregion

    #region IA Action

    bool tuto;
    float timer;
    public override void IAPlay(int targetIndex, bool nextTuto, float timerd)
    {
        tuto = nextTuto;
        timer = timerd;
        base.IAPlay(targetIndex, nextTuto, timerd);
        if (boardSlotsManager.boardSlot[targetIndex] != null)
        {
            IAtargetIndex = targetIndex;
            if (priorityHandler.currentPriority > 1)
            {
                cardBack.SetActive(false);
                cardFrontComp.SetActive(true);
                cardFaceRevealed = true;
            }
            StartCoroutine(MoveAnimations.LerpToAnchor(transform.position, boardSlotsManager.boardSlotTransform[targetIndex].position, anchoringAnimCurve, transform, .3f, endCoroutineEffect));
        }
    }

    void EndCoroutineEffect()
    {
        base.CardUsed();
        boardSlotsManager.boardSlot[IAtargetIndex].GetComponent<Card_Unit>().ChangeStat(attack, defense);
        tutorialManager.tutorialIndex++;
        tutorialManager.timer = timer;
        if(!tuto)
            tutorialManager.tutorialPlaying = false;
        else tutorialManager.canPlay = true;
        Destroy(this.gameObject);
    }


    #endregion
}
