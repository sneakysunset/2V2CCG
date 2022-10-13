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

    public void SkipTurn()
    {
        if (handManager.Hands[1].actionTokenNumber == 0)
        {
            currentPriority += 2;
        }
        else
        {
            currentPriority += 1;
        }
    }

    void SkipAllyTurn()
    {
        currentPriority = 0;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SkipAllyTurn();
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

    public void PlayerActionFinished(int actionIndex)
    {
        print(1);
        if(actionIndex == 2)
        {
           TokenJ1T1.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(false);
        }
        else if(actionIndex == 1)
        {
            TokenJ1T1.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(false);
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

    void ActivateTokens(GameObject actionJ, GameObject attackJ)
    {
        

        actionJ.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(true);
        actionJ.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(true);

        attackJ.transform.Find("TokenSlot1/ActionToken").gameObject.SetActive(true);
        attackJ.transform.Find("TokenSlot2/ActionToken").gameObject.SetActive(true);
    }

}
