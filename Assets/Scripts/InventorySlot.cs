using Assets.Scripts;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItedmData itemData;
    [SerializeField] private int stackSize;

    public InventoryItedmData ItemData => itemData;
    public int StackSize => stackSize;

    public InventorySlot(InventoryItedmData itemData, int stackSize) 
    { 
        this.itemData = itemData;
        this.stackSize = stackSize;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot() 
    {
        itemData = null;
        stackSize = -1;
    }

    public void UpdateInventorySlot(InventoryItedmData itemData, int amount) 
    {
        this.itemData = itemData;
        stackSize = amount;
    }

    public void AddToStack(int amount) 
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount) 
    { 
        stackSize -= amount;
    }

    public bool RoomLeftInStack(int amountToAdd, out int reamainingSize)
    {
        reamainingSize = ItemData.maxSizeStack - stackSize;

        return RoomLeftInStack(amountToAdd);
    }

    public bool RoomLeftInStack(int amountToAdd) 
    {
        return stackSize + amountToAdd <= itemData.maxSizeStack;
    }

    public void AssignItem(InventorySlot invSlot)
    {
        if(itemData == invSlot.itemData) AddToStack(invSlot.stackSize);
        else 
        { 
            itemData = invSlot.itemData;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }

    public bool SplitStack(out InventorySlot splitSlot) 
    {
        if (stackSize <= 1) 
        { 
            splitSlot = null;
            return false;
        }

        int halfStack = stackSize / 2;
        splitSlot = new InventorySlot(itemData, stackSize - halfStack);
        RemoveFromStack(halfStack);

        return true;
    }
}
