using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInvetorySlotChanged;

    public InventorySystem(int sieze)
    {
        inventorySlots = new List<InventorySlot>();

        for (int i = 0; i < sieze; i++) 
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool Add(InventoryItedmData item, int amount) 
    {

        if (ContainsItems(item, out List<InventorySlot> invSlots))
        {
            foreach (var invSlot in invSlots)
            {
                if (invSlot.RoomLeftInStack(amount))
                {   
                    invSlot.AddToStack(amount);
                    OnInvetorySlotChanged?.Invoke(invSlot);
                    return true;
                }
            }  
        }
        
        if (HasFreeSlot(out InventorySlot newSlot)) 
        { 
            newSlot.UpdateInventorySlot(item, amount);
            OnInvetorySlotChanged?.Invoke(newSlot);
            return true;
        }

        return false;
    }

    private bool HasFreeSlot(out InventorySlot slot)
    {
        slot = InventorySlots.FirstOrDefault(item => item.ItemData is null);
        return slot is not null;
    }

    public bool ContainsItems(InventoryItedmData item, out List<InventorySlot> slots)
    {
        slots = InventorySlots.Where(i => i.ItemData == item).ToList();

        return slots.Count > 0;
    }

}
