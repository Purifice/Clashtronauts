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
    public AntigravityZone antigravityzone;


    public float speed;
    public float maxVelocity;
    public float jump;
    public float dampener = .1f;
    

    //private float moveHorizontal;
    //private float moveVertical;
    private float vertical; //for ladder movement
    private float rotationSpeed;
    private float offset;
    private float angle;
    private float targetAngle;
    private float angleDifference;
    private float torque;



    public bool isJumping;
    public bool isLadder; //touching ladder, referenced by external script
    public bool isClimbing;
    public bool isDiving;
    public bool facingFront = true; //for ladder directional
    public bool attackButton = false;
    public bool interactButton = false;
    public bool dove = false;
    public bool canFlip = true;
    public bool climbfromRight = false;
    public bool climbfromLeft = false;
    public bool notGrounded;
    public bool isGravity;
    public bool mustRotate;
    public bool colliderRotate;
    public bool touchingTrigger;
     

    public bool facingRight = true; //always spawns assuming it's facing right
    private bool hasDove;
    private bool isMoving;
    private bool jumped = false;
    private bool climbed = false;

    public Transform modelChild;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 direction;
    private Vector2 faceDirection;





    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        modelChild = this.gameObject.transform.GetChild(0);
        speed = 12.5f;
        maxVelocity = 17.5f;
        jump = 20f;
        dampener = .1f;
        isJumping = false;
        isGravity = true;
        mustRotate = false;
        touchingTrigger = false;
        antigravityzone = GameObject.Find("AntiGravity Zone").GetComponent<AntigravityZone>();
    
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>(); //reads the input value of L/R/U/D movement
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
    public void OnAttack(InputAction.CallbackContext context)
    {
        attackButton = context.action.triggered; //reads whether or not the carry button is being triggered
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        interactButton = context.action.triggered; //reads whether or not the interact button is being triggered
    }

    // Update is called once per frame
    void Update()
    {

        rb2D.AddForce(Vector2.zero); //prevents onstay collisions from falling asleep when standing still

        if (dampener < 1) // gradually unlimits the movement speed for a smooth acceleration
        {
            dampener += 2f * Time.deltaTime;
        }
        else if (dampener > 1)
        {
            dampener = 1f;
        }

        vertical = Input.GetAxis("Vertical"); // for determining a smoother vertical movement occurence

        if (isGravity)
        {

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
                //empty
            }


        }

        else if (!isGravity)
        {
            //set diving conditions for 0g
            if(dove && !isDiving) //if pressing the dive key and not already diving
            {
                isDiving = true;
            }
            if (isDiving)
            {
                faceDirection = transform.right;
                animator.SetBool("isDiving", true); //start animation
                rb2D.AddForce(faceDirection * 30 * (Time.deltaTime * 100f), ForceMode2D.Force);
                
                //rb2D.AddForce(new Vector2(movementInput.x * ((2 * dampener)), movementInput.y * ((2 * dampener))), ForceMode2D.Force);
                maxVelocity = 32.5f;
                colliderRotate = true;

            }
            if (!isDiving)
            {
                animator.SetBool("isDiving", false);
                maxVelocity = 17.5f;
                colliderRotate = false;
            }
           
        }

    }

    // Fixed Update is called every fixed-rate frame, and is best for physics
    void FixedUpdate()
    {
       
        if (isGravity)
        {

            OrientUp();
            if (!isDiving && (movementInput.x > 0f || movementInput.x < -0f))
            {
                rb2D.velocity = new Vector2(movementInput.x * (dampener * speed), rb2D.velocity.y);
                animator.SetBool("isWalking", true);
                isMoving = true;
            }
            else if (!isDiving && (movementInput.x <= 0f || movementInput.x >= -0f))
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
                climbfromLeft = false;
                climbfromRight = true;
                canFlip = false; //prevent 180 degree flips
                facingFront = !facingFront; //prevent looping rotation
                modelChild.transform.Rotate(0, -90, 0); //rotate left to face ladder
            }
            else if (isClimbing && facingFront && !facingRight) //if entering a climb from the left
            {

                climbfromLeft = true;
                climbfromRight = false;
                canFlip = false;
                facingFront = !facingFront;
                modelChild.transform.Rotate(0, 90, 0); //rotate right to face ladder
            }

            if (!facingFront && movementInput.x < 0 && !isLadder) //if off ladder but still facing it, and moving left
            {
                if (!facingRight) //if facing left
                {
                    canFlip = true; //allow for 180 rotations
                    facingFront = !facingFront;
                    modelChild.transform.Rotate(0, -90, 0); //rotate left
                }
                else if (facingRight) //if facing right
                {
                    canFlip = true;
                    facingFront = !facingFront;
                    modelChild.transform.Rotate(0, 90, 0); //rotate right
                }
            }
            else if (!facingFront && movementInput.x > 0 && !isLadder) //if off ladder but still facing it, and moving right
            {
                if (facingRight) //if facing right
                {
                    canFlip = true;
                    facingFront = !facingFront;
                    modelChild.transform.Rotate(0, 90, 0); //rotate right
                }
                else if (!facingRight) //if facing left
                {
                    canFlip = true;
                    facingFront = !facingFront;
                    modelChild.transform.Rotate(0, -90, 0); //rotate left
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
                modelChild.transform.Rotate(0, -90, 0);

            }
            else if (!facingFront && !isJumping && !isMoving && facingRight)
            {
                canFlip = true;
                facingFront = !facingFront;
                modelChild.transform.Rotate(0, 90, 0);

            }


            if (movementInput.x < 0 && facingRight && canFlip) //if moving left and facing right and able to flip
            {
                flip(); //defined in void flip
            }
            if (movementInput.x > 0 && !facingRight && canFlip) //if moving right and facing left and able to flip
            {
                flip();
            }



            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        }

        else if (!isGravity)
        {

            rb2D.gravityScale = 0f;
            animator.SetBool("isInAir", true);
            LockXRotation();
            LimitVelocity();
            //Debug.Log(rb2D.velocity);

            if(isDiving)
            {
                RotateTowardsTargetLimited();
            }

            //applying movement force
            if (!isDiving && ((movementInput.x > 0f || movementInput.x < -0f) || (movementInput.y > 0f || movementInput.y < -0f)))
            {
                rb2D.AddForce(new Vector2(movementInput.x * ((1f + dampener) * speed), movementInput.y * ((1f + dampener) * speed)), ForceMode2D.Force);
                //applies a force as opposed to setting a velocity

                //animator.SetBool("animationstate", true);
                isMoving = true;

            }

            //applies logic for not moving
            else if (!isDiving && ((movementInput.x <= 0f || movementInput.x >= -0f) || (movementInput.y <= 0f || movementInput.y >= -0f)))
            {
                dampener = .1f;
                //animator.SetBool("animationstate", false);
                isMoving = false;
            }

            //ladder movement and direction
            if (climbfromLeft && !facingFront)
            {
                modelChild.transform.Rotate(0, 90, 0); //rotate right
                climbfromLeft = false;

            }
            if (climbfromRight && !facingFront)
            {
                modelChild.transform.Rotate(0, -90, 0); //rotate left
                climbfromRight = false;

            }

            if (isMoving)
            {
                //Debug.Log ("moving"); 
                RotateTowardsTarget();
            }
            else
            {
                rb2D.angularDrag = .5f;
            }

            rb2D.constraints = RigidbodyConstraints2D.None;
        }

        /*if (antigravityzone.interacting && !antigravityzone.zone.enabled) //if a player is pressing the button and the zone is off
        {
            //Debug.Log("test");
            mustRotate = true;
        }
        if (mustRotate && !notGrounded)
        {
            //Debug.Log ("time to rotate to normal!");
            OrientUp();
            mustRotate = false;
        }*/
    }

    void OnTriggerStay2D(Collider2D collision) //every frame where Collider2D is activating a trigger collision
    {

        if (collision.gameObject.tag == "Trigger" || !antigravityzone.canInteract)
        {
            touchingTrigger = true;
        }
        else
        {
            touchingTrigger = false;
        }
        if (collision.gameObject.tag == "Trigger" && interactButton && antigravityzone.canInteract)
        {
            antigravityzone.canInteract = false;
            antigravityzone.interacting = true;
            //Debug.Log ("pressing!");
        }
        if (!antigravityzone.canInteract && !interactButton)
        {
            antigravityzone.interacting = false;
            antigravityzone.canInteract = true;
        }

        if (collision.gameObject.tag == "Antigravity")
        {
            isGravity = false;
            //Debug.Log(collision.tag);
        }

        if (isGravity)
        {
            if (collision.gameObject.tag == "Surface") //every frame upon the surface:
            {
                notGrounded = false;
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

            if (collision.gameObject.tag == "Player")
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

        else if (!isGravity)
        {
            speed = 12.5f;

            if (collision.gameObject.tag != "Antigravity" && collision.gameObject.tag != "Trigger" )
            {
                isDiving = false;
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision) //upon Collider2D no longer activating a trigger collision
    {
        if (collision.gameObject.tag == "Antigravity")
        {
            isGravity = true;
        }
        if (isGravity)
        {
            if (collision.gameObject.tag == "Surface") //if exiting with the surface
            {

                if (collision.gameObject.tag == "Wall") //if exiting both surface and wall
                {
                    isJumping = true;
                    notGrounded = true;
                    animator.SetBool("isInAir", true);
                    speed = 12.5f;
                }
                else //for just exiting a collision with the surface, not the wall
                {
                    isJumping = true;
                    notGrounded = true;
                    animator.SetBool("isInAir", true);
                    speed = 12.5f;
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
                speed = 12.5f;

            }
            else if (collision.gameObject.tag == "Player" && !notGrounded)
            {
                isJumping = false;
                animator.SetBool("isInAir", false);
                speed = 12.5f;
            }
        }

        else if (!isGravity)
        {
            speed = 10f;
        }
    }

    void flip() //flips character direction 180
    {
        facingRight = !facingRight; //prevents flipping loop
        transform.Rotate(0, 180, 0);
        dampener = .1f;

    }

    private void RotateTowardsTarget()
    {
        rb2D.angularDrag = 3.5f;
        rotationSpeed = 1.5f;
        offset = 0f;
        direction = movementInput;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }
    private void RotateTowardsTargetLimited()
    {
        rb2D.angularDrag = 15f;
        rotationSpeed = .75f;
        offset = 0f;
        direction = movementInput;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction.Normalize();
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }

    void LimitVelocity()
    {
        // Get the current velocity of the Rigidbody2D
        Vector2 velocity = rb2D.velocity;

        // If the magnitude of the velocity exceeds the maxSpeed, clamp it
        if (velocity.magnitude > maxVelocity)
        {
            rb2D.velocity = velocity.normalized * maxVelocity;
        }
    }
    void OrientUp()
    {
        /*// Calculate the "up" direction (positive y-axis) in 2D space
        float angle = Mathf.Atan2(Vector2.up.y, Vector2.up.x) * Mathf.Rad2Deg;

        // Rotate the Rigidbody2D to face "up"
        rb2D.rotation = angle + -90f;*/

        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Set the Z + X value of rotation to 0, and the y to either 0 or 180
        currentRotation.z = 0;
        currentRotation.x = 0;
        if(facingRight)
        {
            currentRotation.y = 0;
        }
        else
        {
            currentRotation.y = 180;
        }

        // Apply the new rotation back to the transform
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    void LockXRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Set the xy value of rotation to 0
       // currentRotation.x = 0;
       // currentRotation.y = 0;
        

        // Apply the new rotation back to the transform
        transform.rotation = Quaternion.Euler(currentRotation);
    }
    

}