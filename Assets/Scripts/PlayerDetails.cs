using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{

    public int playerID;
    public Vector3 startPos;
    public Quaternion startRot;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
