using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb2D;
    private Animator animator;


    public float speed;
    public float jump;

    private float moveHorizontal;
    private float moveVertical;
    private float vertical; //for ladder movement

    public bool isJumping;
    public bool isLadder; //touching ladder, referenced by external script
    public bool isClimbing;

    private bool facingRight = true;
    private bool facingFront = true; //for ladder directional
    private bool canFlip = true;


    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        speed = 12.5f;
        jump = 20f;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        vertical = Input.GetAxis("Vertical");       
    }

    // Fixed Update is called every fixed-rate frame, and is best for physics
    void FixedUpdate() 
    {

        if (moveHorizontal > 0f || moveHorizontal < -0f)
        {
            //rb2D.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
            //The above movement method applies a force constantly as the key is pressed, good for 0g?
            rb2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb2D.velocity.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (!isJumping && moveVertical > 0.1f)
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

        if (isLadder && Mathf.Abs(vertical) > 0f) //if touching ladder and moving up
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing)
        {
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

        if (!facingFront && moveHorizontal < 0 && !isLadder) //if off ladder but still facing it, and moving left
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
        else if (!facingFront && moveHorizontal > 0 && !isLadder) //if off ladder but still facing it, and moving right
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

        if (moveHorizontal < 0 && facingRight && canFlip) //if moving left and facing right and able to flip
        {
            flip(); //defined in void flip
        }
        if (moveHorizontal > 0 && !facingRight && canFlip) //if moving right and facing left and able to flip
        {
            flip();
        }

    }

    void OnTriggerStay2D(Collider2D collision) //every frame where Collider2D is activating a trigger collision
    {

        if (collision.gameObject.tag == "Surface") //every frame upon the surface:
        {
            isJumping = false; //allow for jumping
            speed = 12.5f; //default ground movement speed
            animator.SetBool("isInAir", false);

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
    }

    void OnTriggerExit2D(Collider2D collision) //upon Collider2D no longer activating a trigger collision
    {

        if (collision.gameObject.tag == "Surface") //if colliding with the surface
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

    }

    void flip() //flips character direction 180
    {
        facingRight = !facingRight; //prevents flipping loop
        transform.Rotate(0, 180, 0);
    }
  
}
