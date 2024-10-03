using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class MouseItemData : MonoBehaviour
{
    public Image ItemImage;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        ItemImage.sprite = invSlot.ItemData.Icon;
        ItemCount.text = invSlot.StackSize.ToString();
        ItemImage.color = Color.white;
    }

    private void Awake()
    {
        ItemImage.color = Color.clear;
        ItemCount.text = "";
    }

    public void LateUpdate()
    {
        if (AssignedInventorySlot.ItemData is not null) 
        { 
            transform.position = Mouse.current.position.ReadValue();

/*            if (Mouse.current.leftButton.wasPressedThisFrame && IsPointerOverUIObject())
            {
                ClearSlot();
            }*/
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemImage.color = Color.clear;
        ItemImage.sprite = null;
    }

    public static bool IsPointerOverUIObject() 
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}
 