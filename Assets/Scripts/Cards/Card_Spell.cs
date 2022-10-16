using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Card_Spell : Card
{
    public int spellStrength;
    BoardSlotsManager boardSlotsManager;
    public bool targetNexus;
    public GameObject spellProjectile;

    [HideInInspector] public UnityEvent endCoroutineEffect;
    [HideInInspector] public UnityEvent IAendCoroutineEffect;
    public override void Start()
    {
        this.endCoroutineEffect.AddListener(() => this.EndCoroutineEffect());
        this.IAendCoroutineEffect.AddListener(() => this.IAEndCoroutineEffect());
        boardSlotsManager = FindObjectOfType<BoardSlotsManager>();
        base.Start();
    }


    #region playerAction
    public GameObject arrowPointer;
    Transform arrowHeadPos;
    LineRenderer arrowLineRenderer;
    Transform projectile;
    List<GameObject> highlights = new List<GameObject>();

    private void OnMouseDown()
    {
        if (handSlotIndex != -1 && priorityHandler.currentPriority == 0 && tutorialManager.canPlay && !tutorialManager.pause && lvlCondition >= level)
        {
            arrowHeadPos = Instantiate(arrowPointer, transform).transform;
            arrowLineRenderer = arrowHeadPos.GetComponent<LineRenderer>();


            for (int i = 0; i < boardSlotsManager.boardSlot.Length; i++)
            {
                string[] intelliName = boardSlotsManager.boardSlotTransform[i].name.Split(" : ");


                if (intelliName[2] == "T2" && boardSlotsManager.boardSlot[i])
                {
                    boardSlotsManager.boardSlotTransform[i].Find("HighLight").gameObject.SetActive(true);
                    highlights.Add(boardSlotsManager.boardSlotTransform[i].Find("HighLight").gameObject);
                    if (targetNexus)
                    {
                        boardSlotsManager.nexusHPT2.transform.Find("HighLight").gameObject.SetActive(true);
                        highlights.Add(boardSlotsManager.nexusHPT2.transform.Find("HighLight").gameObject);
                    }
                }
            }
        }
    }

    public void OnMouseEnter()
    {
        base.OnMouseEnterEvent();
    }

    public void OnMouseExit()
    {
        base.OnMouseExitEvent();
    }


    private void  Update()
    {
        base.UpdateLevelindex();
        if (arrowHeadPos != null)
        {
            SpellArrowPointer();
        }
    }

    Card_Unit spellTarget;
    Transform spellTargetTransform;
    int colliName;

    void SpellArrowPointer()
    {
        arrowLineRenderer.SetPosition(0, transform.position);
        arrowLineRenderer.SetPosition(1, arrowHeadPos.position);
        arrowHeadPos.rotation = Quaternion.LookRotation(Vector3.forward, arrowHeadPos.position - transform.position);
        LayerMask layerMask = LayerMask.GetMask("Slot");
        if (targetNexus) layerMask = LayerMask.GetMask("Slot") + LayerMask.GetMask("Nexus");

        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, layerMask);

        if (rayHit)
        {
            string[] collName = rayHit.transform.name.Split(" : ");
            colliName = int.Parse(collName[0]);

            bool conditionNexus = colliName == 10 && int.Parse(collName[1]) == 2;
            if (conditionNexus || (collName[1] == "BoardSlot" && collName[2] != teamNum.ToString() && boardSlotsManager.boardSlot[colliName] != null))
            {
                arrowHeadPos.position = rayHit.transform.position;
                if (Input.GetMouseButtonUp(0))
                {
                    if(colliName == 10)
                    {
                        spellTargetTransform = boardSlotsManager.nexusHPT2.transform.parent;
                    }
                    else
                    {
                        spellTarget = boardSlotsManager.boardSlot[colliName].GetComponent<Card_Unit>();
                        spellTargetTransform = boardSlotsManager.boardSlotTransform[colliName];
                    }
                    castSpell();
                }
                return;
            }

        }
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        arrowHeadPos.position = pos;


        if (Input.GetMouseButtonUp(0))
        {
            foreach (GameObject highlight in highlights)
            {
                highlight.SetActive(false);
            }
            highlights.Clear();
            Destroy(arrowHeadPos.gameObject);
        }

    }

    void castSpell()
    {
        Destroy(arrowHeadPos.gameObject);
        projectile = Instantiate(spellProjectile, transform.position, Quaternion.identity).transform;
        
        projectile.rotation = Quaternion.LookRotation(Vector3.forward, spellTargetTransform.position - transform.position);
        projectile.Rotate(0, 0, -90);
        StartCoroutine(MoveAnimations.LerpToAnchor(projectile.position, spellTargetTransform.position, anchoringAnimCurve, projectile, .4f, endCoroutineEffect));
    }

    void EndCoroutineEffect()
    {
        Destroy(projectile.gameObject);
        foreach (GameObject highlight in highlights)
        {
            highlight.SetActive(false);
        }
        highlights.Clear();
        if (colliName == 10)
        {
            boardSlotsManager.AttackNexus(spellStrength, false);
        }
        else spellTarget.ChangeStat(0, -spellStrength);

        base.CardUsed();
        tutorialManager.tutorialPlaying = false;
        Destroy(this.gameObject);
    }

    #endregion

    #region IAAction

    bool tuto;
    float timer;
    bool targetNexus1;
    int targetIndexr;
    public override void IAPlay(int targetIndex, bool nextTuto, float timerd)
    {
        tuto = nextTuto;
        timer = timerd;
        targetIndexr = targetIndex;
        if(targetIndex != 10)
        {
            spellTarget = boardSlotsManager.boardSlot[targetIndex].GetComponent<Card_Unit>();
            spellTargetTransform = boardSlotsManager.boardSlotTransform[colliName];
        }
        else
        {
            if(teamNum == HandManager.teamNum.T1)
            {
                spellTargetTransform = boardSlotsManager.nexusHPT2.transform.parent;
                targetNexus1 = false;
            }
            else
            {
                spellTargetTransform = boardSlotsManager.nexusHPT1.transform.parent;
                targetNexus1 = true;
            }
        }

        projectile = Instantiate(spellProjectile, transform.position, Quaternion.identity).transform;
        projectile.rotation = Quaternion.LookRotation(Vector3.forward, spellTargetTransform.position - transform.position);
        projectile.Rotate(0, 0, -90);

        StartCoroutine(MoveAnimations.LerpToAnchor(projectile.position, spellTargetTransform.position, anchoringAnimCurve, projectile, .4f, IAendCoroutineEffect));
    }

    void IAEndCoroutineEffect()
    {
        Destroy(projectile.gameObject);
        if(targetIndexr != 10)
        {
            spellTarget.ChangeStat(0, -spellStrength);
        }
        else
        {
            boardSlotsManager.AttackNexus(spellStrength, targetNexus1);
        }
        base.CardUsed();
        tutorialManager.timer = timer;
        tutorialManager.tutorialIndex++;
        if(!tuto)
            tutorialManager.tutorialPlaying = false;
        else
        {
            tutorialManager.canPlay = true;
            tutorialManager.changeToolTip();
        }
        Destroy(this.gameObject);

    }

    #endregion
}
