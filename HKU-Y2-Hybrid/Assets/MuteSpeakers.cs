using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteSpeakers : MonoBehaviour, IInteractable
{
    public GameObject[] objectsToMute;

    private bool isOn = false;


    public void OnDeInteract()
    {
        // Nothing
    }

    public void OnInteract(Transform parent)
    {
        print("Mute the Audio");
        isOn = !isOn;
        foreach (GameObject item in objectsToMute)
        {
            item.GetComponentInChildren<AudioSource>().mute = isOn;
        }
        FindObjectOfType<Player>().itemPickedUp = null; //zodat in Player script niet itemPickedUp = true; terwijl je met de knop niks oppakt.
    }
}
