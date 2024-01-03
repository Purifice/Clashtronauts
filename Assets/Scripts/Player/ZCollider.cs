using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCollider : MonoBehaviour
{
    private Quaternion targetPosition;
    public PlayerMovement playermovement;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = Quaternion.Euler (45, -90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (transform.localRotation != targetPosition)
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }*/

    }
    void OnTriggerStay2D(Collider2D collision) //every frame where Collider2D is activating a trigger collision
    {
        if (collision.gameObject.tag == "Surface") //every frame upon the surface:
        {
            /*playermovement.isJumping = false; //allow for jumping
            playermovement.isDiving = false; //allow for normal movement
            playermovement.speed = 12.5f; //default ground movement speed
            playermovement.notGrounded = false;
            playermovement.animator.SetBool("isInAir", false);
            playermovement.animator.SetBool("isDiving", false);
            if (!playermovement.isClimbing)
            {
                playermovement.canFlip = true;
            }*/

            //transform.Rotate(0, 90, 0);
        }
    }
}
