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

    public GameObject grabbedObject;

    private int layerIndex;

    public bool equipped = false;

    public PlayerMovement playermovement;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Pickables");

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
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);
        
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
        {
            if (playermovement.carryButton && grabbedObject == null) //if button to carry is pressed and nothing is grabbed
            {
                grabbedObject = hitInfo.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                grabbedObject.transform.position = grabPoint.position;
                grabbedObject.transform.SetParent(transform);
                equipped = true;

                Physics2D.IgnoreLayerCollision(6, 8, true);
                animator.SetBool("isCarrying", true);

            }
            else if (playermovement.carryButton) //if button to carry is pressed and something is grabbed
            {
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedObject.transform.SetParent(null);
                grabbedObject = null;
                equipped = false;

                Physics2D.IgnoreLayerCollision(6, 8, false);
                animator.SetBool("isCarrying", false);
            }
        }

        if (equipped && playermovement.dove)
        {
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
            equipped = false;

            Physics2D.IgnoreLayerCollision(7, 8, false);
            animator.SetBool("isCarrying", false);

        }
        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }
}
