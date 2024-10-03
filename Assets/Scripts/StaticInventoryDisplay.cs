using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlotUI[] slots;
    

    protected override void Start()
    {
        base.Start();

        if (inventoryHolder is not null)
        {
            inventorySystem = inventoryHolder.InventorySystem;
            inventorySystem.OnInvetorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory slot, {this.gameObject}");

        AssingSlot(inventorySystem);
    }

    public override void AssingSlot(InventorySystem invSystem)
    {
        slotsDictionary = new Dictionary<InventorySlotUI, InventorySlot>();
        Debug.Log($"{slots.Length}");

        for (int i = 0; i < inventorySystem.InventorySize ; i++)
        {
            Debug.Log(i);
            if (slots[i] is null) Debug.Log("null");
            if (inventorySystem.InventorySlots[i] is null) Debug.Log("inv null");
            slotsDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
}
