using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carriables : MonoBehaviour
{
    public Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            rb2D.velocity = new Vector2(0, 0);
            transform.gameObject.tag = "Untagged";
            transform.gameObject.layer = 6;

        }
        else if (transform.parent == null)
        {
            transform.gameObject.tag = "Player";
            transform.gameObject.layer = 7;

        }
    }
}
