using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardSlotsManager : MonoBehaviour
{
    public Card[] boardSlot = new Card[3];
    public Transform[] boardSlotTransform = new Transform[3];
    public TextMesh nexusHPT1, nexusHPT2;

    public void AttackCard(int cardIndex, int targetIndex, bool nextTuto, float timerd)
    {
        boardSlot[cardIndex].IAAttack(targetIndex, nextTuto, timerd);
    }



    public void AttackNexus(int attack, bool targetT1)
    {
        if (targetT1)
        {
            int nexusHP = int.Parse(nexusHPT1.text) - attack;
           if(nexusHP <= 0)
            {
                print("T2 Win");
            }
            nexusHPT1.text = nexusHP.ToString();
        }
        else
        {
            int nexusHP = int.Parse(nexusHPT2.text) - attack;
            if (nexusHP <= 0)
            {
                print("T1 Win");
            }
            nexusHPT2.text = nexusHP.ToString();
        }
    }
}
