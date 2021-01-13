using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouldHighlight : MonoBehaviour
{
    [HideInInspector]public bool shouldHighlightBool = false;
    public Outline theOutline;
    //[SerializeField] Outline theOutline;

    // Update is called once per frame
    void Update()
    {
        if (shouldHighlightBool == true) //this happes if hitting
        {
            //Debug.Log("Highlight");
            theOutline.enabled = true;
        }
        else
        {
            theOutline.enabled = false;
            //Debug.Log("No highlight");
        }
        shouldHighlightBool = false;
    }
}
