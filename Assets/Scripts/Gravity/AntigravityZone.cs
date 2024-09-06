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
    public PlayerMovement playermovement1;
    public PlayerMovement playermovement2;
    public PlayerMovement playermovement3;
    public PlayerMovement playermovement4;
    //public static AntigravityZone instance = null;



    public bool canSwitch;

    void Start()
    {
        zone = GetComponent<BoxCollider2D>();
        zone.enabled = false;
        canSwitch = true;
       //AntigravityZone.instance.playermovement1 = ;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSpawnManager.instance.playerList.Count == 2)
        {

        }
        if (PlayerSpawnManager.instance.playerList.Count == 3)
        {

        }
        if (PlayerSpawnManager.instance.playerList.Count == 4)
        {

        }

        if (playermovement1.interacting || playermovement2.interacting || playermovement3.interacting || playermovement4.interacting && zone.enabled == false && canSwitch)
        {
            canSwitch = false;
            zone.enabled = true;
            //Debug.Log("gravity off!");

        }
        if (playermovement1.interacting || playermovement2.interacting || playermovement3.interacting || playermovement4.interacting && zone.enabled == true && canSwitch)
        {
            canSwitch = false;
            zone.enabled = false;
            //Debug.Log("gravity on!");
        }
        if (playermovement1.canInteract == true)
        {
            canSwitch = true;
        }
    }
   
}
