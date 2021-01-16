using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InteractionNpc : MonoBehaviour, IInteractable
{
    public bool isVideo = true;

    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;

    private int currentVideoClip = 0;

    [Header("Audio stuff")]
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private int currentAudioClip = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(isVideo) videoPlayer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDeInteract()
    {
        
    }

    public void OnInteract(Transform parent)
    {       
        if(isVideo)
        {
            // Play next video clip
            videoPlayer.clip = videoClips[currentVideoClip];
            videoPlayer.Play();
            currentVideoClip++;
            if(currentVideoClip == videoClips.Length) currentVideoClip = 0;
        }
        else
        {
            // Play next audio clip
            audioSource.clip = audioClips[currentAudioClip];
            audioSource.Play();
            currentAudioClip++;
            if(currentAudioClip == audioClips.Length) currentAudioClip = 0;
        }

        // No OnDeInteract
        parent.GetComponentInParent<Player>().itemPickedUp = null;
    }
}
