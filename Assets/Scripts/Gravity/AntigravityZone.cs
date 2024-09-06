using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AntigravityZone : MonoBehaviour
{

    private BoxCollider2D zone;
    public PlayerMovement playermovement;

    public bool canSwitch;

    void Start()
    {
        zone = GetComponent<BoxCollider2D>();
        zone.enabled = false;
        canSwitch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playermovement.interacting && zone.enabled == false && canSwitch)
        {
            canSwitch = false;
            zone.enabled = true;
            //Debug.Log("gravity off!");

        }
        if (playermovement.interacting && zone.enabled == true && canSwitch)
        {
            canSwitch = false;
            zone.enabled = false;
            //Debug.Log("gravity on!");
        }
        if (playermovement.canInteract == true)
        {
            canSwitch = true;
        }
    }
   
}
