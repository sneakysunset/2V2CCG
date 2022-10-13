using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hand  
{
    public List<Transform> handSlots;
    public List<bool> occupied;

    public int actionTokenNumber = 0;


    public HandManager.playerNum pNum;
    public HandManager.teamNum tNum;
}

public class HandManager : MonoBehaviour
{
    public Hand[] Hands = new Hand[4];
    public enum playerNum { J1, J2 };
    public enum teamNum { T1, T2 }

}
