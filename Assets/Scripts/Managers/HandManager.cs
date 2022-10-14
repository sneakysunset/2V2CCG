using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hand  
{
    public List<Transform> handSlots;
    public List<bool> occupied;
    /*[HideInInspector]*/ public Card[] cardList = new Card[5];
    public int actionTokenNumber = 0;

    public int dwarfLvlIndex;
    public int elfLvlIndex;
    public int demonLvlIndex;

    public HandManager.playerNum pNum;
    public HandManager.teamNum tNum;
}

public class HandManager : MonoBehaviour
{
    public Hand[] Hands = new Hand[4];
    public enum playerNum { J1, J2 };
    public enum teamNum { T1, T2 }
    public PriorityHandler priorityHandler;
    public TutorialManager tutorialManager;
    public void PlayCard(int cardIndex, int targetIndex, bool nextTuto, float timerd)
    {
        Hands[priorityHandler.currentPriority].cardList[cardIndex].IAPlay(targetIndex, nextTuto, timerd);
    }

}
