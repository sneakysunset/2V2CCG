using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PriorityHandler : MonoBehaviour
{
    public GameObject TokenJ1T1, TokenJ2T1, TokenJ1T2, TokenJ2T2;
    public HandManager handManager;
    public int currentPriority;
    public Image PriorityButtonImage;
    public Button PriorityButton;
    public TextMeshProUGUI PriorityButtonText;
    private void Start()
    {
        GiveTokens(0);
    }

    public void GiveTokens(int playerIndex)
    {
        DisableTokens();
        switch (playerIndex)
        {
            case 0:
                ActivateTokens(TokenJ1T1, TokenJ2T1);
                handManager.Hands[0].actionTokenNumber = 2;
                handManager.Hands[1].actionTokenNumber = 2;
                currentPriority = 0;
                break;
            case 1:
                ActivateTokens(TokenJ2T1, TokenJ1T1);
                handManager.Hands[1].actionTokenNumber = 2;
                handManager.Hands[0].actionTokenNumber = 2;
                currentPriority = 1;
                break;
            case 2:
                ActivateTokens(TokenJ1T2, TokenJ2T2);
                handManager.Hands[2].actionTokenNumber = 2;
                handManager.Hands[3].actionTokenNumber = 2;
                currentPriority = 2;
                break;
            case 3:
                ActivateTokens(TokenJ2T2, TokenJ1T2);
                handManager.Hands[3].actionTokenNumber = 2;
                handManager.Hands[2].actionTokenNumber = 2;
                currentPriority = 3;
                break;
            default:
                return;
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SkipTurn();
        }

        if (currentPriority != 0)
        {
            PriorityButton.interactable = false;
            //PriorityButtonImage.color = grey
            if (currentPriority == 1)
            {
                PriorityButtonText.text = "Ally Turn";
            }
            else if (currentPriority > 1)
            {
                PriorityButtonText.text = "Ennemy Turn";
            }
        }
        else
        {
            PriorityButton.interactable = true;
            if (handManager.Hands[1].actionTokenNumber == 0)
            {
                PriorityButtonText.text = "End turn";
            }
            else
            {
                PriorityButtonText.text = "Skip";
            }
        }
    }


    void DisableTokens()
    {
        TokenJ1T1.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(false);
        TokenJ1T1.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(false);

        TokenJ2T1.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(false);
        TokenJ2T1.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(false);

        TokenJ1T2.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(false);
        TokenJ1T2.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(false);

        TokenJ2T2.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(false);
        TokenJ2T2.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(false);
    }

    void ActivateTokens(GameObject actionJ1, GameObject actionJ2)
    {
        actionJ1.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(true);
        actionJ1.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(true);

        actionJ2.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(true);
        actionJ2.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(true);
    }


    public void ActionFinished(int actionIndex)
    {
        switch (currentPriority)
        {
            case 0:
                DisableToken(actionIndex, TokenJ1T1);
                if (handManager.Hands[1].actionTokenNumber == 0 && handManager.Hands[0].actionTokenNumber == 1)
                {
                    currentPriority = 2;
                    DisableTokens();
                    ActivateTokens(TokenJ1T2, TokenJ2T2);
                    handManager.Hands[2].actionTokenNumber = 2;
                    handManager.Hands[3].actionTokenNumber = 2;
                    handManager.Hands[1].actionTokenNumber = 0;
                    handManager.Hands[0].actionTokenNumber = 0;
                }
                else currentPriority = 1;
                break;
            case 1:
                DisableToken(actionIndex, TokenJ2T1);
                if (handManager.Hands[1].actionTokenNumber == 1 && handManager.Hands[0].actionTokenNumber == 0) 
                {
                    currentPriority = 3;
                    DisableTokens();
                    ActivateTokens(TokenJ2T2, TokenJ1T2);
                    handManager.Hands[2].actionTokenNumber = 2;
                    handManager.Hands[3].actionTokenNumber = 2;
                    handManager.Hands[1].actionTokenNumber = 0;
                    handManager.Hands[0].actionTokenNumber = 0;
                } 
                else currentPriority = 0;
                break;
            case 2:
                DisableToken(actionIndex, TokenJ1T2);
                if (handManager.Hands[2].actionTokenNumber == 1 && handManager.Hands[3].actionTokenNumber == 0)
                {
                    currentPriority = 0;
                    DisableTokens();
                    ActivateTokens(TokenJ1T1, TokenJ2T1);
                    handManager.Hands[1].actionTokenNumber = 2;
                    handManager.Hands[0].actionTokenNumber = 2;
                    handManager.Hands[2].actionTokenNumber = 0;
                    handManager.Hands[3].actionTokenNumber = 0;
                }
                else currentPriority = 3;
                break;
            case 3:
                DisableToken(actionIndex, TokenJ2T2);
                if (handManager.Hands[2].actionTokenNumber == 0 && handManager.Hands[3].actionTokenNumber == 1)
                {
                    currentPriority = 1;
                    DisableTokens();
                    ActivateTokens(TokenJ2T1, TokenJ1T1);
                    handManager.Hands[1].actionTokenNumber = 2;
                    handManager.Hands[0].actionTokenNumber = 2;
                    handManager.Hands[2].actionTokenNumber = 0;
                    handManager.Hands[3].actionTokenNumber = 0;
                }
                else currentPriority = 2;
                break;
            default:
                return;
        }
    }

    private void DisableToken(int actionIndex, GameObject TokenG)
    {
        if (actionIndex == 2)
        {
            TokenG.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(false);
        }
        else if (actionIndex == 1)
        {
            TokenG.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(false);
        }
    }


    public void SkipTurn()
    {
        if(currentPriority == 0 || currentPriority == 2)
        {
            if (handManager.Hands[currentPriority + 1].actionTokenNumber == 0)
            {
                if (currentPriority == 0)
                {
                    currentPriority += 2; // End Turn J1T1
                    DisableTokens();
                    ActivateTokens(TokenJ1T2, TokenJ2T2);
                    handManager.Hands[2].actionTokenNumber = 2;
                    handManager.Hands[3].actionTokenNumber = 2;
                    handManager.Hands[1].actionTokenNumber = 0;
                    handManager.Hands[0].actionTokenNumber = 0;
                }
                else
                {
                    currentPriority -= 2; // End Turn End Turn J1T2
                    DisableTokens();
                    ActivateTokens(TokenJ1T1, TokenJ2T1);
                    handManager.Hands[0].actionTokenNumber = 2;
                    handManager.Hands[1].actionTokenNumber = 2;
                    handManager.Hands[2].actionTokenNumber = 0;
                    handManager.Hands[3].actionTokenNumber = 0;
                }
            }
            else
            {
                currentPriority += 1; // Skip J1T1 || J1T2
            }
        }
        else
        {
            if (handManager.Hands[currentPriority - 1].actionTokenNumber == 0)
            {
                if (currentPriority == 1)
                {
                    currentPriority += 2; // End Turn J2T1
                    DisableTokens();
                    ActivateTokens(TokenJ1T2, TokenJ2T2);
                    handManager.Hands[2].actionTokenNumber = 2;
                    handManager.Hands[3].actionTokenNumber = 2;
                    handManager.Hands[1].actionTokenNumber = 0;
                    handManager.Hands[0].actionTokenNumber = 0;
                }
                else
                {
                    DisableTokens();
                    currentPriority -= 2; // End Turn J2T2
                    ActivateTokens(TokenJ1T1, TokenJ2T1);
                    handManager.Hands[0].actionTokenNumber = 2;
                    handManager.Hands[1].actionTokenNumber = 2;
                    handManager.Hands[2].actionTokenNumber = 0;
                    handManager.Hands[3].actionTokenNumber = 0;

                } 
            }
            else
            {
                currentPriority -= 1;//Skip J2T1 || J2T2
            }
        }
    }

}
