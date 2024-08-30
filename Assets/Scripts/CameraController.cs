using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform playerOne;
    public Transform playerTwo;
    public Transform playerThree;
    public Transform playerFour;

    private bool onPlayer = false;

    public Vector3 offset;

    Vector3 gizmoPos;
    private Vector3 targetPosition;

    public float smoothSpeed = 3f;

    private Bounds cameraBounds;

    private Camera mainCamera;

    public static CameraController instance = null;

    
    // Start is called before the first frame update
    void Start()
    {
        smoothSpeed = 3f;
        StartCoroutine(CameraStartDelay());
        var height = mainCamera.orthographicSize;
        var width = height * mainCamera.aspect;

        var minX = Globals.WorldBounds.min.x + width;
        var maxX = Globals.WorldBounds.extents.x - width;

        var minY = Globals.WorldBounds.min.y + height;
        var maxY = Globals.WorldBounds.extents.y - height;

        var minZ = Globals.WorldBounds.min.z;
        var maxZ = Globals.WorldBounds.max.z;

        cameraBounds = new Bounds();
        cameraBounds.SetMinMax(
         new Vector3(minX, minY, minZ),
         new Vector3(maxX, maxY, maxZ)
         );
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        mainCamera = Camera.main;
    }
   

    // Update is called once per frame
    void Update()
    {

        if (PlayerSpawnManager.instance.playerList.Count == 2)
        {
        CameraController.instance.playerTwo = PlayerSpawnManager.instance.playerList[1].gameObject.transform;
        }
        if (PlayerSpawnManager.instance.playerList.Count == 3)
        {
        CameraController.instance.playerThree = PlayerSpawnManager.instance.playerList[2].gameObject.transform;
        }
        if (PlayerSpawnManager.instance.playerList.Count == 4)
        {
        CameraController.instance.playerFour = PlayerSpawnManager.instance.playerList[3].gameObject.transform;
        }
        // Assigns the transform value of "playerTwo" "playerThree" and "playerFour" to the second, third, and fourth players upon joining
        

        if(playerOne != null && PlayerSpawnManager.instance.playerList.Count < 2) //if only one player exists
        {
            Vector3 desiredPosition = playerOne.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); 
            transform.position = smoothedPosition;

            targetPosition = smoothedPosition;
            targetPosition = GetCameraBounds();

            transform.position = targetPosition;

            onPlayer = true;
        }
        else if (PlayerSpawnManager.instance.playerList.Count >= 2) //if more than one player exists
        {
            Vector3 desiredPosition = FindCentroid() + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
            gizmoPos = FindCentroid();

            targetPosition = smoothedPosition;
            targetPosition = GetCameraBounds();

            transform.position = targetPosition;

            onPlayer = false;
        }

        

    }

    IEnumerator CameraStartDelay()
    {
        yield return new WaitForSeconds(0.2f);
        if (PlayerSpawnManager.instance.playerList.Count < 0) //if no players exist
        {
            SetCamera();
        }
    }

    public void SetCamera()
    {
        CameraController.instance.playerOne = PlayerSpawnManager.instance.playerList[0].gameObject.transform;
        //sets "player one" transform value to the first item in the player list instance

        if (!onPlayer && PlayerSpawnManager.instance.playerList.Count < 2)
        {
            transform.position = CameraController.instance.playerOne.transform.position + CameraController.instance.offset;
            //moves camera to player 1's position
        }
    }

    Vector3 FindCentroid()
    {
        var totalX = 0f; //set the default x value to 0 at start
        var totalY = 0f; //set the default y value to 0 at start
        var totalZ = 0f; //set the default z value to 0 at start

        foreach (var player in PlayerSpawnManager.instance.playerList)
        {
            totalX += player.transform.position.x; //add each players x position for a totalX
            totalY += player.transform.position.y; //add each players y position for a totalY
            //totalZ += player.transform.position.z;
        }
        
        if (PlayerSpawnManager.instance.playerList.Count == 2)
        {
            totalZ -= Mathf.Abs(playerOne.transform.position.x - playerTwo.transform.position.x) 
            + Mathf.Abs(playerOne.transform.position.y - playerTwo.transform.position.y); 
        }

        if (PlayerSpawnManager.instance.playerList.Count == 3)
        {
            totalZ -= Mathf.Abs(playerOne.transform.position.x - playerTwo.transform.position.x - playerThree.transform.position.x) 
            + Mathf.Abs(playerOne.transform.position.y - playerTwo.transform.position.y - playerThree.transform.position.y); 
        }

        if (PlayerSpawnManager.instance.playerList.Count == 4)
        {
            totalZ -= Mathf.Abs(playerOne.transform.position.x - playerTwo.transform.position.x - playerThree.transform.position.x - playerFour.transform.position.x) 
            + Mathf.Abs(playerOne.transform.position.y - playerTwo.transform.position.y - playerThree.transform.position.y - playerFour.transform.position.y); 
        } 

        var centerX = totalX / PlayerSpawnManager.instance.playerList.Count; // divide total x value by number of players
        var centerY = totalY / PlayerSpawnManager.instance.playerList.Count; // divide total y value by number of players
        var centerZ = totalZ / PlayerSpawnManager.instance.playerList.Count; // divide total z value by number of players
        //Debug.Log(totalZ);
       // Debug.Log (centerZ);

        return new Vector3 (centerX, centerY, centerZ); //create a vector using the divided total of all 3 axis
    }

    private Vector3 GetCameraBounds()
    {
        return new Vector3(
            Mathf.Clamp(targetPosition.x, cameraBounds.min.x, cameraBounds.max.x),
            Mathf.Clamp(targetPosition.y, cameraBounds.min.y, cameraBounds.max.y),
            Mathf.Clamp(targetPosition.z, cameraBounds.min.z, cameraBounds.max.z)
          //  transform.position.z
            );

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(gizmoPos, new Vector3(1, 1, 0));
    }
}
