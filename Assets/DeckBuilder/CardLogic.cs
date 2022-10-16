using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardLogic : MonoBehaviour
{
    public SO_Card card_Data;
    [Space(100)]
    [Header("Components")]

    public Image cardImage;
    public Image cardFond, Star3, Star2, Star1;
    public GameObject UnitOrEquipmentStatsHolder, SpellHolder;
    public TextMeshProUGUI cardAttack, cardDefense, cardSpellValue, cardName, cardClass;

    public Sprite fondSpell, fondUnit, fondEquipment;
    public Color colorSpell, colorUnit, colorEquipment;
    public Color colorNain, colorElf, colorDemon;
    private void Start()
    {

    }

    [ContextMenu("UpdateCardValue")]
    public void UpdateCard()
    {
        switch (card_Data.typeDeCarte)
        {
            case SO_Card.cardType.equipment:
                cardFond.sprite = fondEquipment;
                cardFond.color = colorEquipment;
                cardAttack.text = "+" + card_Data.attack;
                cardDefense.text = "+" + card_Data.defense;
                break;
            case SO_Card.cardType.unit:
                cardFond.sprite = fondUnit;
                cardFond.color = colorUnit;
                cardAttack.text = "" + card_Data.attack;
                cardDefense.text = "" + card_Data.defense;
                break;
            case SO_Card.cardType.spell:
                cardFond.sprite = fondSpell;
                cardFond.color = colorSpell;
                cardSpellValue.text = "" + card_Data.spellPower;
                UnitOrEquipmentStatsHolder.SetActive(false);
                SpellHolder.SetActive(true);
                break;
            default:
                return;
        }

        switch (card_Data.Class)
        {
            case SO_Card.classType.Nain:
                cardClass.text = "Nain";
                Star1.color = colorNain;
                Star2.color = colorNain;
                Star3.color = colorNain;
                break;
            case SO_Card.classType.Elf:
                cardClass.text = "Elf";
                Star1.color = colorElf;
                Star2.color = colorElf;
                Star3.color = colorElf;
                break;
            case SO_Card.classType.Demon:
                cardClass.text = "Demon";
                Star1.color = colorDemon;
                Star2.color = colorDemon;
                Star3.color = colorDemon;

                break;
            default:
                return;
        }

        switch (card_Data.niveau)
        {
            case 0:
                break;
            case 1:
                Star1.gameObject.SetActive(true);
                Star2.gameObject.SetActive(false);
                Star3.gameObject.SetActive(false);
                break;
            case 2:
                Star1.gameObject.SetActive(false);
                Star2.gameObject.SetActive(true);
                Star3.gameObject.SetActive(false);
                break;
            case 3:
                Star1.gameObject.SetActive(false);
                Star2.gameObject.SetActive(false);
                Star3.gameObject.SetActive(true);
                break;

        }

        cardImage.sprite = card_Data.Image;
    }

    private void Update()
    {
        UpdateCard();
    }
}
