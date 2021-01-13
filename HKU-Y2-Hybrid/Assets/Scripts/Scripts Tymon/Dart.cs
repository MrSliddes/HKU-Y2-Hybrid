using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour, IInteractable
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!rb.isKinematic) transform.forward = Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.deltaTime);
    }

    public void Drop()
    {
        transform.parent = null;
        rb.isKinematic = false;
    }

    public void OnInteract(Transform parent)
    {
        // Turn colliders off
        if(GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = false;
        if(GetComponent<SphereCollider>() != null) GetComponent<SphereCollider>().enabled = false;

        transform.SetParent(parent);
        rb.isKinematic = true;
        print(transform.position);
        transform.localPosition = Vector3.zero;
        print(transform.position);
        // Rotate forwards
        Vector3 newDir = Vector3.RotateTowards(transform.forward, parent.forward, 999f, 0.0f); // direction is kinda fucked
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void OnDeInteract()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse);

        // colliders on
        if(GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = true;
        if(GetComponent<SphereCollider>() != null) GetComponent<SphereCollider>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
    }
}
