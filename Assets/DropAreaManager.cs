using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAreaManager : MonoBehaviour
{
    public static DropAreaManager instance;
    public void Awake()
    {
        instance = this;
    }

    GameObject[] dropAreas;
    public bool isActive;
    private void Update()
    {
        if (isActive)
        {
            foreach (var dropAreaCanvasGroup in dropAreas)
            {
                dropAreaCanvasGroup.SetActive(true);
            }
        } 
    }
}
