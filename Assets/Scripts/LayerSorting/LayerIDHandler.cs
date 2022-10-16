using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerIDHandler : MonoBehaviour
{
    void Start()
    {
        if(this.transform.parent.GetComponent<Renderer>())
            this.GetComponent<Renderer>().sortingLayerID = this.transform.parent.GetComponent<Renderer>().sortingLayerID;
    }
}
