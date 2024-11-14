using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHurtbox : MonoBehaviour
{

    public float damageMass;
    public float baseDamage;
    public float itemVelX;
    public float itemVelY;

    public Rigidbody2D rb2d;

    void Start()
    {
        baseDamage = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
        itemVelX = rb2d.velocity.x;
        itemVelY = rb2d.velocity.y;
    }
}
