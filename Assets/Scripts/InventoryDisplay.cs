using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
        bool shiftPressed = Keyboard.current.leftShiftKey.isPressed;
        if (slot.AssignedInventorySlot.ItemData is not null && mouseInventoryItem.AssignedInventorySlot.ItemData is null) 
        {
            if (shiftPressed && slot.AssignedInventorySlot.SplitStack(out InventorySlot splitSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(splitSlot);
                slot.UpdateUISlot();
            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(slot.AssignedInventorySlot);
                slot.ClearSlot();
            }
            return;
        }

        if (slot.AssignedInventorySlot.ItemData is null && mouseInventoryItem.AssignedInventorySlot.ItemData is not null) 
        {
            slot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            slot.UpdateUISlot();
            mouseInventoryItem.ClearSlot();
            return;
        }

        if (slot.AssignedInventorySlot.ItemData is not null && mouseInventoryItem.AssignedInventorySlot.ItemData is not null) 
        {
            bool isSameItem = slot.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;

            if (isSameItem && slot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                Debug.Log("jest mijsce");
                slot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                slot.UpdateUISlot();
                mouseInventoryItem.ClearSlot();

                return;
            }
            else if(isSameItem && !slot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int roomLeft ))
            {
                Debug.Log("cos");
                if (roomLeft == 0) SwapSlots(slot);
                else 
                {
                    Debug.Log("brak mijsce");
                    slot.AssignedInventorySlot.AddToStack(roomLeft);
                    slot.UpdateUISlot();

                    var mouseSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData,
                                    mouseInventoryItem.AssignedInventorySlot.StackSize - roomLeft);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(mouseSlot);
                }

                return;
            }

             SwapSlots(slot);  
        }
    }

    private void SwapSlots(InventorySlotUI slot) 
    {
        var tmpSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();

        mouseInventoryItem.UpdateMouseSlot(slot.AssignedInventorySlot);
        slot.ClearSlot();
        slot.AssignedInventorySlot.AssignItem(tmpSlot);
        slot.UpdateUISlot();
    }
}
