using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    private Vector2 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = rb.position;
    }

    void Update()
    {
        // Check if the object has fallen
        if (!hasFallen && rb.velocity.y != 0)
        {
            // If the object is falling, mark it as fallen
            hasFallen = true;
        }

        // If the object has fallen, make it immovable
        if (hasFallen && Mathf.Approximately(rb.velocity.y, 0))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    // Reset the object (optional, for testing or level reset purposes)
    public void ResetObject()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.position = initialPosition;
        hasFallen = false;
    }
}