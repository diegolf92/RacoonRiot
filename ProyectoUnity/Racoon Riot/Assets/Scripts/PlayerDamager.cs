using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public CheckPointManager checkpoint;

    //Si queremos enemigos/hazards que sean solidos
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && other.gameObject.activeInHierarchy)
        {
            checkpoint.Reviver();
        }
    }
}
