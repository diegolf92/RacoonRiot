using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;  
    public Transform pointB;  
    public float speed = 2f;  

    private Vector3 targetPosition;  

    void Start()
    {
        targetPosition = pointB.position;
    }

    void Update()
    {
        // Move the platform towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the platform has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Switch the target position between point A and point B
            if (targetPosition == pointA.position)
            {
                targetPosition = pointB.position;
            }
            else
            {
                targetPosition = pointA.position;
            }
        }
    }

    // Method to dynamically change the platform speed
    public void SetPlatformSpeed(float newSpeed)
    {
        speed = newSpeed;  // Update the platform's speed
    }
}