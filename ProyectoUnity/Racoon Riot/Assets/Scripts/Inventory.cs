using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    public List<ItemStack> items = new List<ItemStack>();

}

[SerializeField]

public class ItemStack
{
    public Item item;
    public int quantity;
    public bool stackeable;
}