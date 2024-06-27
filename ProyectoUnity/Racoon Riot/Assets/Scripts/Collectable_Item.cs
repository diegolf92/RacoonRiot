using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectable_Item : MonoBehaviour
{
    public Item item;
    public static Action<Item, Vector3> OnCollect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect?.Invoke(item, transform.position);
            Destroy(gameObject);
        }
    }
}
