using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{

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

 
    
    
    // Start is called before the first frame update
    void Start()
    {
        if(!equipped)
        {
            rb2D.isKinematic = false;
            coll.isTrigger = false;
        }

        if (equipped)
        {
            rb2D.isKinematic = true;
            coll.isTrigger = true;
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

        if (equipped && Input.GetKeyDown(KeyCode.Comma))
        {
            Drop();
        }
    }

    private void Pickup()
    {
        equipped = true;
        slotFull = true;
        rb2D.isKinematic = true;
        coll.isTrigger = true;

        transform.SetParent(Container);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb2D.isKinematic = false;
        coll.isTrigger = false;

        rb2D.velocity = player.GetComponent<Rigidbody2D>().velocity;

        rb2D.AddForce(playermomentum.forward * dropForwardForce * -10f, ForceMode2D.Impulse);
        rb2D.AddForce(playermomentum.up * dropUpwardForce, ForceMode2D.Impulse);

    }
}
