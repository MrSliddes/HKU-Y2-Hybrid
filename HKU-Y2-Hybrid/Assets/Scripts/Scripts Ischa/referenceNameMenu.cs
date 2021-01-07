using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class referenceNameMenu : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = InfoHolderMenu.theName; 
    }
}
