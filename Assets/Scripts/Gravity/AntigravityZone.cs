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
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSpawnManager.instance.playerList.Count == 1)
        {
            playermovement1 = GameObject.Find ("Player Prefab(Clone)").GetComponent<PlayerMovement>();

        }
        if (PlayerSpawnManager.instance.playerList.Count == 2)
        {
            playermovement2 = GameObject.Find ("Player Prefab(Clone) (1)").GetComponent<PlayerMovement>();

        }
        if (PlayerSpawnManager.instance.playerList.Count == 3)
        {
            playermovement3 = GameObject.Find ("Player Prefab(Clone) (2)").GetComponent<PlayerMovement>();

        }
        if (PlayerSpawnManager.instance.playerList.Count == 4)
        {
            playermovement4 = GameObject.Find ("Player Prefab(Clone) (3)").GetComponent<PlayerMovement>();

        }

        if ((playermovement1.interacting || playermovement2.interacting) && zone.enabled == false && canSwitch)
        {
            canSwitch = false;
            zone.enabled = true;
            Debug.Log("gravity off!");

        }
        if ((playermovement1.interacting || playermovement2.interacting) && zone.enabled == true && canSwitch)
        {
            canSwitch = false;
            zone.enabled = false;
            Debug.Log("gravity on!");
        }
        if (playermovement1.canInteract == true)
        {
            canSwitch = true;
        }
    }
   
}
