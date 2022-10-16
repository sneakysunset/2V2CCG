using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card_Unit : Card
{
    #region variables
    public TextMesh attackTextComp, defenseTextComp;
    public Transform currentBoardSlot;
    public int currentHoveredBoardSlot;
    BoardSlotsManager boardSlotsManager;
    public int attack = 1;
    public int defense = 1;
    int IAtargetIndex;
    [HideInInspector]public UnityEvent endCoroutineEffect;

    [Space(10)]
    [Header("AttackVariables")]
    public GameObject arrowPointer;
    Transform arrowHeadPos;
    LineRenderer arrowLineRenderer;
    Card_Unit attackTarget;
    Transform attackTargetTransform;
    int colliName;

    #endregion

    #region Generic Functions
    public override void Start()
    {
        this.endCoroutineEffect.AddListener(() => this.EndCoroutineEffect());
        boardSlotsManager = FindObjectOfType<BoardSlotsManager>();
        base.Start();
        classNameComp.text = cardName;
        attackTextComp.text = "" + attack;
        defenseTextComp.text = "" + defense;
    }
    private void Update()
    {
        base.UpdateLevelindex();

        if (arrowHeadPos != null)
        {
            attackArrowPointer();
        }

        if(defense <= 0)
        {
            boardSlotsManager.boardSlot[currentHoveredBoardSlot] = null;
            //boardSlotsManager.boardSlotTransform[currentHoveredBoardSlot].GetComponent<Collider2D>().enabled = true;

            Destroy(this.gameObject);
        }
    }
    public void ChangeStat(int attackChange, int defenseChange)
    {
        attack += attackChange;
        defense += defenseChange;
        attackTextComp.text = attack.ToString();
        defenseTextComp.text = defense.ToString();
    }
    #endregion

    #region PlayerActions

    #region play
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
    void DropCard()
    {
        if (currentHoveredBoardSlot != -1 && boardSlotsManager.boardSlot[currentHoveredBoardSlot] == null)
        {
            boardSlotsManager.boardSlot[currentHoveredBoardSlot] = this;
            tutorialManager.tutorialPlaying = false;
            hoveredStartScale.x = 1.5f;
            hoveredStartScale.y = 1.5f;
            //boardSlotsManager.boardSlotTransform[currentHoveredBoardSlot].GetComponent<Collider2D>().enabled = false;
             MoveAnimations.LerpToScaling(transform.localScale, hoveredStartScale, anchoringAnimCurve, transform, .2f);
            base.CardUsed();
        }
        else if (currentHoveredBoardSlot != -1 && boardSlotsManager.boardSlot[currentHoveredBoardSlot] != null)
        {
            Destroy(boardSlotsManager.boardSlot[currentHoveredBoardSlot].gameObject);
            boardSlotsManager.boardSlot[currentHoveredBoardSlot] = this;
            hoveredStartScale.x = 1.5f;
            hoveredStartScale.y = 1.5f;
            //boardSlotsManager.boardSlotTransform[currentHoveredBoardSlot].GetComponent<Collider2D>().enabled = false;

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


    #region attack
    public List<GameObject> highlights = new List<GameObject>();
    private void OnMouseDown()
    {
        if (handSlotIndex == -1 && priorityHandler.currentPriority == 0 && tutorialManager.canPlay && !tutorialManager.pause && lvlCondition >= level && teamNum == HandManager.teamNum.T1)
        {
            arrowHeadPos = Instantiate(arrowPointer).transform;
            arrowLineRenderer = arrowHeadPos.GetComponent<LineRenderer>();
            arrowLineRenderer.SetPosition(0, transform.position);
            
            for (int i = 0; i < boardSlotsManager.boardSlot.Length; i++)
            {
                string[] intelliName = boardSlotsManager.boardSlotTransform[i].name.Split(" : ");

                
                if (intelliName[2] == "T2" && (boardSlotsManager.boardSlot[i] || currentHoveredBoardSlot + 3 == int.Parse(intelliName[0])))
                {
                    if (boardSlotsManager.boardSlot[i])
                    {
                        boardSlotsManager.boardSlotTransform[i].Find("HighLight").gameObject.SetActive(true);
                        highlights.Add(boardSlotsManager.boardSlotTransform[i].Find("HighLight").gameObject);
                    }
                    else
                    {
                        boardSlotsManager.nexusHPT2.transform.Find("HighLight").gameObject.SetActive(true);
                        highlights.Add(boardSlotsManager.nexusHPT2.transform.Find("HighLight").gameObject);
                    }
                }
            }
        }
    }

    void attackArrowPointer()
    {
        arrowLineRenderer.SetPosition(1, arrowHeadPos.position);
        arrowHeadPos.rotation = Quaternion.LookRotation(Vector3.forward, arrowHeadPos.position - transform.position);
        LayerMask layerMask = LayerMask.GetMask("Slot");
        for (int i = 0; i < boardSlotsManager.boardSlot.Length; i++)
        {
            string[] intelliName = boardSlotsManager.boardSlotTransform[i].name.Split(" : ");

            if (!boardSlotsManager.boardSlot[i] && currentHoveredBoardSlot + 3 == int.Parse(intelliName[0])) 
                layerMask = LayerMask.GetMask("Slot") + LayerMask.GetMask("Nexus");
        }
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, layerMask);
        if (rayHit)
        {
            string[] collName = rayHit.transform.name.Split(" : ");
            colliName = int.Parse(collName[0]);
            bool condition = collName[1] == "BoardSlot" && collName[2] != teamNum.ToString();

            if ((condition && boardSlotsManager.boardSlot[colliName] != null) || rayHit.transform.name == "10 : 2 : Ennemy Nexus")
            {
                arrowHeadPos.position = rayHit.transform.position;
                if (Input.GetMouseButtonUp(0))
                {
                    bool target = false;
                    if (rayHit.transform.name != "10 : 2 : Ennemy Nexus")
                    {
                        attackTarget = boardSlotsManager.boardSlot[colliName].GetComponent<Card_Unit>();
                        target = true;
                    }

                    attackTargetTransform = rayHit.transform;
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
            foreach (GameObject highlight in highlights)
            {
                highlight.SetActive(false);
            }
            highlights.Clear();
        }
       
    }
    public AnimationCurve attackAnimationCurve;
    void Attack(bool target)
    {
        Destroy(arrowHeadPos.gameObject);
        foreach(GameObject highlight in highlights)
        {
            highlight.SetActive(false);
        }
        highlights.Clear();
        StartCoroutine(MoveAnimations.AttackLerpToAnchor(transform.position, attackTargetTransform.position, attackAnimationCurve, transform, .6f));

        if (target)
        {
            attackTarget.ChangeStat(0, -attack);
            ChangeStat(0, -attackTarget.attack);
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

    #endregion

    #endregion

    #region IA Actions
    bool tuto;
    float timer;
    Vector3 targetp;
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
        targetp = boardSlotsManager.boardSlotTransform[targetIndex].position;
        targetp.z = 0;
        StartCoroutine(MoveAnimations.LerpToAnchor(transform.position,targetp, anchoringAnimCurve, transform, .3f, endCoroutineEffect));
    }

    public override void IAAttack(int targetIndex, bool nextTuto, float timerd)
    {
        bool target = false;
        Vector3 targetCor = Vector3.zero ;
        if(boardSlotsManager.boardSlot[targetIndex] != null)
        {
            targetCor = boardSlotsManager.boardSlotTransform[targetIndex].position;
            attackTarget = boardSlotsManager.boardSlot[targetIndex].GetComponent<Card_Unit>();
            target = true;
        }
        else
        {
            if (teamNum == HandManager.teamNum.T1)
                targetCor = boardSlotsManager.nexusHPT2.transform.position;
            else targetCor = boardSlotsManager.nexusHPT1.transform.position;
        } 
        StartCoroutine(MoveAnimations.AttackLerpToAnchor(transform.position, targetCor, attackAnimationCurve, transform, .6f));

        if (target)
        {
            attackTarget.ChangeStat(0, -attack);
            ChangeStat(0, -attackTarget.attack);

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
        else
        {
            tutorialManager.canPlay = true;
            tutorialManager.changeToolTip();
        }
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
        if (!tuto)
            tutorialManager.tutorialPlaying = false;
        else
        {
            tutorialManager.canPlay = true;
            tutorialManager.changeToolTip();
        }
        transform.position = targetp;
    }

    #endregion

}
