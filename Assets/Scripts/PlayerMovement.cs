using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb2D;

    public float speed;
    public float jump;

    private float moveHorizontal;
    private float moveVertical;
    private float vertical;

    public bool isJumping;
    private bool facingRight = true;
    private bool facingFront = true;
    public bool isLadder;
    public bool isClimbing;
    private bool canFlip = true;

    private Animator animator;
    private CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        speed = 12.5f;
        jump = 20f;
        isJumping = false;

        animator = GetComponent<Animator>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f) //if touching ladder and moving up
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }
        //find a way to make this slightly more persisent to prevent ladder animation glitches
    }

    void FixedUpdate() //FOR PHYSICS
    {

        if (moveHorizontal > 0f || moveHorizontal < -0f)
        {
            //rb2D.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
            //The above movement method applies a force constantly as the key is pressed.
            //Not ideal for trying to have a constant max speed until I learn how to set the max velocity effectively
            rb2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb2D.velocity.y);
            animator.SetBool("isWalking", true);

            //This above movement simply multiples the -1 / 1 response by the speed, so it is more linear
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

            //This method simply jumps by the jump amount linearly
        }
        else
        {
            animator.SetBool("isJumping", false);

        }
       
        if (isClimbing && facingFront && facingRight) 
        {
            canFlip = false;
            facingFront = !facingFront;
            transform.Rotate(0, -90, 0); 
        }
        else if (isClimbing && facingFront && !facingRight) 
        {
            canFlip = false;
            facingFront = !facingFront;
            transform.Rotate(0, 90, 0);
        }

        if (!facingFront && moveHorizontal < 0) 
        {
            canFlip = true;
            facingFront = !facingFront;
            transform.Rotate(0, -90, 0);
        }
        else if (!facingFront && moveHorizontal > 0)
        {
            canFlip = true;
            facingFront = !facingFront;
            transform.Rotate(0, 90, 0);
        }

        if (moveHorizontal < 0 && facingRight && canFlip)
        {
            flip();
        }
        if (moveHorizontal > 0 && !facingRight && canFlip)
        {
            flip();
        }

        if (isClimbing)
        {
            rb2D.gravityScale = 0f; //sets gravity to 0
            rb2D.velocity = new Vector2(rb2D.velocity.x, vertical * speed); //moves up ladder
        }    
        else
        {
            rb2D.gravityScale = 5f;

        }
    }

    void OnTriggerStay2D(Collider2D collision) 
    {

        if (collision.gameObject.tag == "Surface") //every frame upon the surface:
        {
            isJumping = false;
            speed = 12.5f;
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
                speed = 0f;

            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Surface")
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

    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
  
}
