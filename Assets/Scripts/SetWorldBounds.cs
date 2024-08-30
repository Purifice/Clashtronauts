using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWorldBounds : MonoBehaviour
{
    private void Awake()
    {
       var bounds = GetComponent<Collider>().bounds; //defines bounds as the Collider on the object this script is attached to.
       Globals.WorldBounds = bounds; //sets the Global WorldBounds to the variable bounds as defined above
    }
}
