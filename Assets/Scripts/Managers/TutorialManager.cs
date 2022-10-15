using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct unityEvent
{
    public string TutorialStep;
    public UnityEvent<int, int, bool, float> tutorialCall;
    public int handSlotTargetOrDrawPlayerIndex;
    public int slotTargetOrDrawNumber;
    public float timerd;
    public bool nextTuto;
}

public class TutorialManager : MonoBehaviour
{
    public CardDraw cardDrawer;
    public DeckManager deckManager;
    public HandManager handManager;
    public BoardSlotsManager boardSlotsManager;
    public PriorityHandler priorityHandler;
    public ReloadScene canvasManager;
    public TextMesh toolTipText;
    public List<unityEvent> tutorialEvents;
    public int tutorialIndex;
    public bool tutorialPlaying;
    public bool canPlay;
    public float timer;
    public bool pause;
    public int toolTipIndex = 0;

    private void Update()
    {
        if(!tutorialPlaying /*&& priorityHandler.currentPriority != 0*/)
        {
            tutorialPlaying = true;
            canPlay = false;
            StartCoroutine(SimpleTimer(timer));
        }
/*
        if(priorityHandler.currentPriority != 0)
        {
            toolTipText.text = "";
        }*/
    }

    IEnumerator SimpleTimer(float timer)
    {
        yield return new WaitUntil(()=> !pause);
        yield return new WaitForSeconds(timer);
        print(tutorialEvents[tutorialIndex].TutorialStep);
        tutorialEvents[tutorialIndex].tutorialCall.Invoke(tutorialEvents[tutorialIndex].handSlotTargetOrDrawPlayerIndex, tutorialEvents[tutorialIndex].slotTargetOrDrawNumber, tutorialEvents[tutorialIndex].nextTuto, tutorialEvents[tutorialIndex].timerd);
    }

    private void Start()
    {
        
    }

    public void TutoTrigger()
    {
        tutorialPlaying = false;
    }

    public void PauseGame(bool boolean)
    {
        pause = boolean;
    }


    public void AttackCard(int cardIndex, int targetIndex, bool nextTuto, float timerd)
    {
        boardSlotsManager.boardSlot[cardIndex].IAAttack(targetIndex, nextTuto, timerd);
    }

    public void PlayCard(int cardIndex, int targetIndex, bool nextTuto, float timerd)
    {
        handManager.Hands[priorityHandler.currentPriority].cardList[cardIndex].IAPlay(targetIndex, nextTuto, timerd);
    }

    public void DrawCard(int cardIndex, int targetIndex, bool nextTuto, float timerd)
    {
        cardDrawer.DrawCard(cardIndex, targetIndex, nextTuto, timerd);
    }

    public void TutorialWindowRight(int textIndex, int proxyIndex, bool nextTuto, float timerd)
    {
        canvasManager.TutoPageRight.SetActive(true);
        canvasManager.TutoTextRight.text = canvasManager.TutoTexts[textIndex];
        canvasManager.tuto = nextTuto;
        canvasManager.timer = timerd;
    }

    public void TutorialWindowLeft(int textIndex, int proxyIndex, bool nextTuto, float timerd)
    {
        canvasManager.TutoPageLeft.SetActive(true);
        canvasManager.TutoTextLeft.text = canvasManager.TutoTexts[textIndex];
        canvasManager.tuto = nextTuto;
        canvasManager.timer = timerd;
    }

    public AnimationCurve animCurveToolTips;

    public void changeToolTip()
    {
        var startPos = toolTipText.transform.parent.position;
        var endPos = startPos;
        endPos.y -= 1;
        StartCoroutine(MoveAnimations.LerpToAnchor(startPos, endPos, animCurveToolTips, toolTipText.transform.parent, 1)) ;
        toolTipText.text = canvasManager.ToolTipsTexts[toolTipIndex];
        toolTipIndex++;
    }
}
