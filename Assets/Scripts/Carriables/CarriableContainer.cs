using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarriableContainer : MonoBehaviour
{
    public PlayerMovement playermovement;
    public PickUpController pickupcontroller;

    private Array coll2D;
    private Collider2D collA;
    private Collider2D collB;


    // Start is called before the first frame update
    void Start()
    {
       var coll2D = GetComponents<Collider2D>();

        collA = coll2D[0];
        collB = coll2D[1];

        collA.enabled = false;
        collB.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playermovement.facingFront)
        {
            transform.localPosition = new Vector3(-.8f, -0.2f, 0);
        }
       
        else if (playermovement.facingFront)
        {

            if (!playermovement.isJumping)
            {
                transform.localPosition = new Vector3(.8f, -.35f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(.75f, -.575f, 0);
            }
        }

        if (pickupcontroller.equipped && playermovement.facingFront)
        {
            collA.enabled = true;
            collB.enabled = true;

        }
        else if (!pickupcontroller.equipped || !playermovement.facingFront)
        {
            collA.enabled = false;
            collB.enabled = false;

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            playermovement.speed = 0;
        }
/*
        if (collision.gameObject.tag == "Player")
        {
            playermovement.isJumping = false; //allow for jumping
            playermovement.isDiving = false; //allow for normal movement
            playermovement.speed = 12.5f; //default ground movement speed
            playermovement.animator.SetBool("isInAir", false);
            playermovement.animator.SetBool("isDiving", false);
            if (!playermovement.isClimbing)
            {
                playermovement.canFlip = true;
            }
        }*/
    }
}
