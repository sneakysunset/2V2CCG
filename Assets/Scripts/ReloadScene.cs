using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ReloadScene : MonoBehaviour
{
    public List<string> TutoTexts;
    public GameObject TutoPageLeft, TutoPageRight;
    public TextMeshProUGUI TutoTextLeft, TutoTextRight;
    public TutorialManager tutorialManager;
    bool tuto;
    public void ReloadSceneMethod()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TutorialWindowRight(int textIndex, int proxyIndex, bool nextTuto)
    {
        TutoPageRight.SetActive(true);
        TutoTextRight.text = TutoTexts[textIndex];
        tuto = nextTuto;
    }

    public void TutorialWindowLeft(int textIndex, int proxyIndex, bool nextTuto)
    {
        TutoPageLeft.SetActive(true);
        TutoTextLeft.text = TutoTexts[textIndex];
        tuto = nextTuto;
    }

    public void SkipWindow()
    {
        tutorialManager.tutorialIndex++;
        if(!tuto)
            tutorialManager.tutorialPlaying = false;
    }
}
