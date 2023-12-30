using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{

    public PlayerMovement playermovement;

    public Rigidbody2D rb2D;
    public CircleCollider2D coll;
    public Transform player;
    public Transform Container;
    public Transform playermomentum;

    public float PickUpRange;
    public float dropForwardForce;
    public float dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private Animator animator;



    // Start is called before the first frame update
    void Start()
    {

        animator = playermovement.GetComponent<Animator>();

        if (!equipped)
        {
            rb2D.isKinematic = false;
            //coll.isTrigger = false;
            Physics2D.IgnoreLayerCollision(7, 8, false);


        }

        if (equipped)
        {
            rb2D.isKinematic = true;
            //coll.isTrigger = true;
            Physics2D.IgnoreLayerCollision(7, 8, true);

            slotFull = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= PickUpRange && Input.GetKeyDown(KeyCode.Slash) && !slotFull)
        {
            Pickup();

        }

        if (playermovement.facingFront && equipped && (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Period)))
        {
            Throw();
        }

       /* if (!playermovement.facingFront)
        {
            Drop();
        }*/
    }

    private void Pickup()
    {
        equipped = true;
        slotFull = true;
        rb2D.isKinematic = true;
        //coll.isTrigger = true;
        Physics2D.IgnoreLayerCollision(7, 8, true);

        animator.SetBool("isCarrying", true);


        transform.gameObject.tag = "Untagged";

        rb2D.velocity = new Vector2(0, 0);

        transform.SetParent(Container);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }

    private void Throw()
    {
        equipped = false;
        slotFull = false;
        transform.gameObject.tag = "Surface";

        animator.SetBool("isCarrying", false);

        transform.SetParent(null);

        rb2D.isKinematic = false;
        //coll.isTrigger = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);



        rb2D.velocity = player.GetComponent<Rigidbody2D>().velocity;

        rb2D.AddForce(playermomentum.forward * dropForwardForce * -10f, ForceMode2D.Impulse);
        rb2D.AddForce(playermomentum.up * dropUpwardForce, ForceMode2D.Impulse);

    }

   /* private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null) ;

        rb2D.isKinematic = false;
        coll.isTrigger = false;
    }*/
}
