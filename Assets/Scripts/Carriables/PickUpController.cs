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

    private int layerIndex;
    private int altLayerIndex;

    private Rigidbody2D rigidbody2;
    private Rigidbody2D rb2D;

    private float dropForwardForce;
    private float dropUpwardForce;



    public Vector2 playermomentum;

    public GameObject grabbedObject;

    public PlayerMovement playermovement;

    public bool equipped = false;

    public float buffer = .1f;





    // Start is called before the first frame update
    void Start()
    {

        rb2D = GetComponentInParent<Rigidbody2D>();
        dropForwardForce = 2f;
        dropUpwardForce = .75f;

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

        buffer = .1f;
    }

    void Update()
    {
        if (buffer < 1 && equipped) //If the carry/throw time buffer hasn't started and something is equipped
            {
                buffer += 2f * Time.deltaTime; //gradually increase the buffer over time
             }
        if (buffer >= 1) // If the buffer has fully finished
            {
                buffer = 1f; //set it to its finished actionable state (can be thrown)
            }
        if (buffer == 1 && !equipped && playermovement.carryButton) //if the buffer has finished and the player has nothing equipped but is still pressing carry
        {
            buffer = .5f; //set the buffer to a non-actionable state (cannot carry or throw)
        }
        if (buffer <1 && buffer > .1 && !equipped) //then if the buffer is in a non-actionable state range
        {
            buffer -= 1.1f *Time.deltaTime; //gradually decrease the buffer over time
        }
        if (buffer <=.1) //then if the buffer has fully reset
        {
            buffer = .1f; //set it to its starting acitonable state (can be picked up)
        }
    
        playermomentum = rb2D.velocity;
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.position, rayDistance);
        //sets the ray to the specified rayPoint object on the player

            if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
            //if hitting something, and if that thing is what's specified in layerIndex (the Pickables layer)
            {
                if (playermovement.carryButton && grabbedObject == null && buffer ==.1f) //if button to carry is pressed and nothing is grabbed and buffer is reset
                {
                    Physics2D.IgnoreLayerCollision(6, 8, true);
                    Physics2D.IgnoreLayerCollision(7, 8, true);

                    animator.SetBool("isCarrying", true);

                    grabbedObject = hitInfo.collider.gameObject; //set the grabbed object to what's being raydetected
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true; //turn it to kinematic
                    grabbedObject.transform.localPosition = grabPoint.position; //move it to the grabpoint position
                    grabbedObject.transform.SetParent(transform); //set the parent of the grabbedObjects transform to it's transform position
                    rigidbody2 = GetComponentInChildren<Rigidbody2D>();
                    equipped = true;


                }
                /*else if (playermovement.carryButton) //if button to carry is pressed and something is grabbed
                {
                    Drop();
                }*/
            }

            //if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == altLayerIndex)
            //above gave issues dropping because hitinfo was detecting camera bounds not the sphere
            if (!playermovement.isClimbing && equipped && playermovement.facingFront)
            {
                if (playermovement.carryButton && grabbedObject != null && equipped && buffer == 1f) //allow dropping if carrying something and the buffer has hit max
                {
                    Drop();
                }
            }

            if (equipped && playermovement.dove) //drop upon diving function
            {
                Drop();
            }

            if (equipped && transform.childCount < 0) //drop upon no longer possessing the carriable
            {
                Drop();
            }

            //Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
            //Debug.Log(hitInfo.collider);        
    }

    void Drop()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
        animator.SetBool("isCarrying", false);
        grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
       // Debug.Log(playermomentum); 
        rigidbody2.AddForce(new Vector2(playermomentum.x * dropForwardForce, 0f), ForceMode2D.Impulse);
        rigidbody2.AddForce(new Vector2(0f, playermomentum.y * dropUpwardForce), ForceMode2D.Impulse);

        grabbedObject.transform.SetParent(null);
        grabbedObject = null;
        rigidbody2 = GetComponentInChildren<Rigidbody2D>();
        equipped = false;
        

        /* rb2D.velocity = player.GetComponent<Rigidbody2D>().velocity;

         rb2D.AddForce(playermomentum.forward * dropForwardForce * -10f, ForceMode2D.Impulse);
         rb2D.AddForce(playermomentum.up * dropUpwardForce, ForceMode2D.Impulse);


             public Transform player;

         */

    }

}
