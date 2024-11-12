using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DogWaves : MonoBehaviour
{
    public float speed = 5f;   // Speed of the object
    public int direction;  // 0 for left, 1 for right

    void Start()
    {
        if (direction == 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        // Destroy the object after 4 seconds
        Destroy(gameObject, 4f);
    }

    void Update()
    {
        // Calculate the movement direction
        Vector3 movementDirection = direction == 0 ? Vector3.left : Vector3.right;

        // Move the object in the specified direction
        transform.Translate(movementDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.GetComponent<PlayerController>().GotDamaged();
        }
    }
}
