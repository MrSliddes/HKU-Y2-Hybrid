using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHighlight : MonoBehaviour
{
    ShouldHighlight ShouldHighlight2 = null;
    //Component ShouldHighlight2;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 2))
        {
            if (hit.transform.GetComponent<IInteractable>() != null)
            {
                //highLightScript = hit.transform.transform.GetComponent<ShouldHighlight>();
                ShouldHighlight2 = hit.transform.GetComponent<ShouldHighlight>();
                ShouldHighlight2.shouldHighlightBool = true;
            }
            else
            {
                //ShouldHighlight2.shouldHighlightBool = false;
            }
        }
    }
}
