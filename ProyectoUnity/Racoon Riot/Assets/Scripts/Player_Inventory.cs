using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;

    private void Start()
    {
        CollectableItem.OnCollect += CollectItem;
    }

    private void CollectItem(Item item, Vector3 position)
    {
        ItemStack stack = inventory.items.Find(s => s.item == item);
        if (stack != null)
        {
            if (stack.stackeable)
            {
                stack.quantity++;
            }
            else
            {
                NewEntry(item, item.stackeable);
            }
        }
        else
        {
            NewEntry(item, item.stackeable);
        }
    }

    private void NewEntry(Item item, bool stackeable)
    {
        inventory.items.Add(new ItemStack { item = item, quantity = 1, stackeable = stackeable });
    }
    private void OnDestroy()
    {
        CollectableItem.OnCollect -= CollectItem;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().CapturingPlayer();
        } else if (other.transform.tag == "MovingEnemy")
        {
            other.GetComponent<MovingEnemy>().CapturingPlayer();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Enemy")
        {
            Debug.Log("Enemy scape");
        } 
    }
}
