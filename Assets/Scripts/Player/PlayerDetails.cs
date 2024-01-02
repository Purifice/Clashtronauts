using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{

    public int playerID; //sets a playerID referenced in SpawnManager
    public Vector3 startPos;
    public Quaternion startRot;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos; //transforms the spawn position to a predefined start Position (referenced in spawn manager)
        transform.rotation = startRot; // same as transform but with a rotation
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
