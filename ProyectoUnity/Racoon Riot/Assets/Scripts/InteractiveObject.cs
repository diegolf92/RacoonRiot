using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public ObjectActionCaller objectToTrigger;
    bool canTrigger;

    void Update()
    {
        if(canTrigger && Input.GetKeyDown(KeyCode.E))
        {
            objectToTrigger.ActionsListInt();
            //enemigo camine hacia objectToTrigger
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            canTrigger = true;
        }        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            canTrigger = false;
        }        
    }
}
