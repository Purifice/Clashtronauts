using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    ItemHurtbox itemHurtbox;

    public float maxHealth = 100f;
    public float currentHealth;
    public float damageTaken;
    public float diffVelX;
    public float diffVelY;
    public float roughDamage;

    public bool isOut;

    private float playerVelX;
    private float playerVelY;
    


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        isOut = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") //if colliding with a "Player" tagged object
        {
            itemHurtbox = collision.gameObject.GetComponent<ItemHurtbox>(); //assign the reference of itemHurtbox

            if(itemHurtbox != null) //if it exists on the collided object
            {

                if (playerVelX >= itemHurtbox.itemVelX) //this and the following lines finds the difference in velocity between the player and the colliding object
                {
                    diffVelX = playerVelX - itemHurtbox.itemVelX;
                }
                else
                {
                    diffVelX = itemHurtbox.itemVelX - playerVelX;
                }

                if (playerVelY >= itemHurtbox.itemVelY)
                {
                    diffVelY = playerVelY - itemHurtbox.itemVelY;
                }
                else
                {
                    diffVelY = itemHurtbox.itemVelY - playerVelY;
                }

                roughDamage = itemHurtbox.baseDamage + (((Mathf.Abs(diffVelX) + Mathf.Abs(diffVelY))) * itemHurtbox.damageMass); //sets a rough damage based off the speed and damage mass of the object
                damageTaken = Mathf.Round(roughDamage / 5f); //sets the final damage to the rounded result of the rough damage divided by 5
                Debug.Log(damageTaken);
            }

            else
            {
                damageTaken = 0f; //otherwise don't take any damage
            }

            TakeDamage();
        }
        else
        {
            damageTaken = 0f; //if not colliding with a "Player" tagged object don't take any damage
        }
    }


    void TakeDamage()
    {
        currentHealth -= damageTaken; //reduce current health by the amount of damage taken

        if(currentHealth <= 0f) //once health reaches 0
        {
            isOut = true;
        }
    }
}
