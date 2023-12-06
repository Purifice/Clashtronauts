using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb2D;

    public float speed;
    public float jump;

    private float moveHorizontal;
    private float moveVertical;

    public bool isJumping;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        speed = 12.5f;
        jump = 20f;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        
    }

    void FixedUpdate() //FOR PHYSICS
    {
        if (moveHorizontal > 0f || moveHorizontal < -0f)
        {
            //rb2D.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
            //The above movement method applies a force constantly as the key is pressed.
            //Not ideal for trying to have a constant max speed until I learn how to set the max velocity effectively
            rb2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb2D.velocity.y);
            //This above movement simply multiples the -1 / 1 response by the speed, so it is more linear
        }

        if (!isJumping && moveVertical > 0.1f)
        {
            // rb2D.AddForce(new Vector2(0f, moveVertical * jump), ForceMode2D.Impulse);
            //The above method uses the applying force method
            rb2D.velocity = new Vector2(rb2D.velocity.x, jump);
            //This method simply jumps by the jump amount linearly
        }

        if (moveHorizontal < 0 && facingRight)
        {
            flip();
        }
        if (moveHorizontal > 0 && !facingRight)
        {
            flip();
        }
        //flips the character depending on the direction input
    }

    void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Surface") //every frame upon the surface:
        {
            isJumping = false;
            speed = 12.5f;

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
                speed = 10f;
            }
            else //for just exiting a collision with the surface, not the wall THIS CAUSES THE PERSISTENT STICKY WALL
            //how to know if exiting a collision with surface but still colliding with wall? new void needed?
            {
                isJumping = true;
                speed = 10f; 
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {

            if (collision.gameObject.tag == "Surface") //if exiting both wall and surface - redundant? neccessary? 
            {
                isJumping = true;
                speed = 10f;
            }
            else //just exiting a collision with the wall, not the surface - regardless of whether on ground or in air
             //how to know if exiting a collision with wall but still colliding with surface? new void needed?
            {
                //isJumping = false; //causes the persistent sticky floor or infinent jump depending on false/true
              speed = 12.5f;
            }
        }

    }

    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}
