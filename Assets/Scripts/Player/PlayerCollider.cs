using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{

    public PlayerMovement playermovement;
    private bool isDiving; //used to prevent infinite position changes
    //private bool isTurned = false; //same as above
    private IEnumerator coroutineOne;
    private IEnumerator coroutineTwo;
    private IEnumerator coroutineBase;


    // Start is called before the first frame update
    void Start()
    {
        coroutineOne = RotateTimeDown();
        coroutineTwo = RotateTimeUp();
        coroutineBase = DiveRotation();

        StartCoroutine(coroutineBase);
    }

    IEnumerator DiveRotation()
    {
        while (true)
        {
            if (playermovement.isDiving && !isDiving && !playermovement.isClimbing) //position when diving
            {
                StopCoroutine(coroutineTwo);
                StartCoroutine(coroutineOne);
                transform.localPosition = new Vector3(0.5f, -0.5f, 0);
                isDiving = !isDiving;
                yield return new WaitForSeconds(.5f);
                StopCoroutine(coroutineOne);

            }
            else if (!playermovement.isDiving && isDiving && !playermovement.isClimbing) //position when not diving
            {
                StopCoroutine(coroutineOne);
                StartCoroutine(coroutineTwo);
                // transform.Rotate(0, 0, 90);
                transform.localPosition = new Vector3(0, 0, 0);
                isDiving = !isDiving;
                yield return new WaitForSeconds(.5f);
                StopCoroutine(coroutineTwo);
            }

            yield return new WaitForSeconds(0f);
        }
        
    }


    // Update is called once per frame
    void Update()
    {


        /*if (!playermovement.facingFront && !isTurned && !playermovement.isDiving) //keeping collider in 2D bounds while facing away (will always face right)
        {
            transform.Rotate(0, 90, 0);
            isTurned = !isTurned;


        }
        else if (playermovement.facingFront && isTurned && !playermovement.isDiving) //resetting collider to normal while facing front
        {
            transform.Rotate(0, -90, 0);
            isTurned = !isTurned;
        }*/

    }

    IEnumerator RotateTimeDown()
    {
       while (true)
        {
            yield return new WaitForSeconds(.1f);
            transform.Rotate(0, 0, -30);
            yield return new WaitForSeconds(.25f);
            transform.Rotate(0, 0, -60);
            yield return new WaitForSeconds(1f);
        }

    }
    IEnumerator RotateTimeUp()
    {
        while (true)
        {
            transform.Rotate(0, 0, 30);
            yield return new WaitForSeconds(.1f);
            transform.Rotate(0, 0, 60);
            yield return new WaitForSeconds(1f);
        }

    }
}
