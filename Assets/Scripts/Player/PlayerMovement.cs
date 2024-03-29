using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb2D;
    public Animator animator;


    public float speed;
    public float jump;
    public float dampener =.1f;
    

    //private float moveHorizontal;
    //private float moveVertical;
    private float vertical; //for ladder movement
    


    public bool isJumping;
    public bool isLadder; //touching ladder, referenced by external script
    public bool isClimbing;
    public bool isDiving;
    public bool facingFront = true; //for ladder directional
    public bool carryButton = false;
    public bool throwButton = false;
    public bool dove = false;
    public bool canFlip = true;



    private bool facingRight = true; //always spawns assuming it's facing right
    private bool hasDove;
    private bool isMoving;
    private bool jumped = false;
    private bool climbed = false;
    public bool notGrounded;


    private Vector2 movementInput = Vector2.zero;
    

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        speed = 12.5f;
        jump = 20f;
        dampener = .1f;
        isJumping = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>(); //reads the input value of the L/R movement
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered; //reads whether or not the set jump button has been pressed
    }
    public void OnClimb(InputAction.CallbackContext context)
    {
        climbed = context.action.triggered; //reads whether or not the climb button is being triggered
    }
    public void OnDive(InputAction.CallbackContext context)
    {
        dove = context.action.triggered; //reads whether or not the dive button is being triggered
    }
    public void OnCarry(InputAction.CallbackContext context)
    {
        carryButton = context.action.triggered; //reads whether or not the carry button is being triggered
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        throwButton = context.action.triggered; //reads whether or not the carry button is being triggered
    }

    // Update is called once per frame
    void Update()
    {
     //   moveHorizontal = Input.GetAxisRaw("Horizontal"); //deprecated
     //   moveVertical = Input.GetAxisRaw("Vertical"); //deprecated
        vertical = Input.GetAxis("Vertical"); // for determining a smoother vertical movement occurence

       if (dove) //if pressing the dive key
        {
            hasDove = true;
            animator.SetBool("isDiving", true); //start animation

            canFlip = false;
        }
        else
        {
            hasDove = false;

        }

        if (!isJumping && !isClimbing && hasDove) //if on the ground and pressing the dive button
        {
            isDiving = true;

            if (facingRight)
            {
                rb2D.velocity = new Vector2(12.5f, 15f); 
            }
            else
            {
                rb2D.velocity = new Vector2(-12.5f, 15f);
            }
        }
        else if (isJumping && !isClimbing && hasDove) //if in air while pressing dive button
        {
            isDiving = true;

            animator.SetBool("isDiving", true); //start the animation
        }
        else //if not diving
        {

        }

        if (dampener < 1) // gradually unlimits the movement speed for a smooth acceleration
        {
            dampener += 2f * Time.deltaTime;
        }
        else if (dampener > 1)
        {
            dampener = 1f;
        }
    }

    // Fixed Update is called every fixed-rate frame, and is best for physics
    void FixedUpdate() 
    {


        if (!isDiving && (movementInput.x > 0f || movementInput.x < -0f))
        {
            //rb2D.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
            //The above movement method applies a force constantly as the key is pressed, good for 0g?
            rb2D.velocity = new Vector2(movementInput.x * (dampener * speed), rb2D.velocity.y);
            //the above calculates movement off the new input system but doesn't return the smoothing of getaxis
            //rb2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb2D.velocity.y);
            animator.SetBool("isWalking", true);
            isMoving = true;
        }
        else if(!isDiving && (movementInput.x <= 0f || movementInput.x >= -0f))
        {
            dampener = .1f;
            animator.SetBool("isWalking", false);
            isMoving = false;
        }
        else
        {
            animator.SetBool("isWalking", false);
            isMoving = false;
        }

        if (!isJumping && jumped)
        {
            // rb2D.AddForce(new Vector2(0f, moveVertical * jump), ForceMode2D.Impulse);
            //The above method uses the applying force method
            rb2D.velocity = new Vector2(rb2D.velocity.x, jump);
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);

        }



    

       // if (isLadder && Mathf.Abs(vertical) > 0f) //if touching ladder and moving up
        if (isLadder && climbed) 
            {
            isClimbing = true;
            animator.SetBool("isClimbing", true);

        }
        else
        {
            isClimbing = false;
            animator.SetBool("isClimbing", false);

        }

        if (isClimbing)
        {
            if (isJumping)
            {
                animator.SetBool("isDiving", false);
            }

            rb2D.gravityScale = 0f; //sets gravity to 0
            rb2D.velocity = new Vector2(rb2D.velocity.x, vertical * speed); //moves up ladder
            
        }
        else
        {
            rb2D.gravityScale = 5f; //reset gravity to default if not climbing

        }

        if (isClimbing && facingFront && facingRight) //if entering a climb from the right
        {
            canFlip = false; //prevent 180 degree flips
            facingFront = !facingFront; //prevent looping rotation
            transform.Rotate(0, -90, 0); //rotate left to face ladder
        }
        else if (isClimbing && facingFront && !facingRight) //if entering a climb from the left
        {
            canFlip = false;
            facingFront = !facingFront;
            transform.Rotate(0, 90, 0); //rotate right to face ladder
        }

        if (!facingFront && movementInput.x < 0 && !isLadder) //if off ladder but still facing it, and moving left
        {
            if (!facingRight) //if facing left
            {
                canFlip = true; //allow for 180 rotations
                facingFront = !facingFront; 
                transform.Rotate(0, -90, 0); //rotate left
            }
            else if (facingRight) //if facing right
            {
                canFlip = true;
                facingFront = !facingFront;
                transform.Rotate(0, 90, 0); //rotate right
            }
        }
        else if (!facingFront && movementInput.x > 0 && !isLadder) //if off ladder but still facing it, and moving right
        {
            if (facingRight) //if facing right
            {
                canFlip = true;
                facingFront = !facingFront;
                transform.Rotate(0, 90, 0); //rotate right
            }
            else if (!facingRight) //if facing left
            {
                canFlip = true;
                facingFront = !facingFront;
                transform.Rotate(0, -90, 0); //rotate left
            }
        }

        if (!facingFront && isLadder)
        {
            canFlip = false;
        }
        if (!facingFront && !isLadder)
        {
            canFlip = true;
        }

        if (!facingFront && !isJumping && !isMoving && !facingRight)
        {
            canFlip = true;
            facingFront = !facingFront;
            transform.Rotate(0, -90, 0);

        }
        else if (!facingFront && !isJumping && !isMoving && facingRight)
        {
            canFlip = true;
            facingFront = !facingFront;
            transform.Rotate(0, 90, 0);

        }




        if (movementInput.x < 0 && facingRight && canFlip) //if moving left and facing right and able to flip
        {
            flip(); //defined in void flip
        }
        if (movementInput.x > 0 && !facingRight && canFlip) //if moving right and facing left and able to flip
        {
            flip();
        }

    }

    void OnTriggerStay2D(Collider2D collision) //every frame where Collider2D is activating a trigger collision
    {

        if (collision.gameObject.tag == "Surface") //every frame upon the surface:
        {
            isJumping = false; //allow for jumping
            isDiving = false; //allow for normal movement
            speed = 12.5f; //default ground movement speed
            notGrounded = false;
            animator.SetBool("isInAir", false);
            animator.SetBool("isDiving", false);
            if (!isClimbing)
            {
             canFlip = true; 
            }
        }

        else if (collision.gameObject.tag == "Wall") //every frame upon the wall:
        {

            if (collision.gameObject.tag == "Surface") //every frame upon both the wall and surface
            {
                isJumping = false;
                speed = 12.5f;

            }
            else //every frame upon just the wall:
            {
                speed = 0f; //prevent momentum pinning to wall

            }
        }

        if(collision.gameObject.tag == "Player")
        {
            isJumping = false; //allow for jumping
            isDiving = false; //allow for normal movement
            speed = 12.5f; //default ground movement speed
            animator.SetBool("isInAir", false);
            animator.SetBool("isDiving", false);
            if (!isClimbing)
            {
                canFlip = true;
            }
        }

        if (collision.gameObject.tag != "Surface")
        {
            notGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) //upon Collider2D no longer activating a trigger collision
    {

        if (collision.gameObject.tag == "Surface") //if exiting with the surface
        {
            
            if (collision.gameObject.tag == "Wall") //if exiting both surface and wall
            {
                isJumping = true;
                animator.SetBool("isInAir", true);
                speed = 10f;
            }
            else //for just exiting a collision with the surface, not the wall
            {
                isJumping = true;
                animator.SetBool("isInAir", true);
                speed = 10f; 
            }

        }

        else if (collision.gameObject.tag == "Wall") // exiting a collision with the wall, not the surface - regardless of whether on surface or not
        {
            speed = 12.5f;
        }

        if (collision.gameObject.tag == "Player" && collision.gameObject.tag != "Surface" && notGrounded)
        {
            isJumping = true;
            animator.SetBool("isInAir", true);
            speed = 10f;

        }
        else if (collision.gameObject.tag  == "Player" && !notGrounded)
        {
            isJumping = false;
            animator.SetBool("isInAir", false);
            speed = 12.5f;


        }



    }

    void flip() //flips character direction 180
    {
        facingRight = !facingRight; //prevents flipping loop
        transform.Rotate(0, 180, 0);
        dampener = .1f;

    }

}
