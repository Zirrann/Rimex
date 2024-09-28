using Assets.Scripts;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]

public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1f;
    public InventoryItedmData ItemData;

    private SphereCollider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponent<InventoryHolder>();

        if (!inventory) return;

        if (inventory.InventorySystem.Add(ItemData, 1)) 
        { 
            Destroy(this.gameObject);
        }
    }
}