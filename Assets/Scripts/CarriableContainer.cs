using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableContainer : MonoBehaviour
{


    public PlayerMovement playermovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playermovement.facingFront)
        {
            transform.localPosition = new Vector3(-.8f, -0.2f, 0);
        }
       
        else if (playermovement.facingFront)
        {
            transform.localPosition = new Vector3(1.15f, -.2f, 0);
        }
    }
}
