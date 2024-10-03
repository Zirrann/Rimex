using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Assets.Scripts
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image itemSprite;
        [SerializeField] private TextMeshProUGUI itemCount;
        [SerializeField] private InventorySlot assingnedInventorySlot;

        private Button button;

        public InventorySlot AssignedInventorySlot => assingnedInventorySlot;
        public InventoryDisplay ParentDisplay { get; private set; }

        private void Awake()
        {
            ClearSlot();

            button = GetComponent<Button>();
            button?.onClick.AddListener(OnUISlotClick);

            ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
        }

        public void Init(InventorySlot slot)
        {
            assingnedInventorySlot = slot;
            UpdateUISlot(slot);
        }

        public void UpdateUISlot(InventorySlot slot)
        {   
            if (slot.ItemData is not null) 
            { 
                itemSprite.sprite = slot.ItemData.Icon;
                itemSprite.color = Color.white;

                if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
                else itemCount.text = "";
            }
            else
            {
                ClearSlot();
            }
        }

        public void UpdateUISlot() 
        {
            if(assingnedInventorySlot != null) UpdateUISlot(assingnedInventorySlot);          
        }

        public void ClearSlot() 
        {
            assingnedInventorySlot?.ClearSlot();
            itemSprite.sprite = null;
            itemSprite.color = Color.clear;
            itemCount.text = null;
        }

        public void OnUISlotClick()
        {
            ParentDisplay?.SlotClicked(this);
        }
    }
}