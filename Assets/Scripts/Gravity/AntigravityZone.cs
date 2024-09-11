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
   
    //public static AntigravityZone instance = null;



    public bool canSwitch;
    public bool canInteract;
    public bool interacting;

    void Start()
    {
        zone = GetComponent<BoxCollider2D>();
        zone.enabled = false;
        canSwitch = true;
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
   
        if (interacting && zone.enabled == false && canSwitch)
        {
            canSwitch = false;
            zone.enabled = true;
            Debug.Log("gravity off!");

        }
        if (interacting && zone.enabled == true && canSwitch)
        {
            canSwitch = false;
            zone.enabled = false;
            Debug.Log("gravity on!");

        }
        if (canInteract == true)
        {
            canSwitch = true;
        }
    }
   
}
