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
    [HideInInspector] public bool tuto;
    [HideInInspector] public float timer;
    
    public void ReloadSceneMethod()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TutorialWindowRight(int textIndex, int proxyIndex, bool nextTuto, float timerd)
    {
        TutoPageRight.SetActive(true);
        TutoTextRight.text = TutoTexts[textIndex];
        tuto = nextTuto;
        timer = timerd;
    }

    public void TutorialWindowLeft(int textIndex, int proxyIndex, bool nextTuto, float timerd)
    {
        TutoPageLeft.SetActive(true);
        TutoTextLeft.text = TutoTexts[textIndex];
        tuto = nextTuto;
        timer = timerd;
    }

    public void SkipWindow()
    {
        tutorialManager.tutorialIndex++;
        tutorialManager.timer = timer;
        if(!tuto)
            tutorialManager.tutorialPlaying = false;
        else tutorialManager.canPlay = true;
    }


}
