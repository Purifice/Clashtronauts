using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    public PlayerMovement playermovement;
    private bool isDiving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playermovement.isDiving && !isDiving && !playermovement.isClimbing)
        {
            transform.Rotate(0, 0, -90);
            transform.localPosition = new Vector3(0.5f,-0.5f,0);
            isDiving = !isDiving;
        }
        else if (!playermovement.isDiving && isDiving && !playermovement.isClimbing)
        {
            transform.Rotate(0, 0, 90);
            transform.localPosition = new Vector3(0, 0, 0);
            isDiving = !isDiving;
        }
    }
}