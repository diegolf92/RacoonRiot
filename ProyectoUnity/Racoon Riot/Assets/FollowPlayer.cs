using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public Vector3 offset = new Vector3(0, 1, 0); // Offset from the player's position

    void Update()
    {
        // Update the spacebar's position relative to the player
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }
    }
}
