using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool hasFallen = false;
    private bool isBeingInteractedWith = false;
    private Vector2 initialPosition;

    public float defaultMass;
    public float immovableMass;
    public bool beingPushed;
    public bool canBePulled;  // New flag to determine if the object can be pulled
    private float xPos;
    public Vector3 lastPos;

    public int mode;
    public int colliding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = rb.position;
        xPos = transform.position.x;
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        if (mode == 0)
        {
            if (!beingPushed)
            {
                transform.position = new Vector3(xPos, transform.position.y);
            }
            else
            {
                xPos = transform.position.x;
            }
        }
        else if (mode == 1)
        {
            if (!beingPushed)
            {
                rb.mass = immovableMass;
            }
            else
            {
                rb.mass = defaultMass;
            }
        }

        // Check if the object has fallen
        if (!hasFallen && rb.velocity.y != 0)
        {
            hasFallen = true;
        }

        // If the object has fallen and is not being interacted with, make it immovable
        if (hasFallen && !isBeingInteractedWith && Mathf.Approximately(rb.velocity.y, 0))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    // Method to set interaction state
    public void SetInteractedState(bool state)
    {
        isBeingInteractedWith = state;
        beingPushed = state;

        if (state)
        {
            // Ensure the object is movable when being interacted with
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = defaultMass;
        }
        else
        {
            rb.mass = immovableMass;
        }
    }

    // Reset the object (optional, for testing or level reset purposes)
    public void ResetObject()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.position = initialPosition;
        hasFallen = false;
        isBeingInteractedWith = false;
        beingPushed = false;
        xPos = transform.position.x;
        lastPos = transform.position;
        rb.mass = defaultMass;
    }
}