using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemsSc : MonoBehaviour
{
    public float amplitude = 0.5f; // How much the object floats up and down
    public float frequency = 1f;   // The speed of the floating motion
    public bool isKeyItem;
    public Victory victory;

    private Vector3 startPos;

    void Start()
    {
        // Store the starting position of the object
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Apply the new position to the object
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isKeyItem)
        {
            victory.canWin = true;
        }
    }
}
