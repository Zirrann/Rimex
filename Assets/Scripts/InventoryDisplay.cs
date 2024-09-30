using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MouseItemData mouseInventoryItem;

    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlotUI, InventorySlot> slotsDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlotUI, InventorySlot> SlotsDictionary => slotsDictionary;

    public abstract void AssingSlot(InventorySystem invSystem);

    protected virtual void Start() 
    { 
    
    }

    protected virtual void UpdateSlot(InventorySlot updatedSlot) 
    {
        foreach (var slot in slotsDictionary) 
        {
            if (slot.Value == updatedSlot)
            { 
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    public void SlotClicked(InventorySlotUI slot)
    {
        Debug.Log("Slot clicked");
    }
}
