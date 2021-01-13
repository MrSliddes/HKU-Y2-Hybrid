using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tv : MonoBehaviour
{
    public GameObject tv;
    public Rigidbody rb;
    public AudioSource audioSource;

    private bool hasFallen = false;

    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Dart>() != null)
        {
            // falloff
            rb.isKinematic = false;
            if(!hasFallen) audioSource.Play();
            tv.SetActive(false);
            hasFallen = true;
        }
    }
}
