using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerPongCounterTrow : MonoBehaviour
{
    public Transform trowPoint;
    public Transform trowPointDirection;
    public GameObject prefabBeerPongBall;

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
            // Trow ball back random
            GameObject a = Instantiate(prefabBeerPongBall, trowPoint.position, Quaternion.identity);
            a.transform.SetParent(null);

            // Set rotation
           Vector3 tarDir = trowPointDirection.position - a.transform.position;
            Vector3 newDir = Vector3.RotateTowards(a.transform.forward, tarDir, 999f, 999f); // direction is kinda fucked
            a.transform.rotation = Quaternion.LookRotation(newDir);


            a.GetComponent<Rigidbody>().isKinematic = false;
            a.GetComponent<Rigidbody>().AddForce(a.transform.forward * Random.Range(3, 6), ForceMode.Impulse);
        }
    }
}