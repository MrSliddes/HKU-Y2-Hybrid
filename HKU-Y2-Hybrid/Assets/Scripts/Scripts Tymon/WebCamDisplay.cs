﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamDisplay : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    public Material matWebcam;
    public Material matWebcamUI;

    // Start is called before the first frame update
    void Start()
    {
        webCamTexture = new WebCamTexture();
        //this.GetComponent<MeshRenderer>().material.mainTexture = webCamTexture;
        matWebcam.mainTexture = webCamTexture;
        if(matWebcamUI != null) matWebcamUI.mainTexture = webCamTexture;
        StartCoroutine(CheckCam());
    }

    IEnumerator CheckCam()
    {
        while(true)
        {
            if(!webCamTexture.isPlaying) webCamTexture.Play();
            yield return new WaitForSeconds(5f);
        }
    }
}
