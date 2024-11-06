using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime = 0.2f; // Time to wait before re-enabling platform collision
    private float originalWaitTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        originalWaitTime = waitTime;
    }

    void Update()
    {
        // When the player presses down, allow them to fall through
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            effector.rotationalOffset = 180f;  // Temporarily rotate to allow passing through
            waitTime = originalWaitTime;       // Reset wait time countdown
        }

        // Restore platform's pass-through state after the wait time
        if (Input.GetKey(KeyCode.DownArrow))
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 0; // Reset to default rotation
                waitTime = originalWaitTime;
            }
        }

        // Reset rotation if the player stops pressing down
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            effector.rotationalOffset = 0;
            waitTime = originalWaitTime;
        }
    }
}