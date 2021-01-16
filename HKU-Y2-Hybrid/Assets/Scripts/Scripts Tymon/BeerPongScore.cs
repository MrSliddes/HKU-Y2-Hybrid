using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerPongScore : MonoBehaviour
{
    public Transform ballResetPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BeerPongBall"))
        {
            print("BeerPong: Score!");
            other.transform.parent.position = ballResetPoint.position; // Shit parent comp
            other.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        else
        {
            print(other.name);
        }
    }
}
