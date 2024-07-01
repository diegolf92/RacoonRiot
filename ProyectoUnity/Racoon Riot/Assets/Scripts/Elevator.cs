using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform player;
    public Transform elevatorSwitch;
    public Transform downPos;
    public Transform upperPos;

    public float speed;
    private bool isElevatorDown;

    private void Update()
    {
        StartElevator();
    }

    private void StartElevator()
    {
        if(Vector2.Distance(player.position, elevatorSwitch.position)<0.5f && Input.GetKeyDown(KeyCode.E))
        {
            if(transform.position.y <= downPos.position.y)
            {
                isElevatorDown = true;
            }
            else if (transform.position.y >= upperPos.position.y)
            { 
                isElevatorDown = false;
            }

        }

        if(isElevatorDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, upperPos.position,speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, downPos.position, speed * Time.deltaTime);
        }
    }
}
