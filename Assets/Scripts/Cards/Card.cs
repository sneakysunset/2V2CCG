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
    public Sprite  cardImage, cardOverlay, star1;
    public AnimationCurve anchoringAnimCurve;
    public float hoveredPositionHeight;

    [Header("Components")]
    public GameObject cardFrontComp;
    public bool cardFaceRevealed;
    public TextMesh cardNameComp;
    public TextMesh classNameComp;
    public SpriteRenderer cardImageComp, cardOverlayComp;
    public SpriteRenderer[] stars = new SpriteRenderer[0];

    [HideInInspector] public Vector3 AnchoringPosition;
    [HideInInspector] public bool dragged;
    /*[HideInInspector]*/ public int handSlotIndex;
    protected HandManager handManager;

    public float hoveredScalingMult = 1.5f;
    protected Vector3 hoveredStartScale, hoveredEndScale;
    protected Vector3  hoveredEndPos;
    protected IEnumerator scalerEnum, posEnum, hoveringEnum;
    public int lvlCondition;
    //UnityEvent tutorialChecker;
    #endregion
    int playerIndexer;

    public virtual void Start()
    {
        if (teamNum == HandManager.teamNum.T1 && playerNum == HandManager.playerNum.J1)
            playerIndexer = 0;
        if (teamNum == HandManager.teamNum.T1 && playerNum == HandManager.playerNum.J2)
            playerIndexer = 1;
        if (teamNum == HandManager.teamNum.T2 && playerNum == HandManager.playerNum.J1)
            playerIndexer = 2;
        if (teamNum == HandManager.teamNum.T2 && playerNum == HandManager.playerNum.J2)
            playerIndexer = 3;

        if (teamNum == HandManager.teamNum.T2)
        {
            hoveredPositionHeight = -hoveredPositionHeight;
        }

        

        foreach(SpriteRenderer sprite in stars)
        {
            sprite.color = Color.grey;
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

        switch (className)
        {
            case "Nain":
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].dwarfLvlIndex;
                break;
            case "Elfe":
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].elfLvlIndex;
                break;
            case "Demon":
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].demonLvlIndex;
                break;
            default:
                Debug.LogError("Wrong Class Name");
                return;
        }


        cardNameComp.text = className;
        
        switch (level)
        {
            case 0:
                //starComp1.enabled = false;
                break;
            case 1:
                stars[0].enabled = true;
                break;
            case 2:
                stars[3].enabled = true;
                stars[4].enabled = true;
                break;
            case 3:
                stars[0].enabled = true;
                stars[1].enabled = true;
                stars[2].enabled = true;
                break;
            default:
                Debug.LogError("WrongLevel");
                break;
        }
    }



    void ColorChange(Color col, int currentLevelIndex)
    {
        if(currentLevelIndex == 1)
        {
            stars[0].color = col;
            stars[3].color = col;
        }
        else if(currentLevelIndex == 2)
        {
            stars[3].color = col;
            stars[4].color = col;
            stars[0].color = col;
            stars[1].color = col;
        }
        else if(currentLevelIndex == 3)
        {
            stars[0].color = col;
            stars[1].color = col;
            stars[2].color = col;
        }
    }

   public void UpdateLevelindex()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0); 
        switch (className)
        {
            case "Nain":
                lvlCondition = handManager.Hands[playerIndexer].dwarfLvlIndex;
                ColorChange(cardManager.dwarfColor, lvlCondition);

                break;
            case "Elfe":
                lvlCondition = handManager.Hands[playerIndexer].elfLvlIndex;
                ColorChange(cardManager.elfColor, lvlCondition);
                break;
            case "Demon":
                lvlCondition = handManager.Hands[playerIndexer].demonLvlIndex;
                ColorChange(cardManager.demonColor, lvlCondition);

                break;
            default:
                Debug.LogError("Wrong Class Name");
                return;
        }
    }

    public virtual void CardUsed()
    {
        switch (className)
        {
            case "Nain":
                handManager.Hands[priorityHandler.currentPriority].dwarfLvlIndex++;
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].dwarfLvlIndex;
                ColorChange(cardManager.dwarfColor, lvlCondition);

                break;
            case "Elfe":
                handManager.Hands[priorityHandler.currentPriority].elfLvlIndex++;
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].elfLvlIndex;
                ColorChange(cardManager.elfColor, lvlCondition);
                break;
            case "Demon":
                handManager.Hands[priorityHandler.currentPriority].demonLvlIndex++;
                lvlCondition = handManager.Hands[priorityHandler.currentPriority].demonLvlIndex;
                ColorChange(cardManager.demonColor, lvlCondition);

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
            GetComponent<SortingGroup>().sortingOrder = 3;

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
        GetComponent<SortingGroup>().sortingOrder = 0;
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
