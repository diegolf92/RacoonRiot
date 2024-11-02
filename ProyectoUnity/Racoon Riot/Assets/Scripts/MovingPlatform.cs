using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;  // The first position (A)
    public Transform pointB;  // The second position (B)
    public float speed = 2f;  // Speed of platform movement

    private Vector3 targetPosition;  // The current target position

    void Start()
    {
        // Start by moving towards point B
        targetPosition = pointB.position;
    }

    void Update()
    {
        // Move the platform towards the current target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the platform has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // If at pointA, switch to pointB, and vice versa
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

    // Trigger event when player steps on the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Make sure the platform is active before setting the parent
            if (gameObject.activeInHierarchy)
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    // Trigger event when player leaves the platform
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Remove the player from the platform's parent only if the platform is active
            if (gameObject.activeInHierarchy)
            {
                collision.transform.SetParent(null);
            }
        }
    }
}