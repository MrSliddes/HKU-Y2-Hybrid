using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerPongBall : MonoBehaviour, IInteractable
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

    public void OnInteract(Transform parent)
    {
        transform.SetParent(parent);
        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
    }

    public void OnDeInteract()
    {
        transform.parent = null;

        // Cast ray, rot to ray end
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            Vector3 tarDir = hit.point - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, tarDir, 999f, 999f); // direction is kinda fucked
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        rb.isKinematic = false;
        rb.AddForce(transform.forward * 10, ForceMode.Impulse);
    }
}
