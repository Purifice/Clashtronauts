using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysics : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public bool isGravity;

    public float currentGravity;
    private float defaultGravity;
    
    // Start is called before the first frame update
    void Start()
    {
     defaultGravity = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        currentGravity = rb2d.gravityScale;

        if (isGravity)
        {
            rb2d.gravityScale = defaultGravity;
        }
        else if (!isGravity)
        {
            rb2d.gravityScale = 0f;
        }
    }
   
   void OnTriggerStay2D(Collider2D collision) //every frame where Collider2D is activating a trigger collision
    {
        if (collision.gameObject.tag == "Antigravity")
        {
            isGravity = false;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision) //upon Collider2D no longer activating a trigger collision
    {
        if (collision.gameObject.tag == "Antigravity")
        {
            isGravity = true;
        }
    }
}
