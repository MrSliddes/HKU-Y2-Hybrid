using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoHolderMenu : MonoBehaviour
{
    public static string theName;
    public void safeName(string nameString)
    {
        theName = nameString;
    }

    private void Update()
    {
        
    }

    /*public static string Name   // property
    {
        get { return theName; }   // get method
        set { theName = value; }  // set method
    }*/
}
