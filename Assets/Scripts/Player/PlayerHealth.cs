using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;
    public float damageTaken; //exists as placeholder for the damge done

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage()
    {
        currentHealth -= damageTaken;

        if(currentHealth <= 0f)
        {
            //death function
        }
    }
}
