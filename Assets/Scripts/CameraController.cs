using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform playerOne;

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

        cameraBounds = new Bounds();
        cameraBounds.SetMinMax(
         new Vector3(minX, minY, 0f),
         new Vector3(maxX, maxY, 0f)
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
        if(playerOne != null && PlayerSpawnManager.instance.playerList.Count < 2)
        {
            Vector3 desiredPosition = playerOne.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            targetPosition = smoothedPosition;
            targetPosition = GetCameraBounds();

            transform.position = targetPosition;

            onPlayer = true;
        }
        else if (PlayerSpawnManager.instance.playerList.Count >= 2)
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
        if (PlayerSpawnManager.instance.playerList.Count < 0)
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
        var totalX = 0f;
        var totalY = 0f;
        var totalZ = 0f;

        foreach (var player in PlayerSpawnManager.instance.playerList)
        {
            totalX += player.transform.position.x;
            totalY += player.transform.position.y;
            totalZ += player.transform.position.z;
        }

        var centerX = totalX / PlayerSpawnManager.instance.playerList.Count;
        var centerY = totalY / PlayerSpawnManager.instance.playerList.Count;
        var centerZ = totalZ / PlayerSpawnManager.instance.playerList.Count;

        return new Vector3 (centerX, centerY, centerZ);
    }

    private Vector3 GetCameraBounds()
    {
        return new Vector3(
            Mathf.Clamp(targetPosition.x, cameraBounds.min.x, cameraBounds.max.x),
            Mathf.Clamp(targetPosition.y, cameraBounds.min.y, cameraBounds.max.y),
            transform.position.z
            );

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(gizmoPos, new Vector3(1, 1, 0));
    }
}
