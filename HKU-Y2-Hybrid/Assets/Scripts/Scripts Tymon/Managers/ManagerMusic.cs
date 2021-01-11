using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMusic : MonoBehaviour
{
    public AudioSource[] speakers;

    public AudioClip[] songs;

    private AudioClip currentClip;

    // Start is called before the first frame update
    void Start()
    {
        SetMusic(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusic(int index)
    {
        switch(index)
        {
            case 0:
                currentClip = songs[0];
                break;
            default: Debug.LogWarning("Music index not found!");
                break;
        }

        // Set song for all speakers        
        for(int i = 0; i < speakers.Length; i++)
        {
            speakers[i].clip = currentClip;
            speakers[i].Play();
        }
        
    }
}
