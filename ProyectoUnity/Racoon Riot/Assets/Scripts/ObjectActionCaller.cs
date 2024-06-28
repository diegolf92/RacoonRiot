using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActionCaller : MonoBehaviour
{
    public int actionInt;

    public void ActionsListInt()
    {
        if(actionInt == 0)
        {
            ActionOne();
        }
    }

    public void ActionOne()
    {
        Debug.Log("Turn on TV");
    }
}
