using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public CheckPointManager manager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(manager.currentCheckpoint != this)
            {
                manager.ChangeCheckPoint(this);
            }
        }
    }
}
