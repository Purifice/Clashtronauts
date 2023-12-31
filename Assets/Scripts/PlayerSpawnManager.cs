using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform[] spawnLocations;
    void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("PlayerInput ID:" + playerInput.playerIndex);

        playerInput.gameObject.GetComponent<PlayerDetails>().playerID = playerInput.playerIndex + 1;

        playerInput.gameObject.GetComponent<PlayerDetails>().startPos = spawnLocations[playerInput.playerIndex].position;

        playerInput.gameObject.GetComponent<PlayerDetails>().startRot = spawnLocations[playerInput.playerIndex].rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
