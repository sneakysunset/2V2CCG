using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


public class Card_Unit : Card
{
    public TextMesh attackTextComp, defenseTextComp;
    public Transform currentBoardSlot;
    int currentHoveredBoardSlot;
    BoardSlotsManager boardSlotsManager;
    public int attack = 1;
    public int defense = 1;

    public override void Start()
    {
        boardSlotsManager = FindObjectOfType<BoardSlotsManager>();
        base.Start();
        attackTextComp.text = "" + attack;
        defenseTextComp.text = "" + defense;
    }

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
        if (teamNum == HandManager.teamNum.T1 && playerNum == HandManager.playerNum.J1)
        {
            if (handSlotIndex != -1)
            {
                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, LayerMask.GetMask("Slot"));

                if (rayHit && currentHoveredBoardSlot != -1 && boardSlotsManager.boardSlot[currentHoveredBoardSlot] == null)
                {
                    string[] collName = rayHit.transform.name.Split(" : ");
                    bool condition = collName[1] == "BoardSlot" && collName[2] == teamNum.ToString() && (collName[3] == playerNum.ToString() || collName[3] == "J");
                    if (condition)
                    {
                        transform.position = rayHit.transform.position;
                        return;
                    }
                }
                
                base.OnMouseDragEvent();
            }
        }
    }

    void DropCard()
    {
        if (currentBoardSlot != null)
        {
            handManager.Hands[playerIndex].occupied[handSlotIndex] = false;
            handSlotIndex = -1;
            boardSlotsManager.boardSlot[currentHoveredBoardSlot] = this;
            StartCoroutine(MoveAnimations.LerpToAnchor(transform.position, currentBoardSlot.position, anchoringAnimCurve, transform, .1f));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string[] collName = collision.name.Split(" : ");
        bool condition = collName[1] == "BoardSlot" && collName[2] == teamNum.ToString() && (collName[3] == playerNum.ToString() || collName[3] == "J");
        //print(collName[0] + " " + collName[1] + " " + collName[2] + " " + collName[3] + "  Enter");
        //print(condition);


        if (condition)
        {
            currentHoveredBoardSlot = int.Parse(collName[0]);
            if (boardSlotsManager.boardSlot[currentHoveredBoardSlot] == null)
                currentBoardSlot = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string[] collName = collision.name.Split(" : ");
        bool condition = collName[1] == "BoardSlot" && collName[2] == teamNum.ToString() && (collName[3] == playerNum.ToString() || collName[3] == "J");
        //print(collName[0] + " " + collName[1] + " " + collName[2] + " " + collName[3] + "  Exit");

        if (condition)
        {
            currentHoveredBoardSlot = -1;
            currentBoardSlot = null;
        }
    }
}
