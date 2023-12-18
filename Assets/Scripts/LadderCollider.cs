using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderCollider : MonoBehaviour
{
  
    public PlayerMovement playermovement;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Ladder") //if touching ladder
        {
            playermovement.isLadder = true;

        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Ladder") // if exiting collision with ladder
        {
            playermovement.isLadder = false;
            playermovement.isClimbing = false;
        }
    }
}
