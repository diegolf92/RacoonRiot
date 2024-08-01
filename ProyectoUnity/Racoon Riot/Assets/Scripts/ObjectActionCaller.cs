using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActionCaller : MonoBehaviour
{
    public int actionInt;

    public void ActionsListInt()
    {
        switch (actionInt)
        {
            case 1: ElevatorObject();
                break;

            default: ActionDefault();
                    break;
        
        }
    }

    public void ActionDefault()
    {
        Debug.Log("Turn on TV");
    }

    public void ElevatorObject()
    {
        GetComponent<Elevator>().MoveElevator();
    }
}
