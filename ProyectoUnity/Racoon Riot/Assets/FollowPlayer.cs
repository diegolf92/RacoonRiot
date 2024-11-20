using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1, 0); // Offset from the player's position

    public void ShowBar(Transform playerTransform)
    {
        transform.position = playerTransform.position + offset;
    }
}
