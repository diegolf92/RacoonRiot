using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask boxMask;
    private GameObject box;
    private FixedJoint2D boxJoint;
    private Collider2D playerCollider;

    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Ensure raycasts don't start inside colliders
        Physics2D.queriesStartInColliders = false;

        // Calculate the center of the player's collider
        Vector2 rayOrigin = playerCollider.bounds.center;

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * transform.localScale.x, distance, boxMask);

        // Check if the raycast hits a pushable object and the interaction key is pressed
        if (hit.collider != null && hit.collider.CompareTag("pushable") && Input.GetKey(KeyCode.E))
        {
            // Get the hit object
            box = hit.collider.gameObject;

            // Get or add the FixedJoint2D component
            boxJoint = box.GetComponent<FixedJoint2D>();
            if (boxJoint == null)
            {
                boxJoint = box.AddComponent<FixedJoint2D>();
            }

            // Enable the FixedJoint2D and connect to the player
            boxJoint.enabled = true;
            boxJoint.connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            // Disable the FixedJoint2D when the interaction key is released
            if (box != null && boxJoint != null)
            {
                boxJoint.enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();
        }

        // Calculate the center of the player's collider for drawing the ray
        Vector2 rayOrigin = playerCollider.bounds.center;

        // Draw the raycast line for debugging purposes
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.right * transform.localScale.x * distance);
    }
}