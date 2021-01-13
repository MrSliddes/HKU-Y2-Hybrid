using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deur : MonoBehaviour, IInteractable
{ 

    public bool isOpen = false;

    public Animator animator;
    public string animOpenName;
    public string animCloseName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDeInteract()
    {
        //throw new System.NotImplementedException();
    }

    public void OnInteract(Transform parent)
    {
        if(isOpen)
        {
            // Close
            animator.Play(animCloseName);
        }
        else
        {
            // Open
            animator.Play(animOpenName);
        }
        isOpen = !isOpen;
        parent.GetComponentInParent<Player>().itemPickedUp = null;
    }
}
