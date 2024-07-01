using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwitch : MonoBehaviour
{
    public Transform player;
    public Transform switchPosition;
    public Elevator elevator;
    public float interactionRange = 1f;

    private void Update()
    {
        if (Vector2.Distance(player.position, switchPosition.position) < interactionRange && Input.GetKeyDown(KeyCode.E))
        {
            elevator.ToggleElevator();
        }
    }
}

