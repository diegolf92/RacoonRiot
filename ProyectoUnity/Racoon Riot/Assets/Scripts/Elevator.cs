using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform downPos;
    public Transform upperPos;
    public float speed;

    private bool isElevatorDown = true;
    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            MoveElevator();
        }
    }

    private void MoveElevator()
    {
        if (isElevatorDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, upperPos.position, speed * Time.deltaTime);
            if (transform.position.y >= upperPos.position.y)
            {
                isElevatorDown = false;
                isMoving = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, downPos.position, speed * Time.deltaTime);
            if (transform.position.y <= downPos.position.y)
            {
                isElevatorDown = true;
                isMoving = false;
            }
        }
    }

    public void ToggleElevator()
    {
        if (!isMoving)
        {
            isMoving = true;
        }
    }
}
