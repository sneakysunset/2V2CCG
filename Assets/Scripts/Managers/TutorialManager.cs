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
    public PriorityHandler priorityHandler;
    public List<unityEvent> tutorialEvents;
    public int tutorialIndex;
    public bool tutorialPlaying;
    public bool canPlay;
    public float timer;
    public bool pause;

    private void Update()
    {
        if(!tutorialPlaying /*&& priorityHandler.currentPriority != 0*/)
        {
            tutorialPlaying = true;
            canPlay = false;
            StartCoroutine(SimpleTimer(timer));
        }
    }



    IEnumerator SimpleTimer(float timer)
    {
        yield return new WaitUntil(()=> !pause);
        yield return new WaitForSeconds(timer);
        tutorialEvents[tutorialIndex].tutorialCall.Invoke(tutorialEvents[tutorialIndex].handSlotTargetOrDrawPlayerIndex, tutorialEvents[tutorialIndex].slotTargetOrDrawNumber, tutorialEvents[tutorialIndex].nextTuto, tutorialEvents[tutorialIndex].timerd);
    }

    public void TutoTrigger()
    {
        tutorialPlaying = false;
    }

    public void PauseGame(bool boolean)
    {
        pause = boolean;
    }
}
