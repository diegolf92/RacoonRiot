using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Inventory : MonoBehaviour
{
    public Inventory inventory;

    private void Start()
    {
        Collectable_Item.OnCollect += CollectItem;
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
        Collectable_Item.OnCollect -= CollectItem;
    }
}
