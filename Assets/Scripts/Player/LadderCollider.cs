using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderCollider : MonoBehaviour
{
  
    public PlayerMovement playermovement;

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
