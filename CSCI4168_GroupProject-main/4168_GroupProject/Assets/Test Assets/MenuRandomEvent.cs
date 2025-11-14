using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MenuRandomEvent : MonoBehaviour
{
    int randomNumber;

    public float waitTime;
    private float waitCounter = 0;

    //variables set in editor for handling player information
    public Transform pointA; // end point
    public Transform pointB; // start point

    public float speed;

    private bool atDestination = false;
    private bool waiting = false;
    private bool moveBack = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomNumber = Random.Range(0, 5); // 20% chance of event occuring
        Debug.Log(randomNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if (randomNumber == 0)
        {
            CheckFlag();
            if (!atDestination && waiting == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
            }
            if (moveBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
            }

            if (waiting == true)
            {
                waitCounter += Time.deltaTime;
            }
            if (waitCounter >= waitTime)
            {
                moveBack = true;
            }

        }

    }

    void CheckFlag()
    {
        //if we're close enough to PointA, set atDestination to true.
        if (Vector3.Distance(transform.position, pointA.position) < 0.05)
        {
            atDestination = true;
            waiting = true;
        }
    }
}
