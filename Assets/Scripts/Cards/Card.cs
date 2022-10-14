using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
    #region variables
    public GameObject cardBack;
    protected CardManager cardManager;
    protected PriorityHandler priorityHandler;
    protected TutorialManager tutorialManager;
    public int playerIndex;
    public HandManager.playerNum playerNum;
    public HandManager.teamNum teamNum;
    public float positionningSpeed = .3f;
    public float hoveringSpeed = .5f;
    [Header("Variables")]

    public int level;
    public string cardName, className;
    public Sprite  cardImage, cardOverlay, star1, star2, star3;
    public AnimationCurve anchoringAnimCurve;
    public float hoveredPositionHeight;

    [Header("Components")]
    public GameObject cardFrontComp;
    public bool cardFaceRevealed;
    public TextMesh cardNameComp;
    public TextMesh classNameComp;
    public SpriteRenderer cardImageComp, cardOverlayComp, starComp;

    [HideInInspector] public Vector3 AnchoringPosition;
    [HideInInspector] public bool dragged;
    [HideInInspector] public int handSlotIndex;
    protected HandManager handManager;

    public float hoveredScalingMult = 1.5f;
    protected Vector3 hoveredStartScale, hoveredEndScale;
    protected Vector3  hoveredEndPos;
    protected IEnumerator scalerEnum, posEnum, hoveringEnum;
    protected int lvlCondition;
    //UnityEvent tutorialChecker;
    #endregion

    public virtual void Start()
    {
        if(teamNum == HandManager.teamNum.T2)
        {
            hoveredPositionHeight = -hoveredPositionHeight;
        }
        hoveredStartScale = transform.localScale;
        hoveredEndScale = transform.localScale * hoveredScalingMult;
        hoveredEndPos = AnchoringPosition;
        hoveredEndPos.y += hoveredPositionHeight;
        handManager = FindObjectOfType<HandManager>();
        cardManager = FindObjectOfType<CardManager>();
        priorityHandler = FindObjectOfType<PriorityHandler>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        //tutorialChecker.AddListener(() => TutorialValidation());

        
        cardNameComp.text = cardName;
        classNameComp.text = className;
        switch (level)
        {
            case 0:
                starComp.enabled = false;
                break;
            case 1:
                starComp.sprite = star1;
                break;
            case 2:
                starComp.sprite = star2;
                break;
            case 3:
                starComp.sprite = star3;
                break;
            default:
                Debug.LogError("WrongLevel");
                break;
        }
    }


    public virtual void CardUsed()
    {
        switch (className)
        {
            case "Dwarf":
                handManager.Hands[priorityHandler.currentPriority].dwarfLvlIndex++;
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].dwarfLvlIndex;
                break;
            case "Elf":
                handManager.Hands[priorityHandler.currentPriority].elfLvlIndex++;
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].elfLvlIndex;
                break;
            case "Demon":
                handManager.Hands[priorityHandler.currentPriority].demonLvlIndex++;
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].demonLvlIndex;
                break;
            default:
                Debug.LogError("Wrong Class Name");
                return;
        }
        handManager.Hands[playerIndex].occupied[handSlotIndex] = false;
        priorityHandler.ActionFinished(handManager.Hands[playerIndex].actionTokenNumber);
        handManager.Hands[playerIndex].actionTokenNumber -= 1;
        handManager.Hands[playerIndex].cardList[handSlotIndex] = null;
        handSlotIndex = -1;
    }


    #region PlayerCards

    public virtual void OnMouseEnterEvent()
    {
        if (!tutorialManager.pause)
        {
            if(scalerEnum != null) StopCoroutine(scalerEnum);
            scalerEnum = MoveAnimations.LerpToScaling(transform.localScale, hoveredEndScale, anchoringAnimCurve, transform, .2f);
            StartCoroutine(scalerEnum);
            GetComponent<SortingGroup>().sortingOrder += 1;

            if (!dragged && handSlotIndex != -1)
            {
                if (posEnum != null) StopCoroutine(posEnum);
                posEnum = MoveAnimations.LerpToAnchor(transform.position, hoveredEndPos, anchoringAnimCurve, transform, .2f);
                StartCoroutine(posEnum);
            }
        }
    }

    public virtual void OnMouseExitEvent()
    {
        if (scalerEnum != null) StopCoroutine(scalerEnum);
        scalerEnum = MoveAnimations.LerpToScaling(transform.localScale, hoveredStartScale, anchoringAnimCurve, transform, .2f);
        StartCoroutine(scalerEnum);
        GetComponent<SortingGroup>().sortingOrder += -1;
        if (handSlotIndex != -1 && !dragged)
        {
            if (posEnum != null) StopCoroutine(posEnum);
            posEnum = MoveAnimations.LerpToAnchor(transform.position, AnchoringPosition, anchoringAnimCurve, transform, .2f);
            StartCoroutine(posEnum);
        }
    }

    public virtual void OnMouseDragEvent()
    {

        dragged = true;
        cardManager.dragging = true;
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
    }
    #endregion


    #region IA Actions

    public virtual void IAPlay(int targetIndex, bool nextTuto, float timerd)
    {

    }

    public virtual void IAAttack(int targetIndex, bool nextTuto, float timerd)
    {

    }


    #endregion
}
