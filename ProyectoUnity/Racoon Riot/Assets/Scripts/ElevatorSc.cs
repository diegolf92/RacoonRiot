using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSc : MonoBehaviour
{
    public Transform upperPoint;
    public Transform lowerPoint;
    public float speed = 2.0f;
    private Vector3 targetPosition;
    private bool isActivated = false;

    void Start()
    {
        // Start at the lower point
        targetPosition = lowerPoint.position;
    }

    void Update()
    {
        if (isActivated)
        {
            MoveElevator();
        }
    }

    private void MoveElevator()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isActivated = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateElevator();
        }
    }

    public void ActivateElevator()
    {
        isActivated = true;
        if (targetPosition == lowerPoint.position)
        {
            targetPosition = upperPoint.position;
        }
        else
        {
            targetPosition = lowerPoint.position;
        }
    }
}
