using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask layerMaskMoveToPosition;
    public float speed = 1;


    [Header("Components")]
    public Rigidbody rb;
    public Transform playerHand;

    private Vector3 target = Vector3.zero;

    private IInteractable itemPickedUp = null;

    /// <summary>
    /// Shows what way the player wants to move
    /// </summary>
    private Vector3 movementInputVector;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //MoveToPosition();
        Movement();
        PickUpInteraction();
    }

    private void FixedUpdate()
    {
        // Forward movement
        Vector3 fm = (rb.transform.forward * movementInputVector.z);
        // Sideway movement
        Vector3 sm = (rb.transform.right * movementInputVector.x);
        rb.MovePosition(transform.position + (fm + sm) * Time.fixedDeltaTime);
    }

    private void MoveToPosition()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            { 
                target = hit.point;
                Debug.Log("Did Hit");
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void Movement()
    {
        movementInputVector = new Vector3(Input.GetAxis("Horizontal") * speed, rb.velocity.y, Input.GetAxis("Vertical") * speed);
    }

    private void PickUpInteraction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Do we already have an item?
            if(itemPickedUp != null)
            {
                // Do something with item
                itemPickedUp.OnDeInteract();
                itemPickedUp = null;
            }
            else
            {
                // Pickup new item
                // Check for ray
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out hit, 2))
                {
                    // Ray hit check if component
                    if(hit.transform.GetComponent<IInteractable>() != null)
                    {
                        itemPickedUp = hit.transform.GetComponent<IInteractable>();
                        itemPickedUp.OnInteract(playerHand);
                    }
                    else
                    {
                        print("Nothing hit" + hit.transform.name);
                    }
                }
            }
        }
    }
}
