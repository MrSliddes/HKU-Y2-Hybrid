using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMusic : MonoBehaviour
{
    public static ManagerMusic Instance { get; set; }

    public AudioSource[] speakers;

    public AudioClip[] songs;

    public bool isPlayingNormal;
    private AudioClip currentClip;

    public AudioSource songSBNormal0;
    public AudioSource songSBNormal1;
    public AudioSource songSBBackroom0;
    public AudioSource songSBBackroom1;
    public AudioSource partBackgroundNoise;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetMusic(0);
        PlayNormalMusic(true);
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

    public void PlayNormalMusic(bool v)
    {
        isPlayingNormal = v;
        if(v)
        {
            songSBNormal0.mute = false;
            songSBNormal1.mute = false;
            songSBBackroom0.mute = true;
            songSBBackroom1.mute = true;
            partBackgroundNoise.volume = 0.5f;
        }
        else
        {
            songSBNormal0.mute = true;
            songSBNormal1.mute = true;
            songSBBackroom0.mute = false;
            songSBBackroom1.mute = false;
            partBackgroundNoise.volume = 0.1f;
        }
    }
}
