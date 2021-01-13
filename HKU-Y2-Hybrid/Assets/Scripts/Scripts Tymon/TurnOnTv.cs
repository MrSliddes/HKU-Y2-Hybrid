using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnTv : MonoBehaviour, IInteractable
{
    public GameObject[] objectsToTurnOff;
    public GameObject[] objectsToTurnOn;

    private bool isOn = false;

    private void Start()
    {
        foreach(GameObject item in objectsToTurnOn)
        {
            item.SetActive(false);
        }
    }

    public void OnDeInteract()
    {
        // Nothing
    }

    public void OnInteract(Transform parent)
    {
        print("interact with tv");
        isOn = !isOn;
        foreach(GameObject item in objectsToTurnOff)
        {
            item.SetActive(!isOn);
        }
        foreach(GameObject item in objectsToTurnOn)
        {
            item.SetActive(isOn);
        }
        FindObjectOfType<Player>().itemPickedUp = null;//zodat in Player script niet itemPickedUp = true; terwijl je met de knop niks oppakt.
    }
}
