using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpecificMusic : MonoBehaviour
{
    public bool playNormal;

    public Deur deur;
    public bool deurNeedsToBeOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(deur != null)
            {
                ManagerMusic.Instance.PlayNormalMusic(deur.isOpen);
                return;
            }

            if(ManagerMusic.Instance.isPlayingNormal != playNormal)
            {
                ManagerMusic.Instance.PlayNormalMusic(playNormal);

                
            }
        }
    }
}
