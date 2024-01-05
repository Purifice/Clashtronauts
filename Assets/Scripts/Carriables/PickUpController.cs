using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using UnityEngine.InputSystem;

public class PickUpController : MonoBehaviour
{
    [SerializeField]
    private Transform grabPoint;

    [SerializeField]
    private Transform rayPoint;

    [SerializeField]
    private float rayDistance;

    private Animator animator;

    public GameObject grabbedObject;

    public PlayerMovement playermovement;


    private int layerIndex;
    private int altLayerIndex;

    public bool equipped = false;

    



    // Start is called before the first frame update
    void Start()
    {
        
       
        layerIndex = LayerMask.NameToLayer("Pickables"); //sets "layerIndex" as the integer value representation of the Pickables layer
        altLayerIndex = LayerMask.NameToLayer("Grabbed");

        animator = playermovement.GetComponent<Animator>();

        if (!equipped)
        {
            //coll.isTrigger = false;
            Physics2D.IgnoreLayerCollision(7, 8, false);
        }

        if (equipped)
        {
            //coll.isTrigger = true;
            Physics2D.IgnoreLayerCollision(7, 8, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance); //sets the ray to the specified rayPoint object on the player
        
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex) 
        //if hitting something, and if that thing is what's specified in layerIndex (the Pickables layer)
        {
            if (playermovement.carryButton && grabbedObject == null) //if button to carry is pressed and nothing is grabbed
            {
                Debug.Log("That");

                Physics2D.IgnoreLayerCollision(6, 8, true);
                Physics2D.IgnoreLayerCollision(7, 8, true);

                animator.SetBool("isCarrying", true);

                grabbedObject = hitInfo.collider.gameObject; //set the grabbed object to what's being raydetected
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true; //turn it to kinematic
                grabbedObject.transform.localPosition = grabPoint.position; //move it to the grabpoint position
                grabbedObject.transform.SetParent(transform); //set the parent of the grabbedObjects transform to it's transform position
                equipped = true;


            }
            /*else if (playermovement.carryButton) //if button to carry is pressed and something is grabbed
            {
                Drop();
            }*/
        }
        
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == altLayerIndex) 
        //if colliding with the "Grabbed" layer 
        {
            if (playermovement.carryButton && grabbedObject != null) //allow dropping if carrying something
            {
                Debug.Log("This");
                Drop();
            }
        }

       if (equipped && playermovement.dove) //drop upon diving function
        {
            Drop();
        }

        if (transform.childCount < 0) //drop upon no longer possessing the carriable
        {
            Drop();
        }


       // Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }

  /* void PickUp()
    {

    }*/

    void Drop()
    {
        //Physics2D.IgnoreLayerCollision(6, 8, false);
        Physics2D.IgnoreLayerCollision(7, 8, false);

        animator.SetBool("isCarrying", false);

        grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
        grabbedObject.transform.SetParent(null);
        grabbedObject = null;
        equipped = false;

        

        /* rb2D.velocity = player.GetComponent<Rigidbody2D>().velocity;

         rb2D.AddForce(playermomentum.forward * dropForwardForce * -10f, ForceMode2D.Impulse);
         rb2D.AddForce(playermomentum.up * dropUpwardForce, ForceMode2D.Impulse);


             public Transform player;

         */

    }

}
