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
        Physics2D.queriesStartInColliders = false;

        Vector2 rayOrigin = playerCollider.bounds.center;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * transform.localScale.x, distance, boxMask);

        if (hit.collider != null && hit.collider.CompareTag("Pushable") && Input.GetKey(KeyCode.E))
        {
            box = hit.collider.gameObject;
            boxJoint = box.GetComponent<FixedJoint2D>();

            // Ensure the object is in dynamic state when interacting
            PushableObject pushableObject = box.GetComponent<PushableObject>();
            if (pushableObject != null)
            {
                // Only interact if the object can be pushed (and possibly pulled)
                if (!pushableObject.canBePulled && Vector2.Dot(transform.localScale, (box.transform.position - transform.position).normalized) < 0)
                {
                    // Skip interaction if the object cannot be pulled
                    return;
                }

                pushableObject.SetInteractedState(true);
            }

            if (boxJoint == null)
            {
                boxJoint = box.AddComponent<FixedJoint2D>();
            }

            boxJoint.enabled = true;
            boxJoint.connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (box != null && boxJoint != null)
            {
                boxJoint.enabled = false;

                PushableObject pushableObject = box.GetComponent<PushableObject>();
                if (pushableObject != null)
                {
                    pushableObject.SetInteractedState(false);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();
        }

        Vector2 rayOrigin = playerCollider.bounds.center;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.right * transform.localScale.x * distance);
    }
}