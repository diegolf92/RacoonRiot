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
        if (!hasFallen && rb.velocity.y != 0)
        {
            hasFallen = true;
        }

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