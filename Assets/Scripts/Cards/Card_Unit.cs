using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card_Unit : Card
{
    #region variables
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
        if (teamNum == HandManager.teamNum.T1 && playerNum == HandManager.playerNum.J1 && handManager.Hands[0].actionTokenNumber > 0 && priorityHandler.currentPriority == 0 && tutorialManager.canPlay && !tutorialManager.pause && lvlCondition >= level)
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
            else
            {

            }
        }
    }

    public GameObject arrowPointer;
    Transform arrowHeadPos;
    LineRenderer arrowLineRenderer;

    private void OnMouseDown()
    {
        if (handSlotIndex == -1 && priorityHandler.currentPriority == 0 && tutorialManager.canPlay && !tutorialManager.pause && lvlCondition >= level)
        {
            arrowHeadPos = Instantiate(arrowPointer).transform;
            arrowLineRenderer = arrowHeadPos.GetComponent<LineRenderer>();
            arrowLineRenderer.SetPosition(0, transform.position);
        }
    }

    private void Update()
    {
        if (arrowHeadPos != null)
        {
            attackArrowPointer();
        }

        if(defense <= 0)
        {
            boardSlotsManager.boardSlot[currentHoveredBoardSlot] = null;
            Destroy(this.gameObject);
        }
    }

    Card_Unit attackTarget;
    Transform attackTargetTransform;
    int colliName;
    void attackArrowPointer()
    {
        arrowLineRenderer.SetPosition(1, arrowHeadPos.position);
        arrowHeadPos.rotation = Quaternion.LookRotation(Vector3.forward, arrowHeadPos.position - transform.position);
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, LayerMask.GetMask("Slot"));
       
        if (rayHit)
        {
            string[] collName = rayHit.transform.name.Split(" : ");
            colliName = int.Parse(collName[0]);
            //currentHoveredBoardSlot = int.Parse(collName[0]);

       
            bool condition = collName[1] == "BoardSlot" && collName[2] != teamNum.ToString();
            if (condition)
            {
                arrowHeadPos.position = rayHit.transform.position;
                if (Input.GetMouseButtonUp(0))
                {
                    bool target = false;
                    if (boardSlotsManager.boardSlot[colliName] != null)
                    {
                        attackTarget = boardSlotsManager.boardSlot[colliName].GetComponent<Card_Unit>();
                        target = true;
                    }
                    attackTargetTransform = boardSlotsManager.boardSlotTransform[colliName];
                    Attack(target);
                }
                return;
            }
       
        }
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        arrowHeadPos.position = pos;
       
       
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(arrowHeadPos.gameObject);
        }
       
    }
    public AnimationCurve attackAnimationCurve;
    void Attack(bool target)
    {
        Destroy(arrowHeadPos.gameObject);
        StartCoroutine(MoveAnimations.LerpToAnchor(transform.position, boardSlotsManager.boardSlotTransform[colliName].position, attackAnimationCurve, transform, .3f));

        if (target)
        {
            attackTarget.ChangeStat(0, -attack);
        }
        else
        {
            if (teamNum == HandManager.teamNum.T1)
                boardSlotsManager.AttackNexus(attack, false);
            else
                boardSlotsManager.AttackNexus(attack, true);

        }

        tutorialManager.tutorialPlaying = false;
        priorityHandler.ActionFinished(handManager.Hands[playerIndex].actionTokenNumber);
        handManager.Hands[playerIndex].actionTokenNumber -= 1;
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
            tutorialManager.tutorialPlaying = false;
            hoveredStartScale.x = 1.5f;
            hoveredStartScale.y = 1.5f;
             MoveAnimations.LerpToScaling(transform.localScale, hoveredStartScale, anchoringAnimCurve, transform, .2f);
            base.CardUsed();
        }
        else if (currentHoveredBoardSlot != -1 && boardSlotsManager.boardSlot[currentHoveredBoardSlot] != null)
        {
            Destroy(boardSlotsManager.boardSlot[currentHoveredBoardSlot].gameObject);
            boardSlotsManager.boardSlot[currentHoveredBoardSlot] = this;
            hoveredStartScale.x = 1.5f;
            hoveredStartScale.y = 1.5f;
            MoveAnimations.LerpToScaling(transform.localScale, hoveredStartScale, anchoringAnimCurve, transform, .2f);
            tutorialManager.tutorialPlaying = false;

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
    bool tuto;
    float timer;
    public override void IAPlay(int targetIndex, bool nextTuto, float timerd)
    {
        tuto = nextTuto;
        timer = timerd;
        IAtargetIndex = targetIndex;
        if(priorityHandler.currentPriority > 1)
        {
            cardBack.SetActive(false);
            cardFrontComp.SetActive(true);
            cardFaceRevealed = true;
        }

        StartCoroutine(MoveAnimations.LerpToAnchor(transform.position, boardSlotsManager.boardSlotTransform[targetIndex].position, anchoringAnimCurve, transform, .3f, endCoroutineEffect));
    }

    public override void IAAttack(int targetIndex, bool nextTuto, float timerd)
    {
        bool target = false;
        if(boardSlotsManager.boardSlot[targetIndex] != null)
        {
            attackTarget = boardSlotsManager.boardSlot[targetIndex].GetComponent<Card_Unit>();
            target = true;
        }
        StartCoroutine(MoveAnimations.LerpToAnchor(transform.position, boardSlotsManager.boardSlotTransform[targetIndex].position, attackAnimationCurve, transform, .3f));

        if (target)
        {
            attackTarget.ChangeStat(0, -attack);
        }
        else
        {
            if (teamNum == HandManager.teamNum.T1)
                boardSlotsManager.AttackNexus(attack, false);
            else
                boardSlotsManager.AttackNexus(attack, true);

        }

        priorityHandler.ActionFinished(handManager.Hands[playerIndex].actionTokenNumber);
        handManager.Hands[playerIndex].actionTokenNumber -= 1;
        tutorialManager.tutorialIndex++;
        tutorialManager.timer = timer;
        if (!nextTuto)
            tutorialManager.tutorialPlaying = false;
        else tutorialManager.canPlay = true;
    }

    void EndCoroutineEffect()
    {
        if (boardSlotsManager.boardSlot[IAtargetIndex] != null) 
            Destroy(boardSlotsManager.boardSlot[IAtargetIndex].gameObject);

        hoveredStartScale.x = 1.5f;
        hoveredStartScale.y = 1.5f;
        StartCoroutine(MoveAnimations.LerpToScaling(transform.localScale, hoveredStartScale, anchoringAnimCurve, transform, .2f));
        boardSlotsManager.boardSlot[IAtargetIndex] = this;
        base.CardUsed();
        tutorialManager.tutorialIndex++;
        if(!tuto)
            tutorialManager.tutorialPlaying = false;
        else tutorialManager.canPlay = true;

    }

    #endregion

}
