using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card_Unit : Card
{
    #region variables
    public Card currentEquipment;
    public TextMesh attackTextComp, defenseTextComp;
    public Transform currentBoardSlot;
    int currentHoveredBoardSlot;
    BoardSlotsManager boardSlotsManager;
    public int attack = 1;
    public int defense = 1;
    int IAtargetIndex;
    [HideInInspector]public UnityEvent endCoroutineEffect;
    #endregion


    public override void Start()
    {
        this.endCoroutineEffect.AddListener(() => this.EndCoroutineEffect());
        boardSlotsManager = FindObjectOfType<BoardSlotsManager>();
        base.Start();
        attackTextComp.text = "" + attack;
        defenseTextComp.text = "" + defense;
    }

    #region PlayerActions

    public void OnMouseEnter()
    {
        if(!cardManager.dragging)
            base.OnMouseEnterEvent();
    }

    public void OnMouseExit()
    {
        base.OnMouseExitEvent();
    }

    public void OnMouseDrag()
    {
        if (teamNum == HandManager.teamNum.T1 && playerNum == HandManager.playerNum.J1 && handManager.Hands[0].actionTokenNumber > 0 && priorityHandler.currentPriority == 0)
        {
            if (handSlotIndex != -1)
            {
                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, LayerMask.GetMask("Slot"));

                if (rayHit)
                {
                    string[] collName = rayHit.transform.name.Split(" : ");
                    currentHoveredBoardSlot = int.Parse(collName[0]);
                    if(boardSlotsManager.boardSlot[currentHoveredBoardSlot] == null)
                    {
                        bool condition = collName[1] == "BoardSlot" && collName[2] == teamNum.ToString() && (collName[3] == playerNum.ToString() || collName[3] == "J");
                        if (condition)
                        {

                            transform.position = rayHit.transform.position;
                            return;
                        }
                    }
                }
                else
                {
                    currentHoveredBoardSlot = -1;
                    currentBoardSlot = null;
                }
                base.OnMouseDragEvent();
            }
        }
    }

    public void ChangeStat(int attackChange, int defenseChange)
    {
        attack += attackChange;
        defense += defenseChange;
        attackTextComp.text = attack.ToString();
        defenseTextComp.text = defense.ToString();
    }

    void DropCard()
    {
        if (currentHoveredBoardSlot != -1 && boardSlotsManager.boardSlot[currentHoveredBoardSlot] == null)
        {
            boardSlotsManager.boardSlot[currentHoveredBoardSlot] = this;
            base.CardUsed();
        }
        else if (currentHoveredBoardSlot != -1 && boardSlotsManager.boardSlot[currentHoveredBoardSlot] != null)
        {
            Destroy(boardSlotsManager.boardSlot[currentHoveredBoardSlot].gameObject);
            boardSlotsManager.boardSlot[currentHoveredBoardSlot] = this;
            base.CardUsed();
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

    #region IA Actions

    public override void IAPlay(int targetIndex)
    {
        base.IAPlay(targetIndex);
        IAtargetIndex = targetIndex;
        if(priorityHandler.currentPriority > 1)
        {
            cardBack.SetActive(false);
            cardFrontComp.SetActive(true);
            cardFaceRevealed = true;
        }
        StartCoroutine(MoveAnimations.LerpToAnchor(transform.position, boardSlotsManager.boardSlotTransform[targetIndex].position, anchoringAnimCurve, transform, .3f, endCoroutineEffect));
    }

    void EndCoroutineEffect()
    {
        if (boardSlotsManager.boardSlot[IAtargetIndex] != null) 
            Destroy(boardSlotsManager.boardSlot[IAtargetIndex].gameObject);
        boardSlotsManager.boardSlot[IAtargetIndex] = this;
        base.CardUsed();
        tutorialManager.tutorialIndex++;
        tutorialManager.tutorialPlaying = false;

    }

    #endregion

}
