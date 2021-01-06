using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=gmnU2gw6AL0
public class AudioFeedbackTest : MonoBehaviour
{
    public float sensitivity = 100;
    public float loudness = 0;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_STANDALONE
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        audioSource.loop = true;
        audioSource.mute = false;
        audioSource.Play();
#endif
    }
}
