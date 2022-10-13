using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct unityEvent
{
    public string TutorialStep;
    public UnityEvent<int, int, bool> tutorialCall;
    public int handSlotTargetOrDrawPlayerIndex;
    public int slotTargetOrDrawNumber;
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



    private void Update()
    {
        if(!tutorialPlaying /*&& priorityHandler.currentPriority != 0*/)
        {
            tutorialPlaying = true;
            StartCoroutine(SimpleTimer());
        }
    }

    IEnumerator SimpleTimer()
    {
        yield return new WaitForSeconds(1);
        tutorialEvents[tutorialIndex].tutorialCall.Invoke(tutorialEvents[tutorialIndex].handSlotTargetOrDrawPlayerIndex, tutorialEvents[tutorialIndex].slotTargetOrDrawNumber, tutorialEvents[tutorialIndex].nextTuto);
    }

    public void TutoTrigger()
    {
        tutorialPlaying = false;
    }
}
