using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerPongBall : MonoBehaviour, IAbleToPickUp
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {        
        transform.parent = null;
        rb.isKinematic = false;
    }

    public void PickUp(Transform parent)
    {
        transform.SetParent(parent);
        rb.isKinematic = true;
        print(transform.position);
        transform.localPosition = Vector3.zero;
        print(transform.position);
    }

    public void Trow()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10, ForceMode.Impulse);
    }
}
