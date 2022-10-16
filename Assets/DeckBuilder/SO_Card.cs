using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/card", order = 1)]
public class SO_Card : ScriptableObject
{
    public enum cardType {spell, unit, equipment };
    public enum classType {Nain, Elf, Demon };

    public cardType typeDeCarte;
    public classType Class;

    public string nameT;

    [Range(0, 10)]
    public int attack, defense, spellPower;

    [Range(0, 3)]
    public int niveau;

    public Sprite Image;
}


